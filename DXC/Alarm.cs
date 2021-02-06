/*
 * Created by SharpDevelop.
 * User: Ситал
 * Date: 20.02.2017
 * Time: 16:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace DXC
{
	/// <summary>
	/// Description of Struct1.
	/// </summary>
	public struct Alarm : IEquatable<Alarm>
	{
		string name;
		DateTime Start;
		DateTime End;
		bool active;
		
		#region Equals and GetHashCode implementation
		// The code in this region is useful if you want to use this structure in collections.
		// If you don't need it, you can just remove the region and the ": IEquatable<Struct1>" declaration.
		
		public override bool Equals(object obj)
		{
			if (obj is Alarm)
				return Equals((Alarm)obj); // use Equals method below
			else
				return false;
		}
		
		public bool Equals(Alarm other)
		{
			// add comparisions for all members here
			return (this.Start == other.Start && this.name==other.name);
		}

		
		public static bool operator ==(Alarm left, Alarm right)
		{
			return left.Equals(right);
		}
		
		public static bool operator !=(Alarm left, Alarm right)
		{
			return !left.Equals(right);
		}
		#endregion
	}
}
