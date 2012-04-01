using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.StringTemplate;

namespace SizeUp.Core.Templates
{
    public class TemplateFactory
    {
        public static Template GetTemplate(string Template)
        {
            return new Template(Template, '{', '}');
        }
    }
}
