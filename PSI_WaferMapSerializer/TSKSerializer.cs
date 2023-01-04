using PSI_WaferMapSerializer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSI_WaferMapSerializer
{
    public class TSKSerializer
    {
        public TSKModel Deserialize(string filePath)
        {
            TSKModel mapModel;

            try
            {
                mapModel = new TSKModel();

                var byteArray = GetByteArray(filePath);
                var fieldPointer = GetFieldPointer();

                mapModel.GetType().GetProperties().ToList().ForEach(property =>
                {
                    if (property.Name == "DieResults")
                    {
                        var dieCount = mapModel.MapDataAreaRowSize * mapModel.MapDataAreaLineSize;
                        mapModel.DieResults = GetDieResults(byteArray, fieldPointer["DieResults"], dieCount);
                    }
                    else if (property.PropertyType == typeof(string))
                    {
                        property.SetValue(mapModel, GetValueString(byteArray, fieldPointer[property.Name]));
                    }
                    else
                    {
                        property.SetValue(mapModel, GetValueInt(byteArray, fieldPointer[property.Name]));
                    }
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine($"TSK deserialization fails on : {ex.Message}\n{ex.StackTrace}");
                throw;
            }

            return mapModel;
        }

        public byte[] GetByteArray(string filePath)
        {
            using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                int arraylength = Convert.ToInt32(fileStream.Length);
                var result = new byte[arraylength];
                using (var binaryReader = new BinaryReader(fileStream))
                {
                    return binaryReader.ReadBytes(arraylength);
                }
            }
        }
        public Dictionary<string, (int, int)> GetFieldPointer()
        {
            /*
             * Each element in the dictionary follows the following format:
             * {[DataName], ([sequence start position], [size])}
             */
            return new Dictionary<string, (int, int)>
            {
                { "OperatorName",  (1, 20) },
                { "DeviceName", (21, 16) },
                { "WaferSize", (37, 2) },
                { "MachineNo", (39, 2) },
                { "IndexSizeX", (41, 4) },
                { "IndexSizeY", (45, 4) },
                { "StandardOrientationFlatDirection", (49, 2) },
                { "FinalEditingMachineType", (51, 1) },
                { "MapVersion", (52, 1) },
                { "MapDataAreaRowSize", (53, 2) },
                { "MapDataAreaLineSize", (55, 2) },
                { "GroupManagement", (57, 4) },
                { "WaferID", (61, 21) },
                { "NumberOfProbing", (82, 1) },
                { "LotNo", (83, 18) },
                { "CassetteNo", (101, 2) },
                { "SlotNo", (103, 2) },
                { "XCoordinatesIncreaseDirection", (105, 1) },
                { "YCoordinatesIncreaseDirection", (106, 1) },
                { "ReferenceDieSettingProcedures", (107, 1) },
                { "WaferCoordinateSystemDataReserved1", (108, 1) },
                { "TargetDiePositionX", (109, 4) },
                { "TargetDiePositionY", (113, 4) },
                { "ReferenceDieCoordinatorX", (117, 2) },
                { "ReferenceDieCoordinatorY", (119, 2) },
                { "ProbingStartPosition", (121, 1) },
                { "ProbingDirection", (122, 1) },
                { "WaferCoordinateSystemDataReserved2", (123, 2) },
                { "DistanceXToWaferCenterDieOrigin", (125, 4) },
                { "DistanceYToWaferCenterDieOrigin", (129, 4) },
                { "CoordinatorXOfWaferCenterDie", (133, 4) },
                { "CoordinatorYOfWaferCenterDie", (137, 4) },
                { "FirstDieCoordinatorX", (141, 4) },
                { "FirstDieCoordinatorY", (145, 4) },
                { "TestingStartYear", (149, 2) },
                { "TestingStartMonth", (151, 2) },
                { "TestingStartDay", (153, 2) },
                { "TestingStartHour", (155, 2) },
                { "TestingStartMinute", (157, 2) },
                { "TestingStartReserved", (159, 2) },
                { "TestingEndYear", (161, 2) },
                { "TestingEndMonth", (163, 2) },
                { "TestingEndDay", (165, 2) },
                { "TestingEndHour", (167, 2) },
                { "TestingEndMinute", (169, 2) },
                { "TestingEndReserved", (171, 2) },
                { "LoadEndYear", (173, 2) },
                { "LoadEndMonth", (175, 2) },
                { "LoadEndDay", (177, 2) },
                { "LoadEndHour", (179, 2) },
                { "LoadEndMinute", (181, 2) },
                { "LoadReserved", (183, 2) },
                { "UnloadStartYear", (185, 2) },
                { "UnloadStartMonth", (187, 2) },
                { "UnloadStartDay", (189, 2) },
                { "UnloadStartHour", (191, 2) },
                { "UnloadStartMinute", (193, 2) },
                { "UnloadReserved", (195, 2) },
                { "MachineNo1", (197, 4) },
                { "MachineNo2", (201, 4) },
                { "SpecialCharacters", (205, 4) },
                { "TestingEndInformation", (209, 1) },
                { "TestingResultReserved", (210, 1) },
                { "TotalTestedDice", (211, 2) },
                { "TotalPassDice", (213, 2) },
                { "TotalFailDice", (215, 2) },
                { "TestDieInformationAddress", (217, 4) },
                { "NumberOfLineCategoryData", (221, 4) },
                { "LineCategoryAddress", (225, 4) },
                { "MapFileConfiguration", (229, 2) },
                { "MaxMultiSite", (231, 2) },
                { "MaxCategories", (233, 2) },
                { "ExtendedMapInformationReserved", (235, 2) },
                { "DieResults", (237, 6)}
            };
        }
        public string GetValueString(byte[] byteArray, (int, int) pointer)
        {
            var sequenceStart = pointer.Item1;
            var size = pointer.Item2;

            var skipCount = sequenceStart - 1;
            return Encoding.Default.GetString(byteArray.Skip(skipCount).Take(size).ToArray()).Trim();
        }
        public int GetValueInt(byte[] byteArray, (int, int) pointer)
        {
            var sequenceStart = pointer.Item1;
            var size = pointer.Item2;

            var skipCount = sequenceStart - 1;
            var input = byteArray.Skip(skipCount).Take(size).ToArray();

            var result = 0;

            for (int i  = 0; i < input.Length; i++)
            {
                result += (int)(input[i] * Math.Pow(256, input.Length - i - 1));
            }

            return result;
        }
        public List<TSKDieResult> GetDieResults(byte[] byteArray, (int, int) pointer, int dieCount)
        {
            var dieResults = new List<TSKDieResult>();

            var sequenceStart = pointer.Item1;
            var size = pointer.Item2;

            var resultPattern =
                    "(?<DieTestResult>[01]{2})" +
                    "(?<Marking>[01]{1})" +
                    "(?<FailMarkInspection>[01]{1})" +
                    "(?<ReProbingResult>[01]{2})" +
                    "(?<NeedleMarkInspectionResult>[01]{1})" +
                    "(?<DieCoordinatorValueX>[01]{9})" +
                    "(?<DieProperty>[01]{2})" +
                    "(?<ExecutionDieSelection>[01]{1})" +
                    "(?<SampleDie>[01]{1})" +
                    "(?<CodeBitCoordX>[01]{1})" +
                    "(?<CodeBitCoordY>[01]{1})" +
                    "(?<DummyData>[01]{1})" +
                    "(?<DieCoordinatorValueY>[01]{9})" +
                    "(?<MeasurementFinishFlag>[01]{1})" +
                    "(?<RejectChipFlag>[01]{1})" +
                    "(?<TestExecutionSite>[01]{6})" +
                    "(?<BlockAreaJudgementFunction>[01]{2})" +
                    "(?<CategoryData>[01]{6})";

            var resultRegex = new Regex(resultPattern);

            for (int i = 0; i < dieCount; i++)
            {
                var die = new TSKDieResult();
                var skipCount = sequenceStart + (i * size) - 1;
                var input = byteArray.Skip(skipCount).Take(size).ToArray();

                var resultString = "";
               for (int j = 0; j < input.Length; j++)
                {
                    resultString += Convert.ToString(input[j], 2).PadLeft(8, '0');
                }

                var resultMatch = resultRegex.Match(resultString);
                var dieResult = new TSKDieResult()
                {
                    TestResult = Convert.ToInt32(resultMatch.Groups["DieTestResult"].Value, 2),
                    Marking = Convert.ToInt32(resultMatch.Groups["Marking"].Value, 2),
                    FailMarkInspection = Convert.ToInt32(resultMatch.Groups["FailMarkInspection"].Value, 2),
                    ReProbingResult = Convert.ToInt32(resultMatch.Groups["ReProbingResult"].Value, 2),
                    NeedleMarkInspectionResult = Convert.ToInt32(resultMatch.Groups["NeedleMarkInspectionResult"].Value, 2),
                    DieCoordinatorValueX = Convert.ToInt32(resultMatch.Groups["DieCoordinatorValueX"].Value, 2),
                    DieProperty = Convert.ToInt32(resultMatch.Groups["DieProperty"].Value, 2),
                    NeedleMarkingInspectionExecutionDieSelection = Convert.ToInt32(resultMatch.Groups["ExecutionDieSelection"].Value, 2),
                    SamplingDie = Convert.ToInt32(resultMatch.Groups["SampleDie"].Value, 2),
                    CodeBitOfCoordinatorValueX = Convert.ToInt32(resultMatch.Groups["CodeBitCoordX"].Value, 2),
                    CodeBitOfCoordinatorValueY = Convert.ToInt32(resultMatch.Groups["CodeBitCoordY"].Value, 2),
                    DummyData = Convert.ToInt32(resultMatch.Groups["DummyData"].Value, 2),
                    DieCoordinatorValueY = Convert.ToInt32(resultMatch.Groups["DieCoordinatorValueY"].Value, 2),
                    MeasurementFinishFlag = Convert.ToInt32(resultMatch.Groups["MeasurementFinishFlag"].Value, 2),
                    RejectChipFlag = Convert.ToInt32(resultMatch.Groups["RejectChipFlag"].Value, 2),
                    TestExecutionSiteNo = Convert.ToInt32(resultMatch.Groups["TestExecutionSite"].Value, 2),
                    BlockAreaJudgementFunction = Convert.ToInt32(resultMatch.Groups["BlockAreaJudgementFunction"].Value, 2),
                    CategoryData = Convert.ToInt32(resultMatch.Groups["CategoryData"].Value, 2)
                };

                dieResults.Add(dieResult);
            }

            return dieResults;
        }

    }
}
