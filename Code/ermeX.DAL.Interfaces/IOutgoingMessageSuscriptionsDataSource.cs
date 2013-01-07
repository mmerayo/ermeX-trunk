// /*---------------------------------------------------------------------------------------*/
// If you viewing this code.....
// The current code is under construction.
// The reason you see this text is that lot of refactors/improvements have been identified and they will be implemented over the next iterations versions. 
// This is not a final product yet.
// /*---------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using ermeX.Entities.Entities;

namespace ermeX.DAL.Interfaces
{
    internal interface IOutgoingMessageSuscriptionsDataSource : IDataSource<OutgoingMessageSuscription>
    {
        IList<OutgoingMessageSuscription> GetByMessageType(string bizMessageType);

        /// <summary>
        /// Saves an outgoing subscription from an incomming suscription in other component
        /// </summary>
        /// <param name="request"></param>
        void SaveFromOtherComponent(IncomingMessageSuscription request);
    }
}