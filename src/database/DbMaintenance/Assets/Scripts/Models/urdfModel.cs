﻿using System;
using Mono.Data.Sqlite;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;


/// <summary>
/// This is a model of urdfs stored in the database
/// </summary>
public class UrdfItem
{
    public int uid;
    public string name;
    public string modelNumber;
    public float internalCost;
    public float externalCost;
    public float weight;
    public float powerUsage;
    public int fk_type_id;
    public int fk_category_id;
    public int usable;
    public string urdfFilename;
    public string prefabFilename;
    public float time;
    // TODO: Add validation code
}

/// <summary>
/// This is a model of urdf types within the database
/// </summary>
public class UrdfType
{
    public int uid;
    public string name;
    // TODO: Add validation code
}

/// <summary>
/// This is a model of sensor categories within the database
/// </summary>
public class SensorCategories
{
    public int uid;
    public string name;
    // TODO: Add validation code here for setting data
}

/// <summary>
/// This is the model for URDF interactions which contains actions to interact with the DB
/// </summary>
public class UrdfModel
{
    private DbEngine _engine;

    /// <summary>
    /// Constructor for the urdf model by providing a connection string
    /// </summary>
    /// <param name="connString">string representation to connect to the sqlite db</param>
    public UrdfModel(string connString)
    {
        _engine = new DbEngine(connString);
    }

    /// <summary>
    /// Constructor for the urdf model by providing an existing DbEngine to midigate the number of connections
    /// to the database.
    /// </summary>
    /// <param name="engine">DbEngine with an esitablished connection to a sqlite database</param>
    public UrdfModel(DbEngine engine)
    {
        if (engine.HasConnection())
        {
            _engine = engine;
        }
        else
        {
            throw new Exception("No connection has been established yet.");
        }
    }

    /// <summary>
    /// This will query for all sensors in the database
    /// </summary>
    /// <returns>A list of all sensors/urdfs in the database.</returns>
    public List<UrdfItem> GetSensors()
    {
        List<UrdfItem> list = new List<UrdfItem>();

        string sql = "SELECT `uid`, `name`, `modelNumber`, `internalCost`, `externalCost`, `weight`, `powerUsage`, `fk_type_id`, `fk_category_id`, `usable`, `urdfFilename`, `prefabFilename`, `time` FROM `tblUrdfs`;";
        SqliteCommand cmd = _engine.conn.CreateCommand();
        cmd.CommandText = sql;
        SqliteDataReader reader = cmd.ExecuteReader();
        UrdfItem item;
        try
        {
            while(reader.Read())
            {
                item = new UrdfItem();
                item.uid = reader.GetInt32(0);
                item.name = (!reader.IsDBNull(1) ? reader.GetString(1) : String.Empty);
                item.modelNumber = (!reader.IsDBNull(2) ?  reader.GetString(2) : String.Empty);
                item.internalCost = (!reader.IsDBNull(3) ?  reader.GetFloat(3) : 0.0f);
                item.externalCost = (!reader.IsDBNull(4) ?  reader.GetFloat(4) : 0.0f);
                item.weight = (!reader.IsDBNull(5) ?  reader.GetFloat(5) : 0.0f);
                item.powerUsage = (!reader.IsDBNull(6) ?  reader.GetFloat(6) : 0.0f);
                item.fk_type_id = (!reader.IsDBNull(7) ?  reader.GetInt32(7) : 0);
                item.fk_category_id = (!reader.IsDBNull(8) ?  reader.GetInt32(8) : 0);
                item.usable = (!reader.IsDBNull(9) ?  reader.GetInt32(9) : 0);
                item.urdfFilename = (!reader.IsDBNull(10) ?  reader.GetString(10) : String.Empty);
                item.prefabFilename = (!reader.IsDBNull(11) ?  reader.GetString(11) : String.Empty);
                item.time = (!reader.IsDBNull(12) ? reader.GetFloat(12) : 0.0f);

                list.Add(item);
            }
        } 
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            reader.Close();
            reader = null;
            cmd.Dispose();
            cmd = null;
        }

        return list;
    }

    /// <summary>
    /// This will grab all the urdf types that is known in the database
    /// </summary>
    /// <returns>A list of type of urdf in the database.</returns>
    public List<UrdfType> GetUrdfTypes()
    {
        List<UrdfType> list = new List<UrdfType>();

        string sql = "SELECT `uid`, `name` FROM `tblUrdfType`;";
        SqliteCommand cmd = _engine.conn.CreateCommand();
        cmd.CommandText = sql;
        SqliteDataReader reader = cmd.ExecuteReader();
        UrdfType item;
        try
        {
            while (reader.Read())
            {
                item = new UrdfType();
                item.uid = reader.GetInt32(0);
                item.name = reader.GetString(1);

                list.Add(item);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            reader.Close();
            reader = null;
            cmd.Dispose();
            cmd = null;
        }

        return list;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<SensorCategories> GetSensorCategories()
    {
        List<SensorCategories> list = new List<SensorCategories>();

        string sql = "SELECT `uid`, `name` FROM `tblSensorCategories`;";
        IDbCommand cmd = _engine.conn.CreateCommand();
        cmd.CommandText = sql;
        IDataReader reader = cmd.ExecuteReader();
        SensorCategories item;
        try
        {
            while (reader.Read())
            {
                item = new SensorCategories();
                item.uid = reader.GetInt32(0);
                item.name = reader.GetString(1);

                list.Add(item);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            reader.Close();
            reader = null;
            cmd.Dispose();
            cmd = null;
        }

        return list;
    }

    /// <summary>
    /// This will add a given UrdfItem into the database with provided details.
    /// </summary>
    /// <param name="item">UrdfItem to be added into the database</param>
    /// <returns>The insert id of the newely added id. 0 if it was not added successfully.</returns>
    public int AddSensor(UrdfItem item)
    {
        long lastId = 0;
        string sql = "INSERT INTO `tblUrdfs` (`name`, `modelNumber`, `internalCost`, `externalCost`, `weight`, `powerUsage`, `fk_type_id`, `fk_category_id`, `usable`, `urdfFilename`, `prefabFilename`) VALUES (@name, @modelNumber, @internalCost, @externalCost, @weight, @powerUsage, @fk_type_id, @fk_category_id, @usable, @urdfFilename, @prefabFilename);";
        SqliteCommand cmd = _engine.conn.CreateCommand();
        cmd.CommandText = sql;
        
        cmd.Parameters.AddWithValue("@name", item.name);
        cmd.Parameters.AddWithValue("@modelNumber", item.modelNumber);
        cmd.Parameters.AddWithValue("@internalCost", item.internalCost);
        cmd.Parameters.AddWithValue("@externalCost", item.externalCost);
        cmd.Parameters.AddWithValue("@weight", item.weight);
        cmd.Parameters.AddWithValue("@powerUsage", item.powerUsage);
        cmd.Parameters.AddWithValue("@fk_type_id", item.fk_type_id);
        cmd.Parameters.AddWithValue("@fk_category_id", item.fk_category_id);
        cmd.Parameters.AddWithValue("@usable", item.usable);
        cmd.Parameters.AddWithValue("@urdfFilename", item.urdfFilename);
        cmd.Parameters.AddWithValue("@prefabFilename", item.prefabFilename);

        try
        {
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT last_insert_rowid();";
            lastId = (long)cmd.ExecuteScalar();
        }
        catch (SqliteException ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            cmd.Dispose();
            cmd = null;
        }

        return (int)lastId;
    }

    /// <summary>
    /// Update the given UrdfItem in the database with the given values.
    /// </summary>
    /// <param name="item">The UrdfItem that is to be updated in the database.</param>
    /// <returns>The number of rows affected by the update. 0 if the id was not found.</returns>
    public int UpdateSensor(UrdfItem item)
    {
        string sql = "UPDATE `tblUrdfs` SET `name` = @name, `modelNumber` = @modelNumber, `internalCost` = @internalCost, `externalCost` = @externalCost, `weight` = @weight, `powerUsage` = @powerUsage, `fk_type_id` = @fk_type_id, `fk_category_id` = @fk_category_id, `usable` = @usable, `urdfFilename` = @urdfFilename, `prefabFilename` = @prefabFilename WHERE `uid` = @uid;";
        SqliteCommand cmd = _engine.conn.CreateCommand();
        cmd.CommandText = sql;

        cmd.Parameters.AddWithValue("@uid", item.uid);
        cmd.Parameters.AddWithValue("@name", item.name);
        cmd.Parameters.AddWithValue("@modelNumber", item.modelNumber);
        cmd.Parameters.AddWithValue("@internalCost", item.internalCost);
        cmd.Parameters.AddWithValue("@externalCost", item.externalCost);
        cmd.Parameters.AddWithValue("@weight", item.weight);
        cmd.Parameters.AddWithValue("@powerUsage", item.powerUsage);
        cmd.Parameters.AddWithValue("@fk_type_id", item.fk_type_id);
        cmd.Parameters.AddWithValue("@fk_category_id", item.fk_category_id);
        cmd.Parameters.AddWithValue("@usable", item.usable);
        cmd.Parameters.AddWithValue("@urdfFilename", item.urdfFilename);
        cmd.Parameters.AddWithValue("@prefabFilename", item.prefabFilename);

        int affectedRows = 0;
        try
        {
            affectedRows = cmd.ExecuteNonQuery();
        }
        catch (SqliteException ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            cmd.Dispose();
            cmd = null;
        }

        return affectedRows;
    }

    /// <summary>
    /// This allows for flexibilty of deleting a UrdfItem with the object
    /// </summary>
    /// <param name="item">UrdfItem that is to be removed in the database</param>
    /// <returns>The number of rows affected. 0 if the id was not found</returns>
    public int DeleteSensor(UrdfItem item)
    {
        return DeleteSensor(item.uid);
    }

    /// <summary>
    /// Give the unique id of the sensor this method will remove it from the DB if it exists.
    /// </summary>
    /// <param name="urdfItemId">int representation of the sensor id in the database</param>
    /// <returns>The number of rows there were affected by the delete statement, 0 if the id was not found</returns>
    public int DeleteSensor(int urdfItemId)
    {
        int affectedRows = 0;
        string sql = "DELETE FROM `tblUrdfs` WHERE `uid` = @uid";
        SqliteCommand cmd = _engine.conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.Parameters.AddWithValue("@uid", urdfItemId);

        try
        {
            affectedRows = cmd.ExecuteNonQuery();
        }
        catch (SqliteException ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            cmd.Dispose();
            cmd = null;
        }
        
        return affectedRows;
    }

}
