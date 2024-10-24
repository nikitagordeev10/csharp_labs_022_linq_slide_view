using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace linq_slideviews {
    public class ParsingTask {
        public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines) {
            var slideTypeMapping = new Dictionary<string, SlideType>()
            {
                {"theory", SlideType.Theory},
                {"quiz", SlideType.Quiz},
                {"exercise", SlideType.Exercise}
            };

            return lines.Skip(1)
                .Select(line => {
                    var lineParams = line.Split(new[] { ';' }, 3, StringSplitOptions.RemoveEmptyEntries);
                    if (lineParams.Length < 3 || !int.TryParse(lineParams[0], out int slideID))
                        return null;

                    if (!slideTypeMapping.TryGetValue(lineParams[1], out SlideType slideType))
                        return null;

                    return new SlideRecord(slideID, slideType, lineParams[2]);
                })
                .Where(slideRecord => slideRecord != null)
                .ToDictionary(slideRecord => slideRecord.SlideId);
        }

        public static IEnumerable<VisitRecord> ParseVisitRecords(IEnumerable<string> lines, IDictionary<int, SlideRecord> slides) {
            return lines.Skip(1)
                .Select(line => {
                    var lineParams = line.Split(new[] { ';' }, 3, StringSplitOptions.RemoveEmptyEntries);
                    if (lineParams.Length < 3
                        || !int.TryParse(lineParams[0], out int userID)
                        || !int.TryParse(lineParams[1], out int slideID)
                        || !DateTime.TryParseExact(lineParams[2], "yyyy-MM-dd;HH:mm:ss",
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime)) {
                        throw new FormatException($"Wrong line [{line}]");
                    }

                    if (!slides.TryGetValue(slideID, out SlideRecord slideRecord)) {
                        throw new FormatException($"Slide with ID {slideID} not found in line [{line}]");
                    }

                    return new VisitRecord(userID, slideID, dateTime, slideRecord.SlideType);
                });
        }
    }
}
