using BAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class baseDL
    {
        public List<Vocabulary> GetListVocab()
        {
            using (ConnectClass dataAccess = new ConnectClass())
            {
                return dataAccess.GetListWord("Proc_allWord");
            }
        }

        public Vocabulary Search(string word)
        {
            using (ConnectClass connect=new ConnectClass())
            {
                Vocabulary lst = new Vocabulary();
                lst=connect.Search("timtu", word);
                return lst;
            }
        }
    }
}
