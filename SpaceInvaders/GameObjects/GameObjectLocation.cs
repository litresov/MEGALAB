using System;
using System.Runtime.InteropServices;

namespace SpaceInvaders
{
   
    class GameObjectLocation
    {
        public int X { get; set; }

        public int Y { get; set; }

        public static MSScriptControl.ScriptControl Script = new MSScriptControl.ScriptControl(); //Создание нового экземпляра скрипта
        static GameObjectLocation()
        {
            Script.Language = "VBScript";                          // Заполняем скрипт.
            Script.AddCode("Function MSscript(X, lX, Y , lY)" +    // Добавляем Функцию
                "If (x = lX AND y = lY) Then MSscript = true " +   // Сравнивающую координаты
                "Else MSscript = false End If End Function");      // Если координаты равны, то возвращает true, иначе false
        }

        public override bool Equals(object place)
        {
            GameObjectLocation location = place as GameObjectLocation;
            bool temp = Convert.ToBoolean(Script.Run("MSscript", X, location.X, Y, location.Y));
            if (location != null && temp)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}