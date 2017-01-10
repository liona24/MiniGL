using System.Xml.Serialization;
using System.IO;

namespace MiniGL.IO
{
    public static class GImportExport
    {
        public static void Export(string file, GObject[] objs)
        {
            using (var write = new StreamWriter(file))
            {
                var s = new XmlSerializer(objs.GetType());
                s.Serialize(write, objs);
            }
        }
        public static void Import(string file, out GObject[] objs)
        {
            using (var read = new StreamReader(file))
            {
                var type = (new GObject[1]).GetType(); //if there is a better way...
                var s = new XmlSerializer(type);
                objs = (GObject[])s.Deserialize(read);
            }
        }
        public static void Export(string file, GObject2D[] objs)
        {
            using (var write = new StreamWriter(file))
            {
                var s = new XmlSerializer(objs.GetType());
                s.Serialize(write, objs);
            }
        }
        public static void Import(string file, out GObject2D[] objs)
        {
            using (var read = new StreamReader(file))
            {
                var type = (new GObject2D[1]).GetType(); //if there is a better way...
                var s = new XmlSerializer(type);
                objs = (GObject2D[])s.Deserialize(read);
            }
        }
       
    }
}
