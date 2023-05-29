using DanSaul.SharedCode.Extensions;
using System.Text;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL
{
	public class ContextBlock : Block
	{
        
        public string ContextName { get; set; }
		public List<ContextBlock> Includes { get; } = new();
		public List<ExtensionBlock> Extensions { get; } = new();
		public bool EnsureHangupExtension { get; private set; } = true;
		public bool EnsureFailedExtension { get; private set; } = true;
		public ContextBlock(string _ContextName)
		{
			ContextName = _ContextName;
		}

		public override string Generate(int indentLevel)
		{
			StringBuilder sb = new();
			sb.AppendLine($"{Tabs(indentLevel)}context {ContextName} {{");

			indentLevel++;

			if (Includes.Any())
			{
				sb.AppendLine($"{Tabs(indentLevel)} includes {{");

				indentLevel++;
				foreach (ContextBlock include in Includes)
				{
					sb.AppendLine($"{Tabs(indentLevel)}{include.ContextName};");
				}
				indentLevel--;

				sb.AppendLine($"{Tabs(indentLevel)}}}");
			}

			bool hasHangupExtension = false;
			bool hasFailedExtension = false;
			if (Extensions.Any())
			{
				foreach (ExtensionBlock extension in Extensions)
				{
					if (extension.Extension == "h")
						hasHangupExtension = true;
					if (extension.Extension == "failed")
						hasFailedExtension = true;
					sb.AppendLine(extension.Generate(indentLevel));
				}
			}

			if (EnsureHangupExtension && !hasHangupExtension)
			{
				sb.AppendLine($"{Tabs(indentLevel)}h => Hangup();");
			}

			if (EnsureFailedExtension && !hasFailedExtension)
			{
				sb.AppendLine($"{Tabs(indentLevel)}failed => Hangup();");
			}

			indentLevel--;

			
			sb.AppendLine($"{Tabs(indentLevel)}}}");
			return sb.ToString();
		}
	}
}
