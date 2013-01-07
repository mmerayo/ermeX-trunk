// /*---------------------------------------------------------------------------------------*/
// If you viewing this code.....
// The current code is under construction.
// The reason you see this text is that lot of refactors/improvements have been identified and they will be implemented over the next iterations versions. 
// This is not a final product yet.
// /*---------------------------------------------------------------------------------------*/
using System;
using System.Data;
using ermeX.Entities.Base;

namespace ermeX.Entities.Entities
{
    internal class OutgoingMessageSuscription : ModelBase, IEquatable<OutgoingMessageSuscription>
    {
        internal const string TableName = "OutgoingMessageSuscriptions";

        public OutgoingMessageSuscription(IncomingMessageSuscription suscription, Guid suscriberComponentId,
                                          Guid localComponentId)
        {
            Id = 0;
            Component = suscriberComponentId;
            ComponentOwner = localComponentId;
            BizMessageFullTypeName = suscription.BizMessageFullTypeName;
            DateLastUpdateUtc = suscription.DateLastUpdateUtc;
            Version = suscription.Version;
        }

        public OutgoingMessageSuscription()
        {
        }

        public virtual string BizMessageFullTypeName { get; set; }

        public virtual Guid Component { get; set; }

        public virtual DateTime DateLastUpdateUtc { get; set; }

        internal static string GetDbFieldName(string fieldName)
        {
            return string.Format("{0}_{1}", TableName, fieldName);
        }

        public static OutgoingMessageSuscription FromDataRow(DataRow dataRow)
        {
            var result = new OutgoingMessageSuscription
                             {
                                 Id = Convert.ToInt32( dataRow[GetDbFieldName("Id")]),
                                 ComponentOwner = new Guid(dataRow[GetDbFieldName("ComponentOwner")].ToString()),
                                 Version = (long) dataRow[GetDbFieldName("Version")],
                                 Component = new Guid(dataRow[GetDbFieldName("ComponentId")].ToString()),
                                 BizMessageFullTypeName = dataRow[GetDbFieldName("BizMessageFullTypeName")].ToString(),
                                 DateLastUpdateUtc = new DateTime((long) dataRow[GetDbFieldName("DateLastUpdateUtc")]),
                             };
            return result;
        }

        #region Equatable

        public virtual bool Equals(OutgoingMessageSuscription other)
        {
            if (other == null)
                return false;

            var result = Component.ToString() == other.Component.ToString() &&
                         BizMessageFullTypeName == other.BizMessageFullTypeName && Version == other.Version;
#if !NEED_FIX_MILLISECONDS
            result = result && DateLastUpdateUtc == other.DateLastUpdateUtc;
#endif
            return result;
        }

        public static bool operator ==(OutgoingMessageSuscription a, OutgoingMessageSuscription b)
        {
            if ((object) a == null || ((object) b) == null)
                return Equals(a, b);

            return a.Equals(b);
        }

        public static bool operator !=(OutgoingMessageSuscription a, OutgoingMessageSuscription b)
        {
            if (a == null || b == null)
                return !Equals(a, b);

            return !(a.Equals(b));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (OutgoingMessageSuscription)) return false;
            return Equals((OutgoingMessageSuscription) obj);
        }

        public override int GetHashCode()
        {
            return Component.GetHashCode();
        }

        #endregion
    }
}