using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEProgPart3
{
    public class SaveData
    {
        public string userName;
        public string favouriteTopic;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string FavouriteTopic
        {
            get { return favouriteTopic; }
            set { favouriteTopic = value; }
        }
    }
}
