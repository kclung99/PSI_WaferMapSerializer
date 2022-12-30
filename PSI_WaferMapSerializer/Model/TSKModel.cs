using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PSI_WaferMapSerializer.Model
{
    public class TSKModel
    {
        public string OperatorName { get; set; }
        public string DeviceName { get; set; }
        public int WaferSize { get; set; } 
        public int MachineNo { get; set; }
        public int IndexSizeX { get; set; } 
        public int IndexSizeY { get; set; } 
        public int StandardOrientationFlatDirection { get; set; } 
        public int FinalEditingMachineType { get; set; }
        public int MapVersion { get; set; } 
        public int MapDataAreaRowSize { get; set; }
        public int MapDataAreaLineSize { get; set; }
        public int GroupManagement { get; set; } 
        public string WaferID { get; set; }
        public int NumberOfProbing { get; set; }
        public string LotNo { get; set; }
        public int CassetteNo { get; set; }
        public int SlotNo { get; set; }
        public int XCoordinatesIncreaseDirection { get; set; } 
        public int YCoordinatesIncreaseDirection { get; set; } 
        public int ReferenceDieSettingProcedures { get; set; } 
        public int WaferCoordinateSystemDataReserved1 { get; set; }
        public int TargetDiePositionX { get; set; } 
        public int TargetDiePositionY { get; set; } 
        public int ReferenceDieCoordinatorX { get; set; } 
        public int ReferenceDieCoordinatorY { get; set; } 
        public int ProbingStartPosition { get; set; } 
        public int ProbingDirection { get; set; } 
        public int WaferCoordinateSystemDataReserved2 { get; set; }
        public int DistanceXToWaferCenterDieOrigin { get; set; } //
        public int DistanceYToWaferCenterDieOrigin { get; set; } //
        public int CoordinatorXOfWaferCenterDie { get; set; } 
        public int CoordinatorYOfWaferCenterDie { get; set; } 
        public int FirstDieCoordinatorX { get; set; } 
        public int FirstDieCoordinatorY { get; set; } 
        public string TestingStartYear { get; set; }
        public string TestingStartMonth { get; set; }
        public string TestingStartDay { get; set; }
        public string TestingStartHour { get; set; }
        public string TestingStartMinute { get; set; }
        public int TestingStartReserved { get; set; }
        public string TestingEndYear { get; set; }
        public string TestingEndMonth { get; set; }
        public string TestingEndDay { get; set; }
        public string TestingEndHour { get; set; }
        public string TestingEndMinute { get; set; }
        public int TestingEndReserved { get; set; }
        public string LoadEndYear { get; set; }
        public string LoadEndMonth { get; set; }
        public string LoadEndDay { get; set; }
        public string LoadEndHour { get; set; }
        public string LoadEndMinute { get; set; }
        public int LoadReserved { get; set; }
        public string UnloadStartYear { get; set; }
        public string UnloadStartMonth { get; set; }
        public string UnloadStartDay { get; set; }
        public string UnloadStartHour { get; set; }
        public string UnloadStartMinute { get; set; }
        public int UnloadReserved { get; set; }
        public int MachineNo1 { get; set; }
        public int MachineNo2 { get; set; }
        public int SpecialCharacters { get; set; }
        public int TestingEndInformation { get; set; } 
        public int TestingResultReserved { get; set; }
        public int TotalTestedDice { get; set; }
        public int TotalPassDice { get; set; }
        public int TotalFailDice { get; set; }
        public int TestDieInformationAddress { get; set; } 
        public int NumberOfLineCategoryData { get; set; }
        public int LineCategoryAddress { get; set; }
        public int MapFileConfiguration { get; set; }
        public int MaxMultiSite { get; set; }
        public int MaxCategories { get; set; }
        public int ExtendedMapInformationReserved { get; set; }
    }


}
