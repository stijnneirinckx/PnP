﻿using OfficeDevPnP.PowerShell.Commands.Base;
using OfficeDevPnP.PowerShell.Commands.Base.PipeBinds;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDevPnP.PowerShell.Commands
{

    [Cmdlet(VerbsCommon.Add, "SPOFieldToContentType")]
    public class AddFieldToContentType : SPOWebCmdlet
    {
        [Parameter(Mandatory = true)]
        public FieldPipeBind Field;

        [Parameter(Mandatory = true)]
        public ContentTypePipeBind ContentType;

        [Parameter(Mandatory = false)]
        public SwitchParameter Required;

        [Parameter(Mandatory = false)]
        public SwitchParameter Hidden;

        protected override void ExecuteCmdlet()
        {
            Field field = Field.Field;
            if (field == null)
            {
                if (Field.Id != Guid.Empty)
                {
                    field = this.SelectedWeb.Fields.GetById(Field.Id);
                }
                else if (!string.IsNullOrEmpty(Field.Name))
                {
                    field = this.SelectedWeb.Fields.GetByInternalNameOrTitle(Field.Name);
                }
                ClientContext.Load(field);
                ClientContext.ExecuteQuery();
            }
            if (field != null)
            {
                if (ContentType.ContentType != null)
                {
                    this.SelectedWeb.AddFieldToContentType(ContentType.ContentType, field, Required, Hidden);
                }
                else
                {
                    ContentType ct = null;
                    if (!string.IsNullOrEmpty(ContentType.Id))
                    {
                        ct = this.SelectedWeb.GetContentTypeById(ContentType.Id);
                      
                    }
                    else
                    {
                        ct = this.SelectedWeb.GetContentTypeByName(ContentType.Name);
                    }
                    if (ct != null)
                    {
                        this.SelectedWeb.AddFieldToContentType(ct, field, Required, false);
                    }
                }
            }
            else
            {
                throw new Exception("Field not found");
            }
        }


    }
}
