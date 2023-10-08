using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateExample
{
        public static class Calculator
        {
            // Создадим статический экземпляр DataTable, чтобы каждый раз не инициализировать его заново
            private static DataTable Table { get; } = new DataTable();

            // Наш метод подсчета
            // Добавьте отлов ошибок по вкусу)
            public static double Calc(string Expression) => Convert.ToDouble(Table.Compute(Expression, string.Empty));
        }

        public class Example
        {
            public int id { get; set; }
            public string example { set; get; }
            public int answer { set; get; }
            public int tryAnswer { set; get; }
        }

}
