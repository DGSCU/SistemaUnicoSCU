using System;
using System.Collections.Generic;
using System.Text;

namespace Logger.Data
{
	public class Entity
	{
		public Entity(){

		}

		public Entity(int id, string name){
			Id = id;
			Name = name;
		}
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
