//using System;
//using System.Web.Script.Serialization;
//using Microsoft.Practices.Unity;

//namespace Pilgrims.ProjectManagement.Contracts.Classes
//{
//    public static class JsSerializer
//    {
//        public static string Serialize(object myobj)
//        {

//            if (myobj == null)
//                return null;

//            var js = new JavaScriptSerializer();
//            var returnstring = js.Serialize(myobj);
//            return returnstring;
//        }



//        public static T Deserialize<T>(string obj)
//        {

//            if (string.IsNullOrWhiteSpace(obj))
//                return default(T);

//            if (typeof(T).IsInterface)
//            {
//                var type = GlobalDeclarations.DiContainer.Resolve<T>().GetType();
//                var xxx = Deserialize(obj, type);
//                return (T) xxx;
//            }

//            var js = new JavaScriptSerializer();
//            var returnstring = js.Deserialize<T>(obj);
//            return returnstring;
//        }


//        public static object Deserialize(string obj, Type type)
//        {
//            if (string.IsNullOrWhiteSpace(obj))
//                return null;

//            var js = new JavaScriptSerializer();
//            var returnstring = js.Deserialize(obj, type);
//            return returnstring;
//        }
//    }
//}
