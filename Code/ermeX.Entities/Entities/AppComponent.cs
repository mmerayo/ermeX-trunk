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
    internal class AppComponent : ModelBase, IEquatable<AppComponent>
    {
        internal const int DefaultLatencyMilliseconds = 20000;

        public AppComponent() : this(DefaultLatencyMilliseconds)
        {
        }

        public AppComponent(int latencyMilliseconds = DefaultLatencyMilliseconds)
        {
            Latency = latencyMilliseconds;
        }


        public virtual Guid ComponentId { get; set; }

        public static string TableName
        {
            get { return "Components"; }
        }

        public virtual int Latency { get; set; }
        public virtual bool IsRunning { get; set; }

        public virtual bool ExchangedDefinitions { get; set; }
        /// <summary>
        /// Only one component exchanges the definitions, this is done by the one holded here
        /// </summary>
        public virtual Guid? ComponentExchanges { get; set; }

        #region Equatable

        public virtual bool Equals(AppComponent other)
        {
            if (other == null)
                return false;

            return ComponentId == other.ComponentId 
                && Latency == other.Latency 
                && ComponentOwner == other.ComponentOwner 
                && Version == other.Version 
                && IsRunning == other.IsRunning 
                && ExchangedDefinitions == other.ExchangedDefinitions;
        }

        public static bool operator ==(AppComponent a, AppComponent b)
        {
            if ((object) a == null || ((object) b) == null)
                return Equals(a, b);

            return a.Equals(b);
        }

        public static bool operator !=(AppComponent a, AppComponent b)
        {
            if (a == null || b == null)
                return !Equals(a, b);

            return !(a.Equals(b));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (AppComponent)) return false;
            return Equals((AppComponent) obj);
        }

        public override int GetHashCode()
        {
            return ComponentId.GetHashCode();
        }

        #endregion

        public static AppComponent FromDataRow(DataRow dataRow)
        {
            var result = new AppComponent
                             {
                                 Id =  Convert.ToInt32(dataRow[GetDbFieldName("Id")]),
                                 ComponentOwner = (Guid) dataRow[GetDbFieldName("ComponentOwner")],
                                 Version = (long) dataRow[GetDbFieldName("Version")],
                                 ComponentId = (Guid) dataRow[GetDbFieldName("ComponentId")],
                                 Latency = Convert.ToInt32( dataRow[GetDbFieldName("Latency")]),
                                 IsRunning = (bool)dataRow[GetDbFieldName("IsRunning")],
                                 ExchangedDefinitions = (bool)dataRow[GetDbFieldName("ExchangedDefinitions")]
                             };
            return result;
        }

        protected internal static string GetDbFieldName(string fieldName)
        {
            return String.Format("{0}_{1}", TableName, fieldName);
        }


        public static AppComponent NewFromExisting(AppComponent component)
        {
            var result = new AppComponent(component.Latency)
                             {
                                 ComponentId = component.ComponentId,
                                 ComponentOwner = component.ComponentOwner,
                                 Version = component.Version,
                                 IsRunning = component.IsRunning,
                                 Latency = component.Latency,
                                 ExchangedDefinitions=component.ExchangedDefinitions
                             };
            return result;
        }
    }
}