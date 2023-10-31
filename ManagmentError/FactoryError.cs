using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentError
{
    public class FactoryError
    {
        private string _ErrorTitle;
        private string Filepath = "C:\\";
        private string _ErrorMessage;
        private DateTime _Datetime = DateTime.Now;


        public FactoryError(string ErrorTitle , string ErrorMessage) 
        {
          _ErrorMessage = ErrorMessage;
          _ErrorTitle = ErrorTitle;
        }

        public void Errorfile()
        {
            try
            {
                StreamWriter Write = File.CreateText(Filepath);
                Write.WriteLine("------------------------"+_Datetime.ToString()+"----------------");
                Write.WriteLine("------------------------" + _ErrorTitle + "---------------------");
                Write.WriteLine("------------------------" +_ErrorMessage+ "---------------------");
                Write.WriteLine("----------------------------------------------------------------");
            }
            catch(Exception ex)
            {
                StreamWriter Write = File.CreateText(Filepath);
                Write.WriteLine("------------------------" + _Datetime.ToString() + "----------------");
                Write.WriteLine("------------------------" + ex.Message + "---------------------");
                Write.WriteLine("------------------------" + ex.InnerException + "---------------------");
                Write.WriteLine("----------------------------------------------------------------");
            }
        }

    }
}
