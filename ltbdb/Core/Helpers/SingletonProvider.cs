using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Reflection;

namespace Singleton
{
	public abstract class SingletonBase<T> where T: SingletonBase<T>
	{
		public static T GetInstance()
		{
			return SingletonProvider.GetInstance<T>();
		}
	}
	
	public static class SingletonProvider
	{
		private static Hashtable _selfs = new Hashtable();
		private static object _lock = new object();
		
		public static T GetInstance<T>() where T: class
		{
			ConstructorInfo checkPrivateCtor = (typeof(T)).GetConstructor(Type.EmptyTypes);
			
			if(checkPrivateCtor != null)
			{
				throw new InvalidOperationException("Singleton means that you don't have any public constructors.");
			}
			
			lock(_lock)
			{
				if(_selfs.ContainsKey(typeof(T).GUID))
				{
					return (T)_selfs[typeof(T).GUID];
				}
				
				ConstructorInfo privateCtor;
				
				privateCtor = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
				
				T instance = (T)privateCtor.Invoke(new object[] { });
				
				_selfs.Add(typeof(T).GUID, instance);
				
				return instance;
			}
		}
	}
}
