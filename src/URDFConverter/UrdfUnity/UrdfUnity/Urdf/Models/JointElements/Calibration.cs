﻿namespace UrdfUnity.Urdf.Models.JointElements
{
    /// <summary>
    /// Represents the reference positions of the joint, used to calibrate the absolute position of the joint. 
    /// </summary>
    /// <seealso cref="http://wiki.ros.org/urdf/XML/joint"/>
    public class Calibration
    {
        /// <summary>
        /// The reference position that will trigger a rising edge when the joint moves in a positive direction.
        /// </summary>
        /// <value>Optional.</value>
        public double Rising { get; }

        /// <summary>
        /// The reference position that will trigger a falling edge when the joint moves in a positive direction.
        /// </summary>
        /// <value>Optional.</value>
        public double Falling { get; }


        /// <summary>
        /// Creates a new instance of Calibration.
        /// </summary>
        /// <param name="rising">The reference position that triggers a rising edge</param>
        /// <param name="falling">The reference position that triggers a falling edge</param>
        public Calibration(double rising, double falling)
        {
            this.Rising = rising;
            this.Falling = falling;
        }
    }
}