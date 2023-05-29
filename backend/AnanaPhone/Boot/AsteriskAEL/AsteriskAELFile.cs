using DanSaul.SharedCode.Extensions;
using System.Text;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL
{
	public class AsteriskAELFile
	{
		public List<ContextBlock> Contexts { get; } = new();
		public GlobalsBlock? Globals { get; init; } = null;
		public string? PreambleComment { get; init; } = null;
		public bool ShowGeneratedWarning { get; init; } = true;

		const string kGeneratedWarning = "This file has been automatically generated, your changes may end up being erased.";
		const int kCommentMaxLineLength = 80;
		const string kCommentLinePrefix = "// ";

		public string Generate()
		{
			int indentLevel = 0;


			StringBuilder sb = new();

			if (ShowGeneratedWarning)
				sb.AppendLine(kGeneratedWarning.MaxLineLengthAddPrefix(kCommentMaxLineLength, kCommentLinePrefix));
			if (!string.IsNullOrEmpty(PreambleComment))
				sb.AppendLine(PreambleComment.MaxLineLengthAddPrefix(kCommentMaxLineLength, kCommentLinePrefix));

			if (Globals != null)
				sb.AppendLine(Globals.Generate(indentLevel));

			foreach (ContextBlock context in Contexts)
				sb.AppendLine(context.Generate(indentLevel));
			



			return sb.ToString();
		}
	}
}
