using System;
using System.ComponentModel.DataAnnotations;

namespace user_service.Models
{
    public class Utils
    {
        public static void CopyProperties(dynamic fromObject, dynamic toObject, bool isCopyNullProperties = false)
        {
            var fromObjectProperties = fromObject.GetType().GetProperties();
            var childObjectProperties = toObject.GetType().GetProperties();

            foreach (var fromObjectProperty in fromObjectProperties)
            {
                foreach (var toObjectProperty in childObjectProperties)
                {
                    if (fromObjectProperty.Name == toObjectProperty.Name
                    && fromObjectProperty.PropertyType == toObjectProperty.PropertyType
                    && toObjectProperty.GetSetMethod() != null)
                    {
                        var fromObjectPropertyValue = fromObjectProperty.GetValue(fromObject);
                        if (isCopyNullProperties)
                        {
                            toObjectProperty.SetValue(toObject, fromObjectPropertyValue);
                        }
                        else
                        {
                            if (fromObjectPropertyValue != null)
                                toObjectProperty.SetValue(toObject, fromObjectPropertyValue);
                        }

                        break;
                    }
                }
            }
        }

        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}
