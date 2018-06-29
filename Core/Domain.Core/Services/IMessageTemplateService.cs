namespace Nagnoi.SiC.Domain.Core.Services {

    #region Imports
        
    using Domain.Core.Model;
    using System.Collections.Generic;

    #endregion
    public interface IMessageTemplateService {

        IEnumerable<MessageTemplate> GetAllSettings();
        MessageTemplate GetMessageTemplate(string keyword);
    }
}
