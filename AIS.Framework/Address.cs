using System;
namespace AIS.Framework
{
	/// <summary>
	/// Represents an agent address using an application-wide unique identifier.
	/// </summary>
	[Serializable]
	public class Address
	{
		#region Fields

		private Uid uid;
		public static readonly Address Unknown = new Address(Uid.Empty);

		#endregion

		#region Methods

		public Address()
		{
			uid = Uid.NewUid();
		}

		public Address(Uid uid)
		{
			this.uid = uid;
		}

		public static bool operator ==(Address a, Address b)
		{
			if (object.Equals(b, null))
			{
				return false;
			}
			return a.uid == b.uid;
		}

		public static bool operator !=(Address a, Address b)
		{
			if (object.Equals(b, null))
			{
				return true;
			}
			return a.uid != b.uid;
		}

		public override int GetHashCode()
		{
			return uid.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override string ToString()
		{
			return uid.ToString();
		}

		#endregion
	}

	public enum NotifyReason { Birth, Death }

	public class AddressNotification
	{
		public Address Address { get; set; }
		public TissueType TissueType { get; set; }
		public NotifyReason Reason { get; set; }

		public AddressNotification(Address address, TissueType tissueType, NotifyReason reason)
		{
			Address = address;
			TissueType = tissueType;
			Reason = reason;
		}
	}
}
