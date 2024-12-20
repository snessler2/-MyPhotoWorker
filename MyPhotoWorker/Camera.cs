using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoWorker
{
    class Camera
    {
        //_s110,Canon S110, Steffens Fotoapparat
        //Canon S110
        private string _Name = "";
        //_s110
        private string _ShortName = "_";
        private string _AddName = "";
        //Steffens Fotoapparat
        private string _Description = "";

        public string Name { get { return _Name; } set { _Name = value; } }
        public string ShortName { get { return _ShortName; } set { _ShortName = value; } }
        public string AddName { get { return _AddName; } set { _AddName = value; } }
        public string Description { get { return _Description; } set { _Description = value; } }

        public Camera(string iname, string ishort, string iaddname, string idesc)
        {
            if (iname !=null) Name = iname;
            if (ishort != null) ShortName = ishort;
            if (iaddname != null) AddName = iaddname;
            if (idesc != null) Description = idesc;
        }
        public override string ToString()
        {
            return ShortName+ " " +Name;
        }
        public string GetCSVString()
        {
            return ShortName + ";" + Name + ";" + Description + ";" + AddName;
        }
        
    }
}
