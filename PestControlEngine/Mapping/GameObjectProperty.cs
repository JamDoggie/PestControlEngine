using PestControlEngine.Mapping.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PestControlEngine.Mapping
{
    public class GameObjectProperty
    {
        // Name that shows up in the map editor. The actual internal variable name is stored as the key in the parent dictionary.
        public string RealName { get; set; }

        /* 
         * Since this value can be any type, it is stored as a string that can be parsed.
         * It is mainly stored this way instead of directly as a generic type for easy serialization.
        */

        public string DefaultValue { get; set; }

        public PropertyType propertyType { get; set; }

        public GameObjectProperty(string realName, PropertyType type)
        {
            RealName = realName;

            switch(type)
            {
                case PropertyType.INT:
                    SetValue(0);
                    break;
                case PropertyType.FLOAT:
                    SetValue(0.0f);
                    break;
                case PropertyType.DOUBLE:
                    SetValue(0.0d);
                    break;
                case PropertyType.STRING:
                    SetValue("");
                    break;
                case PropertyType.BOOL:
                    SetValue(false);
                    break;
            }
        }

        /// <summary>
        /// Attempts to read and return the current value as an int32.
        /// </summary>
        /// <returns>(Nullable)Integer</returns>
        public int? GetAsInt32()
        {
            int.TryParse(DefaultValue, out int output);

            return output;
        }

        /// <summary>
        /// Attempts to read and return the current value as a float.
        /// </summary>
        /// <returns>(Nullable)Float</returns>
        public float? GetAsFloat()
        {

            float.TryParse(DefaultValue, out float output);

            return output;
        }

        /// <summary>
        /// Attempts to read and return the current value as a double. 
        /// </summary>
        /// <returns>(Nullable)Double</returns>
        public double? GetAsDouble()
        {
            double.TryParse(DefaultValue, out double output);
            return output;
        }

        /// <summary>
        /// Attempts to read and return the current value as a string. 
        /// </summary>
        /// <returns>(Nullable)String</returns>
        public string GetAsString()
        {
            return DefaultValue;
        }

        /// <summary>
        /// Attempts to read and return the current value as a boolean. 
        /// </summary>
        /// <returns>(Nullable)Boolean</returns>
        public bool? GetAsBool()
        {
            bool.TryParse(DefaultValue, out bool output);
            return output;
        }

        public void SetValue(int value)
        {
            DefaultValue = value.ToString();
            propertyType = PropertyType.INT;
        }

        public void SetValue(float value)
        {
            DefaultValue = value.ToString();
            propertyType = PropertyType.FLOAT;
        }

        public void SetValue(double value)
        {
            DefaultValue = value.ToString();
            propertyType = PropertyType.DOUBLE;
        }

        public void SetValue(string value)
        {
            DefaultValue = value;
            propertyType = PropertyType.STRING;
        }

        public void SetValue(bool value)
        {
            DefaultValue = value.ToString();
            propertyType = PropertyType.BOOL;
        }
    }
}
