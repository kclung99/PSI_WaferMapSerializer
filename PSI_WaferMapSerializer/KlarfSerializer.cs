﻿using PSI_WaferMapSerializer.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSI_WaferMapSerializer
{
    public class KlarfSerializer
    {
        public KlarfModel Deserialize(string input)
        {
            var mapDictionary = new Dictionary<string, string>();
            KlarfModel mapModel;

            try
            {
                AssignMapDictionary(input, mapDictionary);

                mapModel = new KlarfModel()
                {
                    //FileVersion,
                    FileTimeStamp = GetTimeStamp(mapDictionary["FileTimeStamp"]),
                    //TiffSpec,
                    InspectionStationID = GetInspectionStationID(mapDictionary["InspectionStationID"]),
                    SampleType = mapDictionary["SampleType"],
                    SampleSize = GetSampleSize(mapDictionary["SampleSize"]),
                    //ResultsID = mapDictionary["ResultsID"],
                    ResultTimestamp = GetTimeStamp(mapDictionary["ResultTimestamp"]),
                    LotID = mapDictionary["LotID"].Trim('"'),
                    SetupID = GetSetupID(mapDictionary["SetupID"]),
                    StepID = mapDictionary["StepID"].Trim('"'),
                    DeviceID = mapDictionary["DeviceID"].Trim('"'),
                    WaferID = mapDictionary["WaferID"].Trim('"'),
                    Slot = int.Parse(mapDictionary["Slot"]),
                    SampleOrientationMarkType = mapDictionary["SampleOrientationMarkType"],
                    OrientationMarkLocation = mapDictionary["OrientationMarkLocation"],
                    InspectionOrientation = mapDictionary["InspectionOrientation"],
                    OrientationInstructions = mapDictionary["OrientationInstructions"].Trim('"'),
                    CoordinatesMirrored = mapDictionary["CoordinatesMirrored"],
                    SampleCenterLocation = GetDieCoordinate(mapDictionary["SampleCenterLocation"]),
                    //TiffFileName = mapDictionary["TiffFileName"],
                    //AlignmentPoints,
                    //AlignmentImages,
                    //AlignmentImageTransforms,
                    //DatabaseAlignmentMarks,
                    DiePitch = GetDiePitch(mapDictionary["DiePitch"]),
                    DieOrigin = GetDieCoordinate(mapDictionary["DieOrigin"]),
                    //RemovedDieList,
                    //SampleDieMap,
                    //ClassLookup,
                    //DefectClusterSpec,
                    //DefectClusterSetup,
                    DefectRecordSpec = GetSpec(mapDictionary["DefectRecordSpec"]),
                    DefectList = GetList(mapDictionary["DefectList"]),
                    SummarySpec = GetSpec(mapDictionary["SummarySpec"]),
                    SummaryList = GetList(mapDictionary["SummaryList"]),
                    ClusterClassificationList = GetClusterClassificationList(mapDictionary["ClusterClassificationList"]),
                    //WaferStatus,
                    //LotStatus,
                    EndOfFile = mapDictionary.ContainsKey("EndlOfFile")
                };

                mapModel.InspectionTests = GetInspectionTests(mapDictionary);

                var a = mapModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Klarf deserialization fails on : {ex.Message}\n{ex.StackTrace}");
                throw;
            }

            return mapModel;
        }

        public void AssignMapDictionary(string input, Dictionary<string, string> mapDictionary)
        {
            var fieldsRegex = new Regex(@"(?<TagField>[a-zA-Z]+)(\s(?<DataFields>[^;]*))?;");
            var fieldsMatches = fieldsRegex.Matches(input);

            foreach (Match fieldsMatch in fieldsMatches)
            {
                mapDictionary.Add(fieldsMatch.Groups["TagField"].Value, fieldsMatch.Groups["DataFields"].Value);
            }
        }
        public KlarfTimeStamp GetTimeStamp(string input)
        {
            var date = input.Split(" ")[0];
            var time = input.Split(" ")[1];

            return new KlarfTimeStamp()
            {
                Date = DateOnly.ParseExact(date, "MM-dd-yyyy"),
                Time = TimeOnly.ParseExact(time, "HH:mm:ss")
            };
        }
        public KlarfInspectionStationID GetInspectionStationID(string input)
        {
            var parsedInput = input.Split(" ");

            var manufacture = parsedInput[0].Trim('"');
            var model = parsedInput[1].Trim('"');
            var equipmentID = parsedInput[2].Trim('"');

            return new KlarfInspectionStationID()
            {
                EquipmentManufacturer = manufacture,
                Model = model,
                EquipmentID = equipmentID,
            };
        }
        public KlarfSampleSize GetSampleSize(string input)
        {
            var parsedInput = input.Split(" ");
            var numberOfFields = int.Parse(parsedInput[0]);

            var output = new KlarfSampleSize();

            if (numberOfFields != 3)
            {
                output.NumberOfFields = numberOfFields;
                output.Length = int.Parse(parsedInput[1]);
                output.Width = int.Parse(parsedInput[1]);
            }
            else
            {
                output.Width = int.Parse(parsedInput[2]);
            }

            return output;
        }
        public KlarfSetupID GetSetupID(string input)
        {
            var parsedInput = input.Split(" ");

            var data = parsedInput[0].Trim('"');
            var definedDate = DateOnly.ParseExact(parsedInput[1], "MM-dd-yyyy");
            var definedTime = TimeOnly.ParseExact(parsedInput[2], "HH:mm:ss");

            return new KlarfSetupID()
            {
                Data = data,
                DefinedDate = definedDate,
                DefinedTime = definedTime
            };
        }
        public KlarfDieCoordinate GetDieCoordinate(string input)
        {
            var parsedInput = input.Split(" ");

            return new KlarfDieCoordinate()
            {
                XCoordinate = decimal.Parse(parsedInput[0]),
                YCoordinate = decimal.Parse(parsedInput[1])
            };
        }
        public KlarfDiePitch GetDiePitch(string input)
        {
            var parsedInput = input.Split(" ");

            return new KlarfDiePitch()
            {
                XDiePitch = decimal.Parse(parsedInput[0]),
                YDiePitch = decimal.Parse(parsedInput[1])
            };
        }
        public List<KlarfInspectionTest> GetInspectionTests(Dictionary<string, string> mapDictionary)
        {
            var inspectionTests = new List<KlarfInspectionTest>();

            foreach(KeyValuePair<string, string> kv in mapDictionary)
            {
                if (kv.Key == "InspectionTest")
                {
                    var inpectionTest = new KlarfInspectionTest() { TestNumber = int.Parse(kv.Value) };
                    inspectionTests.Add(inpectionTest);
                }
                else if (kv.Key == "SampleTestPlan")
                {
                    inspectionTests.Last().SampleTestPlan = GetKlarfDice(kv.Value);
                }
                else if (kv.Key == "SampleTestReferencePlan")
                {
                    //inspectionTests.Last().SampleTestReferencePlan
                }
                else if (kv.Key == "InspectedAreaOrigin")
                {
                    //inspectionTests.Last().InspectedAreaOrigin
                }
                else if (kv.Key == "InspectedArea")
                {
                    //inspectionTests.Last().InspectedArea
                }
                else if (kv.Key == "AreaPerTest")
                {
                    inspectionTests.Last().AreaPerTest = decimal.Parse(kv.Value);
                }
                else if (kv.Key == "TestParametersSpec")
                {
                    //inspectionTests.Last().TestParametersSpec
                }
                else if (kv.Key == "TestParametersList")
                {
                    //inspectionTests.Last().TestParametersList
                }

            }

            return inspectionTests;
        }
        public KlarfDice GetKlarfDice(string input)
        {
            var fieldRegex = new Regex(@"(?<NumberOfGroups>^[0-9]{0,5})\r\n|(?<XIndex>-?[0-9]{1,3})\s(?<YIndex>-?[0-9]{1,3})\r\n");
            var fieldMatchs = fieldRegex.Matches(input);

            var dice = new KlarfDice();

            foreach(Match fieldMatch in fieldMatchs)
            {

                if (!string.IsNullOrEmpty(fieldMatch.Groups["NumberOfGroups"].Value))
                {
                    dice.NumberOfGroups = int.Parse(fieldMatch.Groups["NumberOfGroups"].Value);
                }
                else
                {
                    var dieIndex = new KlarfDieIndex()
                    {
                        XIndex = int.Parse(fieldMatch.Groups["XIndex"].Value),
                        YIndex = int.Parse(fieldMatch.Groups["YIndex"].Value),
                    };

                    dice.Dice.Add(dieIndex);
                }
            }

            return dice;
        }
        public KlarfSpec GetSpec(string input)
        {
            var fieldRegex = new Regex(@"(?<NumberOfFields>^[0-9]{0,2})\r\n|(?<Spec>\w+)");
            var fieldMatchs = fieldRegex.Matches(input);

            var spec = new KlarfSpec() { Specs = new List<string>() };

            foreach (Match fieldMatch in fieldMatchs)
            {
                if (!string.IsNullOrEmpty(fieldMatch.Groups["NumberOfFields"].Value))
                {
                    spec.NumberOfFields = int.Parse(fieldMatch.Groups["NumberOfFields"].Value);
                }
                else
                {
                    spec.Specs.Add(fieldMatch.Groups["Spec"].Value);
                }
            }
            
            return spec;
        }
        public List<List<decimal>>? GetList(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            var output = new List<List<decimal>>();
            var rows = input.Split("\r\n");

            foreach(string row in rows)
            {
                var list = new List<decimal>();
                row.Split(" ").ToList().ForEach(element => list.Add(decimal.Parse(element)));
                output.Add(list);
            }

            return output;
        }
        public KlarfClusterClassificationList GetClusterClassificationList(string input)
        {
            var fieldRegex = new Regex(@"(?<NumberOfGroups>^[0-9]+)\r\n?|(?<Identification>[0-9]+)\s(?<Classification>[0-9]+)");
            var fieldMatches = fieldRegex.Matches(input);

            var output = new KlarfClusterClassificationList() { ClusterClassifications = new List<KlarfClusterClassification>() };

            foreach(Match fieldMatch in fieldMatches)
            {
                if (!string.IsNullOrEmpty(fieldMatch.Groups["NumberOfGroups"].Value))
                {
                    output.NumberOfGroups = int.Parse(fieldMatch.Groups["NumberOfGroups"].Value);
                }
                else
                {
                    output.ClusterClassifications.Add(new KlarfClusterClassification
                    {
                        Identification = int.Parse(fieldMatch.Groups["Identification"].Value),
                        Classification = int.Parse(fieldMatch.Groups["Classification"].Value)
                    });
                }
            }

            return output;
        }
    }
}
