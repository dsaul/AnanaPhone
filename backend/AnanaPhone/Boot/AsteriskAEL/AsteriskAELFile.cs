using DanSaul.SharedCode.Extensions;
using System.Text;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL
{
	public record AsteriskAELFile
	{
		//public IEnumerable<Section> Sections { get; init; } = Array.Empty<Section>();
		public string? PreambleComment { get; init; } = null;
		public bool ShowGeneratedWarning { get; init; } = true;

		const string kGeneratedWarning = "This file has been automatically generated, your changes may end up being erased.";
		const int kCommentMaxLineLength = 80;
		const string kCommentLinePrefix = "// ";

		public void Generate(StringBuilder sb)
		{
			if (ShowGeneratedWarning)
				sb.AppendLine(kGeneratedWarning.MaxLineLengthAddPrefix(kCommentMaxLineLength));
			if (!string.IsNullOrEmpty(PreambleComment))
				sb.AppendLine(PreambleComment.MaxLineLengthAddPrefix(kCommentMaxLineLength));

			//foreach (Section section in Sections)
			//	section.Generate(sb);
		}
	}
}
