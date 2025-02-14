﻿using Must.Models;

namespace Must;

public static class SensorToModelMapper
{
    public static void Map<T>(ushort startAddress, ushort[] values, T model)
    {
        //Generate a lookup from sensor id to class property.
        var dictionary = ModbusSensorHelper.GetModbusSensorPropertyInfos<T>();

        var index = startAddress;
        foreach (var value in values)
        {
            if (!dictionary.ContainsKey(index))
            {
                index++;
                continue;
            }

            //Get the property we are interested in
            var property = dictionary[index];

            //Get the sensor attributes
            var attribute = property.GetCustomAttributes(true).First(y => y.GetType() == typeof(ModbusSensorAttribute)) as ModbusSensorAttribute;

            if (property.PropertyType == typeof(double?))
            {
                // truncate
                property.SetValue(model, Math.Truncate((value * (attribute?.Coefficient ?? 0))*100)/100 );
                //property.SetValue(model, value * attribute?.Coefficient);
            }

            if (property.PropertyType == typeof(short?))
            {
                property.SetValue(model, (short)(Math.Truncate((value * (attribute?.Coefficient ?? 0))*100)/100));
            }

            index++;
        }

    }

}
