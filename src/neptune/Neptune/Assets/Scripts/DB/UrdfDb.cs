﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;


/// <summary>
/// This is the model for URDF interactions which contains actions to interact with the DB
/// </summary>
public class UrdfDb
{
    private string CONN_STRING = "../../../db/data.csv";
    private string usingFilepath;
    private string firstLine = "";
    private List<UrdfItemModel> UrdfItems;
    private bool tryOnce = true;

    /// <summary>
    /// Constructor for the urdf model using the default connection string
    /// </summary>
    /// <param name="connString">string representation to connect to the sqlite db</param>
    public UrdfDb()
    {
        SetupData(CONN_STRING);
    }

    /// <summary>
    /// Constructor for the urdf model by providing an existing DbEngine to midigate the number of connections
    /// to the database.
    /// </summary>
    /// <param name="file">File path to </param>
    public UrdfDb(string file)
    {
        SetupData(file);
    }

    private void SetupData(string filepath)
    {
        this.usingFilepath = filepath;
        this.UrdfItems = new List<UrdfItemModel>();
        FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        
        using (StreamReader sr = new StreamReader(fs))
        {
            string line = "";
            UrdfItemModel item;
            bool firstline = true;
            while ((line = sr.ReadLine()) != null)
            {
                if (firstline)
                {
                    firstLine = line;
                    firstline = false;
                    continue;
                }
                else
                {
                    try
                    {
                        item = new UrdfItemModel();
                        item.extract(line.Split('\t'));
                        this.UrdfItems.Add(item);
                    }
                    catch (Exception)
                    {
                        // log invalid row in data file
                    }
                }
            }
        }
        fs.Close();
    }

    public void Save()
    {
        try
        {
            using (StreamWriter file = new StreamWriter(this.usingFilepath))
            {
                file.WriteLine(firstLine);
                foreach(UrdfItemModel item in this.UrdfItems)
                {
                    file.WriteLine(item.GetCSV());
                }
            }
        }
        catch (IOException)
        {
            // file is open as read-only create new file for back
            this.usingFilepath = this.usingFilepath + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss.csv");
            if (tryOnce)
            {
                tryOnce = false;
                Save();
            }
        }
    }


    /// <summary>
    /// This will query for all urdfs in the database. 
    /// </summary>
    /// <returns>A list of all urdfs in the database.</returns>
    public List<UrdfItemModel> GetUrdfs()
    {
        return this.UrdfItems;
    }


    /// <summary>
    /// This will query for all usable sensors in the database by default unless otherwise specified. 
    /// A sensor is usable if it has an appropriate mesh and data associated to it. 
    /// </summary>
    /// <param name="all">Inclusion of sensors that are not usable</param>
    /// <returns>A list of all sensors/urdfs in the database.</returns>
    public List<UrdfItemModel> GetSensors(bool all = false)
    {
        return this.UrdfItems.FindAll(delegate (UrdfItemModel m) { return m.type == "sensor"; });
    }
    /// <summary>
    /// This will query for all usable robots in the database by default unless otherwise specified. 
    /// A robot is usable if it has an appropriate mesh and data associated to it. 
    /// </summary>
    /// <returns>A list of all robots/urdfs in the database.</returns>
    public List<UrdfItemModel> GetRobots()
    {
        return this.UrdfItems.FindAll(delegate(UrdfItemModel m) { return m.type == "robot"; }) ;
    }

    /// <summary>
    /// This will grab all the urdf types that is known in the database
    /// </summary>
    /// <returns>A list of type of urdf in the database.</returns>
    public List<string> GetUrdfTypes()
    {
        List<string> list = new List<string>();
        foreach (UrdfItemModel model in this.UrdfItems)
        {
            if (!list.Contains(model.type))
            {
                list.Add(model.type);
            }
        }

        return list;
    }

    /// <summary>
    /// Retrieves the categories the list of categories within the database.
    /// </summary>
    /// <returns>A List of categories found. List may be empty.</returns>
    public List<string> GetSensorCategories()
    {
        List<string> list = new List<string>();
        foreach(UrdfItemModel model in this.UrdfItems)
        {
            if(!list.Contains(model.category))
            {
                list.Add(model.category);
            }
        }

        return list;
    }

    /// <summary>
    /// This will add a given UrdfItemModel into the database with provided details.
    /// </summary>
    /// <param name="item">UrdfItemModel to be added into the database</param>
    /// <returns>The insert id of the newely added id. 0 if it was not added successfully.</returns>
    public int AddSensor(UrdfItemModel item)
    {
        item.uid = this.UrdfItems.Count+1;
        this.UrdfItems.Add(item);
        return this.UrdfItems.Count;
    }

    /// <summary>
    /// Update the given UrdfItemModel in the database with the given values.
    /// </summary>
    /// <param name="item">The UrdfItemModel that is to be updated in the database.</param>
    /// <returns>The number of rows affected by the update. 0 if the id was not found.</returns>
    public int UpdateSensor(UrdfItemModel item)
    {
        try
        {
            this.UrdfItems[item.uid-1] = item;
            return 1;
        }
        catch (Exception)
        {
            return 0;
        }
    }

    /// <summary>
    /// This allows for flexibilty of deleting a UrdfItemModel with the object
    /// </summary>
    /// <param name="item">UrdfItemModel that is to be removed in the database</param>
    /// <returns>The number of rows affected. 0 if the id was not found</returns>
    public int DeleteSensor(UrdfItemModel item)
    {
        return DeleteSensor(item.uid);
    }

    /// <summary>
    /// Give the unique id of the sensor this method will remove it from the DB if it exists.
    /// </summary>
    /// <param name="UrdfItemModelId">int representation of the sensor id in the database</param>
    /// <returns>The number of rows there were affected by the delete statement, 0 if the id was not found</returns>
    public int DeleteSensor(int UrdfItemModelId)
    {
        try
        {
            UrdfItems.RemoveAt(UrdfItemModelId-1);
            return 1;
        }
        catch (ArgumentOutOfRangeException)
        {
            return 0;
        }
    }

}
