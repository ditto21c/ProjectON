using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UIElements;

namespace ProjectON
{
    [DefaultExecutionOrder(-100)]
    [DisallowMultipleComponent]
    public sealed class ColonySimGame : MonoBehaviour
    {
        private const int WorldWidth = 80;
        private const int WorldHeight = 46;
        private const float CycleLengthSeconds = 120f;
        private const int SaveVersion = 74;
        private const int ScenarioMilestoneTotal = 53;
        private const float AutosaveIntervalSeconds = 45f;
        private const float ObjectiveRefreshSeconds = 1f;
        private const float DefaultSleepStartCycleTime = 0.72f;
        private const float DefaultSleepEndCycleTime = 0.06f;
        private const float ScheduleStep = 0.04f;
        private const float JobQueueRefreshIntervalSeconds = 0.75f;
        private const float JobAgingPriorityStepSeconds = 55f;
        private const float JobAgingMaxSeconds = 180f;
        private const float BaseDryResourceCapacity = 300f;
        private const float StorageBinCapacity = 240f;
        private const float LiquidPipeCapacity = 10f;
        private const float LiquidPumpRate = 3.5f;
        private const float LiquidVentRate = 2.5f;
        private const float LiquidReservoirCapacity = 120f;
        private const float LiquidReservoirRate = 4.2f;
        private const float LiquidTileCapacity = 140f;
        private const float LiquidWorldStepInterval = 0.20f;
        private const float LiquidVerticalStepMass = 72f;
        private const float LiquidSideStepMass = 24f;
        private const float LiquidMinimumRetainedMass = 0.5f;
        private const float PipeFreezeTemperature = -1f;
        private const float PipeBoilTemperature = WaterEvaporationTemperature;
        private const float PipePhaseRuptureMinimumMass = 0.05f;
        private const float LiquidSensorThreshold = 2f;
        private const float GasPipeCapacity = 4f;
        private const float GasPumpRate = 0.72f;
        private const float GasVentRate = 0.82f;
        private const float GasReservoirCapacity = 36f;
        private const float GasReservoirRate = 1.1f;
        private const float ChlorineExposureThreshold = 0.08f;
        private const float ChlorineDamageRate = 0.65f;
        private const float ChlorineSterilizeRate = 0.50f;
        private const float GasSensorPressureThreshold = 1f;
        private const float GasSensorHydrogenThreshold = 0.08f;
        private const float NaturalVentCycleSeconds = 120f;
        private const float NaturalVentActiveSeconds = 80f;
        private const float SteamVentWaterRate = 0.12f;
        private const float HydrogenVentRate = 0.045f;
        private const float NaturalGasVentRate = 0.052f;
        private const float NaturalVentOutputPressure = 2.6f;
        private const float SandFallInterval = 0.24f;
        private const float WashBasinWaterUse = 1.2f;
        private const float WashBasinPollutedWaterOutput = 0.9f;
        private const float WashBasinGermReduction = 44f;
        private const float PollutedWaterOffgasMinimum = 0.5f;
        private const float BottleEmptierPourAmount = 18f;
        private const float BottleEmptierWorkRequired = 2.2f;
        private const float BottleEmptierCleanWaterReserve = 180f;
        private const int FarmStationRange = 7;
        private const float CropTendedSeconds = 90f;
        private const float CropTendedGrowthMultiplier = 1.85f;
        private const float CropTendPollutedDirtCost = 1.0f;
        private const float CropTendDirtFallbackCost = 1.8f;
        private const float CropStressThresholdSeconds = 12f;
        private const float CropWiltThresholdSeconds = 38f;
        private const float CropStressRecoveryRate = 2.4f;
        private const float CropFloodWaterMass = 8f;
        private const int AutoSweeperRange = 5;
        private const float AutoSweeperPowerRate = 0.28f;
        private const float AutoSweeperTransferRate = 2.4f;
        private const float ShippingRailCapacity = 10f;
        private const float ShippingRailMoveRate = 4.5f;
        private const float ConveyorLoaderPowerRate = 0.22f;
        private const float ConveyorLoaderTransferRate = 2.2f;
        private const float ConveyorChuteDropRate = 4.0f;
        private const float ElectrolyzerWaterRate = 0.16f;
        private const float ElectrolyzerPowerRate = 0.55f;
        private const float ElectrolyzerOxygenRate = 0.34f;
        private const float ElectrolyzerHydrogenRate = 0.055f;
        private const float CarbonSkimmerWaterRate = 0.07f;
        private const float CarbonSkimmerPowerRate = 0.36f;
        private const float CarbonSkimmerCarbonRate = 0.48f;
        private const float WaterSievePollutedWaterRate = 0.22f;
        private const float WaterSieveDirtRate = 0.012f;
        private const float WaterSievePowerRate = 0.42f;
        private const float SkillExperiencePerLevel = 80f;
        private const int MaxWorkerSkillLevel = 5;
        private const int DecorPlantRadius = 6;
        private const int HatchGroomRange = 8;
        private const int MaxWildHatches = 5;
        private const float HatchMoveIntervalSeconds = 2.4f;
        private const float HatchMoveSpeed = 2.8f;
        private const float HatchEatIntervalSeconds = 12f;
        private const float HatchEatAmount = 3.2f;
        private const float HatchCoalYield = 0.62f;
        private const float HatchGroomedSeconds = 90f;
        private const float SmartBatteryLowThreshold = 0.45f;
        private const float WireSafeLoad = 2.0f;
        private const float PowerTransformerLoadBonus = 2.0f;
        private const float WireOverloadBreakStress = 16f;
        private const float WireOverloadHeatRate = 0.06f;
        private const float CoalGeneratorPowerRate = 2.4f;
        private const float CoalGeneratorCoalRate = 0.055f;
        private const float CoalGeneratorCarbonRate = 0.18f;
        private const float HydrogenGeneratorPowerRate = 2.2f;
        private const float HydrogenGeneratorHydrogenRate = 0.08f;
        private const float NaturalGasGeneratorPowerRate = 2.6f;
        private const float NaturalGasGeneratorGasRate = 0.075f;
        private const float NaturalGasGeneratorCarbonRate = 0.16f;
        private const float NaturalGasGeneratorPollutedWaterRate = 0.045f;
        private const float SteamTurbinePowerRate = 3.2f;
        private const float SteamTurbineSteamRate = 0.19f;
        private const float SteamTurbineWaterYield = 0.92f;
        private const float SteamTurbineMinimumTemperature = 97f;
        private const int SteamTurbineRadius = 3;
        private const float SolarPanelPowerRate = 1.65f;
        private const float SolarDayStart = 0.18f;
        private const float SolarDayEnd = 0.74f;
        private const float MeteorInitialDelaySeconds = CycleLengthSeconds * 2.5f;
        private const float MeteorCooldownSeconds = CycleLengthSeconds * 1.65f;
        private const float MeteorShowerDurationSeconds = 22f;
        private const float MeteorStrikeIntervalSeconds = 2.2f;
        private const int MeteorStrikesPerWave = 2;
        private const float MeteorBunkerDoorDamage = 0.055f;
        private const float MeteorEquipmentDamage = 0.24f;
        private const float MeteorImpactHeat = 36f;
        private const float MeteorBunkerImpactHeat = 12f;
        private const float MeteorRegolithTemperature = 82f;
        private const float SpaceScannerPowerRate = 0.24f;
        private const float SpaceScannerWarningSeconds = 48f;
        private const float HydrogenFilterPowerRate = 0.38f;
        private const float HydrogenFilterRate = 0.85f;
        private const float RockCrusherOrePerJob = 8f;
        private const float RockCrusherRefinedMetalYield = 4f;
        private const float RockCrusherPowerCost = 3f;
        private const float SuitDockOxygenCapacity = 12f;
        private const float SuitDockChargeRate = 0.30f;
        private const float SuitDockPowerRate = 0.18f;
        private const float SuitBreathRate = 0.014f;
        private const float SuitCheckpointMinimumCharge = 0.05f;
        private const int MaxRecognizedRoomTiles = 96;
        private const int MaxWorkers = 8;
        private const float PrintingPodChargeSeconds = CycleLengthSeconds * 2f;
        private const float MoppableSpillMaxMass = 55f;
        private const float MopRecoveryEfficiency = 0.9f;
        private const float EquipmentAutoRepairThreshold = 0.42f;
        private const float EquipmentBrokenThreshold = 0.04f;
        private const float RepairMetalCost = 1.5f;
        private const float EquipmentOverheatTemperature = 72f;
        private const float EquipmentCriticalOverheatTemperature = 104f;
        private const float EquipmentOverheatDamageRate = 0.0075f;
        private const float EquipmentSteamOverheatMass = 0.35f;
        private const float SubmergedEquipmentWaterMass = 8f;
        private const float SubmergedEquipmentDamageRate = 0.026f;
        private const float FloodedWireWaterMass = 4f;
        private const float FloodedWireStressRate = 4.8f;
        private const float OverpressureStressThreshold = 2.20f;
        private const float OverpressureDamageThreshold = 2.65f;
        private const float OverpressureStressRate = 2.1f;
        private const float OverpressureDamageRate = 1.1f;
        private const float ThermalColdStressTemperature = 4f;
        private const float ThermalColdDamageTemperature = -5f;
        private const float ThermalHeatStressTemperature = 44f;
        private const float ThermalHeatDamageTemperature = 58f;
        private const float ThermalExposureBuildRate = 2.8f;
        private const float ThermalExposureRecoveryRate = 4.5f;
        private const float ThermalInjuryExposureThreshold = 32f;
        private const float ThermalExposureDamageRate = 1.15f;
        private const float SteamScaldMassThreshold = 0.22f;
        private const float BaseWorkerMorale = 2f;
        private const float MoraleNeedPerSkillLevel = 1.45f;
        private const float MoraleAdjustRate = 1.2f;
        private const float MoraleDeficitStressRate = 0.28f;
        private const float MoraleSurplusStressReliefRate = 0.08f;
        private const float StaleFoodFreshnessThreshold = 0.55f;
        private const float FoodPoisoningFreshnessThreshold = 0.28f;
        private const float WaterEvaporationTemperature = 101f;
        private const float SteamCondensationTemperature = 96f;
        private const float WaterEvaporationRate = 0.08f;
        private const float SteamCondensationRate = 0.06f;

        private enum CellKind
        {
            Empty,
            Dirt,
            Rock,
            Sand,
            MetalOre,
            Algae,
            Ladder,
            Floor,
            OxygenDiffuser,
            ManualGenerator,
            Battery,
            Bed,
            Planter,
            Water,
            WaterPump,
            ResearchStation,
            MicrobeMusher,
            Slime,
            AirDeodorizer,
            MedicalCot,
            Ice,
            SpaceHeater,
            ThermoRegulator,
            Outhouse,
            MassageTable,
            ManualAirlock,
            Refrigerator,
            StorageBin,
            LiquidVent,
            GasPump,
            GasVent,
            Electrolyzer,
            CarbonSkimmer,
            WaterSieve,
            MessTable,
            DecorPlant,
            Compost,
            SmartBattery,
            Coal,
            CoalGenerator,
            RockCrusher,
            AtmoSuitDock,
            InsulatedTile,
            PrintingPod,
            HydrogenGenerator,
            HydrogenFilter,
            AtmoSuitCheckpoint,
            LiquidReservoir,
            GasReservoir,
            LiquidPipeSensor,
            LiquidShutoff,
            GasPipeSensor,
            GasShutoff,
            SteamVent,
            HydrogenVent,
            RanchingStation,
            PowerTransformer,
            WashBasin,
            FarmStation,
            AutoSweeper,
            ConveyorLoader,
            ConveyorChute,
            BottleEmptier,
            SignalSwitch,
            NaturalGasVent,
            NaturalGasGenerator,
            SteamTurbine,
            SolarPanel,
            Regolith,
            BunkerDoor,
            SpaceScanner
        }

        private enum CommandMode
        {
            Inspect,
            Dig,
            Ladder,
            Floor,
            OxygenDiffuser,
            ManualGenerator,
            Battery,
            Bed,
            Planter,
            WaterPump,
            ResearchStation,
            MicrobeMusher,
            AirDeodorizer,
            Cancel,
            MedicalCot,
            SpaceHeater,
            ThermoRegulator,
            PowerWire,
            Outhouse,
            MassageTable,
            ManualAirlock,
            Refrigerator,
            StorageBin,
            LiquidPipe,
            LiquidVent,
            GasPump,
            GasPipe,
            GasVent,
            Electrolyzer,
            CarbonSkimmer,
            WaterSieve,
            MessTable,
            DecorPlant,
            Compost,
            SmartBattery,
            AutomationWire,
            CoalGenerator,
            RockCrusher,
            AtmoSuitDock,
            InsulatedTile,
            Deconstruct,
            PrintingPod,
            Mop,
            Repair,
            Sweep,
            HydrogenGenerator,
            HydrogenFilter,
            AtmoSuitCheckpoint,
            LiquidReservoir,
            GasReservoir,
            LiquidPipeSensor,
            LiquidShutoff,
            GasPipeSensor,
            GasShutoff,
            RanchingStation,
            PowerTransformer,
            WashBasin,
            FarmStation,
            AutoSweeper,
            ShippingRail,
            ConveyorLoader,
            ConveyorChute,
            BottleEmptier,
            SignalSwitch,
            NaturalGasGenerator,
            SteamTurbine,
            SolarPanel,
            BunkerDoor,
            SpaceScanner
        }

        private enum OverlayMode
        {
            Gas,
            Temperature,
            Power,
            Germs,
            Plumbing,
            Ventilation,
            Decor,
            Rooms,
            Logistics
        }

        private enum RoomKind
        {
            None,
            OpenArea,
            BasicRoom,
            Barracks,
            MessHall,
            Washroom,
            Clinic,
            RecreationRoom,
            MachineRoom,
            StorageRoom,
            MixedRoom
        }

        private enum LooseResourceKind
        {
            None,
            Dirt,
            Metal,
            Algae,
            Coal,
            RefinedMetal,
            PollutedDirt
        }

        private enum JobType
        {
            Dig,
            Build,
            OperateGenerator,
            Harvest,
            PumpWater,
            Research,
            Cook,
            Sleep,
            Treat,
            BuildWire,
            UseToilet,
            Relax,
            BuildPipe,
            BuildGasPipe,
            Eat,
            Compost,
            BuildAutomationWire,
            RefineMetal,
            Deconstruct,
            Mop,
            Repair,
            Rescue,
            Sweep,
            GroomHatch,
            WashHands,
            TendCrop,
            BuildShippingRail,
            EmptyBottle
        }

        private enum JobCategory
        {
            Survival,
            Construction,
            LifeSupport,
            FoodOps,
            PowerOps,
            ResearchOps,
            Logistics,
            Maintenance,
            Industry,
            Ranching,
            MoraleCare
        }

        private readonly struct BuildSpec
        {
            public BuildSpec(CellKind kind, string label, float dirt, float metal, float algae, float work, float refinedMetal = 0f)
            {
                Kind = kind;
                Label = label;
                Dirt = dirt;
                Metal = metal;
                Algae = algae;
                Work = work;
                RefinedMetal = refinedMetal;
            }

            public readonly CellKind Kind;
            public readonly string Label;
            public readonly float Dirt;
            public readonly float Metal;
            public readonly float Algae;
            public readonly float Work;
            public readonly float RefinedMetal;
        }

        private sealed class Job
        {
            public Job(JobType type, Vector2Int cell, float workRequired)
            {
                Type = type;
                Cell = cell;
                WorkRequired = Mathf.Max(0.1f, workRequired);
            }

            public JobType Type;
            public Vector2Int Cell;
            public CellKind BuildKind;
            public float WorkRequired;
            public float Progress;
            public float DirtCost;
            public float MetalCost;
            public float AlgaeCost;
            public float RefinedMetalCost;
            public int Priority;
            public float AgeSeconds;
            public string TargetWorkerName;
            public bool BuildWire;
            public bool BuildPipe;
            public bool BuildGasPipe;
            public bool BuildShippingRail;
            public bool RemovePowerWire;
            public bool RemoveAutomationWire;
            public bool RemoveLiquidPipe;
            public bool RemoveGasPipe;
            public bool RemoveShippingRail;
            public Worker AssignedWorker;
            public bool Cancelled;
            public bool AutoGenerated;
        }

        private sealed class Worker
        {
            public string Name;
            public Transform Transform;
            public SpriteRenderer Renderer;
            public Vector2Int Cell;
            public List<Vector2Int> Path = new List<Vector2Int>();
            public int PathIndex;
            public Job AssignedJob;
            public string Activity = "Idle";
            public float Calories = 2800f;
            public float Stress;
            public float Health = 100f;
            public float Morale = BaseWorkerMorale;
            public float Fatigue = 25f;
            public float Bladder = 20f;
            public float GermExposure;
            public float Sickness;
            public float HeatExposure;
            public float ChillExposure;
            public float StressBreakSeconds;
            public float StressBreakPulseTimer;
            public float IncapacitatedSeconds;
            public float Experience;
            public float WorkSpeed = 1f;
            public float MoveSpeed = 5f;
            public bool SuitEquipped;
        }

        private sealed class HatchCritter
        {
            public string Name;
            public Transform Transform;
            public SpriteRenderer Renderer;
            public Vector2Int Cell;
            public Vector2Int TargetCell;
            public float MoveTimer;
            public float EatTimer;
            public float GroomedSeconds;
            public float Happiness = 35f;
            public float CoalProduced;
        }

        private sealed class RoomInfo
        {
            public int Id;
            public RoomKind Kind;
            public int Tiles;
            public bool Enclosed;
            public int Beds;
            public int MessTables;
            public int Outhouses;
            public int WashBasins;
            public int MedicalCots;
            public int MassageTables;
            public int StorageBins;
            public int MachineBuildings;
            public int DecorPlants;
            public float AverageOxygen;
            public float AverageTemperature;
        }

        [Serializable]
        private sealed class SaveData
        {
            public int version;
            public int width;
            public int height;
            public int[] cells;
            public float[] oxygen;
            public float[] carbonDioxide;
            public float[] pollutedOxygen;
            public float[] hydrogen;
            public float[] steam;
            public float[] chlorine;
            public float[] naturalGas;
            public float[] germs;
            public float[] plantGrowth;
            public float[] cropTendedSeconds;
            public float[] cropStress;
            public float[] waterMass;
            public float[] temperature;
            public float[] equipmentCondition;
            public int[] looseResourceKind;
            public float[] looseResourceAmount;
            public bool[] powerWire;
            public bool[] automationWire;
            public bool[] automationSwitchState;
            public bool[] airlockOpen;
            public bool[] shippingRail;
            public int[] shippingRailKind;
            public float[] shippingRailAmount;
            public bool[] liquidPipe;
            public float[] pipeWater;
            public float[] liquidReservoirWater;
            public bool[] gasPipe;
            public float[] gasPipeOxygen;
            public float[] gasPipeCarbonDioxide;
            public float[] gasPipePollutedOxygen;
            public float[] gasPipeHydrogen;
            public float[] gasPipeChlorine;
            public float[] gasPipeNaturalGas;
            public float[] gasPipeGerms;
            public float[] gasReservoirOxygen;
            public float[] gasReservoirCarbonDioxide;
            public float[] gasReservoirPollutedOxygen;
            public float[] gasReservoirHydrogen;
            public float[] gasReservoirChlorine;
            public float[] gasReservoirNaturalGas;
            public float[] gasReservoirGerms;
            public WorkerSave[] workers;
            public HatchSave[] hatches;
            public JobSave[] jobs;
            public float dirt;
            public float metal;
            public float algae;
            public float coal;
            public float refinedMetal;
            public float suitOxygen;
            public float suitOxygenUsed;
            public int suitCheckpointUses;
            public int suitEntryDenials;
            public int sandFalls;
            public int sandStrikeInjuries;
            public float liquidFlowedMass;
            public int liquidFlowEvents;
            public float pipeBurstWater;
            public int pipeBurstEvents;
            public int frozenPipeBursts;
            public int boiledPipeBursts;
            public float reservoirBurstWater;
            public int reservoirBurstEvents;
            public int frozenReservoirBursts;
            public int boiledReservoirBursts;
            public int iceMeltedTiles;
            public int waterFrozenTiles;
            public float steamEvaporatedMass;
            public float steamCondensedMass;
            public float chlorineSterilizedGerms;
            public float chlorineExposureSeconds;
            public float chlorineHealthDamage;
            public float submergedEquipmentDamage;
            public int floodedWireFailures;
            public float overheatedEquipmentDamage;
            public int overheatedEquipmentFailures;
            public float overpressureExposureSeconds;
            public float overpressureHealthDamage;
            public float thermalExposureSeconds;
            public float thermalHealthDamage;
            public int heatStrokeCases;
            public int hypothermiaCases;
            public float moralePressureSeconds;
            public float moraleStressAdded;
            public int staleMealsEaten;
            public int foodPoisoningCases;
            public float printingPodProgress;
            public float water;
            public float pollutedWater;
            public float pollutedWaterOffgassedMass;
            public int pollutedWaterOffgasEvents;
            public float pollutedDirt;
            public float recycledWater;
            public float researchPoints;
            public float food;
            public float foodFreshness;
            public float power;
            public float maxPower;
            public float elapsedTime;
            public float cycleTimer;
            public float sleepStartCycleTime;
            public float sleepEndCycleTime;
            public int cycle;
            public int currentMode;
            public int currentOverlayMode;
            public bool milestoneBasicShelter;
            public bool milestoneStableOxygen;
            public bool milestoneFoodProduction;
            public bool milestonePowerBuffer;
            public bool milestoneCycleFive;
            public bool milestoneResearchProgram;
            public bool milestoneWaterSupply;
            public bool milestoneFoodPreparation;
            public bool milestoneThermalControl;
            public bool milestonePowerGrid;
            public bool milestoneSanitation;
            public bool milestoneMoraleCare;
            public bool milestonePressureControl;
            public bool milestoneAirlockControl;
            public bool milestoneFoodStorage;
            public bool milestoneMaterialStorage;
            public bool milestonePlumbing;
            public bool milestoneVentilation;
            public bool milestoneAdvancedAtmosphere;
            public bool milestoneWaterRecycling;
            public bool milestoneDining;
            public bool milestoneSkilledLabor;
            public bool milestoneDecorComfort;
            public bool milestoneWasteProcessing;
            public bool milestoneAutomation;
            public bool milestoneFuelPower;
            public bool milestoneMetalRefining;
            public bool milestoneAtmoSuits;
            public bool milestoneInsulation;
            public bool milestoneRoomPlanning;
            public bool milestoneReconfiguration;
            public bool milestoneColonyExpansion;
            public bool milestoneSpillCleanup;
            public bool milestoneMaintenance;
            public bool milestoneEmergencyResponse;
            public bool milestoneResourceLogistics;
            public bool milestoneHydrogenPower;
            public bool milestoneHydrogenFiltering;
            public bool milestoneReservoirBuffering;
            public bool milestoneConduitAutomation;
            public bool milestoneRenewableVents;
            public bool milestoneRanching;
            public bool milestonePowerLoadManagement;
            public bool milestoneHygiene;
            public bool milestoneCropTending;
            public bool milestoneAutoSweeping;
            public bool milestoneShippingLogistics;
            public bool milestoneBottleEmptying;
            public bool milestoneSignalSwitching;
            public bool milestoneSteamPower;
            public bool milestoneSolarPower;
            public bool milestoneMeteorShielding;
            public bool milestoneSpaceScanning;
            public int mealsEatenAtTable;
            public float compostedPollutedDirt;
            public float coalPowerGenerated;
            public float refinedMetalProduced;
            public int deconstructionsCompleted;
            public int duplicantsPrinted;
            public float moppedLiquid;
            public int repairsCompleted;
            public int equipmentFailures;
            public int rescuesCompleted;
            public float sweptResources;
            public float hydrogenPowerGenerated;
            public float naturalGasPowerGenerated;
            public float steamTurbinePowerGenerated;
            public float steamTurbineWaterRecovered;
            public float solarPowerGenerated;
            public float solarBlockedSeconds;
            public float meteorShowerSeconds;
            public float meteorCooldownSeconds;
            public float meteorStrikeTimer;
            public int meteorStrikes;
            public int meteorImpactsBlocked;
            public int meteorDamageEvents;
            public float meteorRegolithDeposited;
            public float spaceScannerSignalSeconds;
            public float spaceScannerBlockedSeconds;
            public float hydrogenFilteredGas;
            public float reservoirBufferedMass;
            public float automatedConduitFlow;
            public float renewableWaterGenerated;
            public float renewableHydrogenGenerated;
            public float renewableNaturalGasGenerated;
            public float hatchCoalProduced;
            public int hatchesGroomed;
            public float transformedPowerDelivered;
            public float overloadedWireSeconds;
            public int handsWashed;
            public int cropsTended;
            public float cropStifledSeconds;
            public int cropsWilted;
            public float autoSweptResources;
            public float conveyorShippedResources;
            public float bottleEmptiedLiquid;
            public int signalSwitchesToggled;
            public int airlockToggles;
            public bool techAirSystems;
            public bool techFoodPreparation;
            public bool techPowerRegulation;
            public bool colonyVictory;
            public bool colonyFailed;
            public string lastLog;
            public string language;
        }

        [Serializable]
        private sealed class WorkerSave
        {
            public string name;
            public int cellX;
            public int cellY;
            public float positionX;
            public float positionY;
            public float calories;
            public float stress;
            public float health;
            public float morale;
            public float fatigue;
            public float bladder;
            public float germExposure;
            public float sickness;
            public float heatExposure;
            public float chillExposure;
            public float stressBreakSeconds;
            public float incapacitatedSeconds;
            public float experience;
            public bool suitEquipped;
        }

        [Serializable]
        private sealed class HatchSave
        {
            public string name;
            public int cellX;
            public int cellY;
            public float positionX;
            public float positionY;
            public float moveTimer;
            public float eatTimer;
            public float groomedSeconds;
            public float happiness;
            public float coalProduced;
        }

        [Serializable]
        private sealed class JobSave
        {
            public int type;
            public int cellX;
            public int cellY;
            public int buildKind;
            public float workRequired;
            public float progress;
            public float dirtCost;
            public float metalCost;
            public float algaeCost;
            public float refinedMetalCost;
            public int priority;
            public float ageSeconds;
            public bool autoGenerated;
            public string targetWorkerName;
            public bool buildWire;
            public bool buildPipe;
            public bool buildGasPipe;
            public bool buildShippingRail;
            public bool removePowerWire;
            public bool removeAutomationWire;
            public bool removeLiquidPipe;
            public bool removeGasPipe;
            public bool removeShippingRail;
        }

        private readonly CellKind[,] cells = new CellKind[WorldWidth, WorldHeight];
        private readonly float[,] oxygen = new float[WorldWidth, WorldHeight];
        private readonly float[,] carbonDioxide = new float[WorldWidth, WorldHeight];
        private readonly float[,] pollutedOxygen = new float[WorldWidth, WorldHeight];
        private readonly float[,] germs = new float[WorldWidth, WorldHeight];
        private readonly float[,] nextOxygen = new float[WorldWidth, WorldHeight];
        private readonly float[,] nextCarbonDioxide = new float[WorldWidth, WorldHeight];
        private readonly float[,] nextPollutedOxygen = new float[WorldWidth, WorldHeight];
        private readonly float[,] hydrogen = new float[WorldWidth, WorldHeight];
        private readonly float[,] nextHydrogen = new float[WorldWidth, WorldHeight];
        private readonly float[,] steam = new float[WorldWidth, WorldHeight];
        private readonly float[,] nextSteam = new float[WorldWidth, WorldHeight];
        private readonly float[,] chlorine = new float[WorldWidth, WorldHeight];
        private readonly float[,] nextChlorine = new float[WorldWidth, WorldHeight];
        private readonly float[,] naturalGas = new float[WorldWidth, WorldHeight];
        private readonly float[,] nextNaturalGas = new float[WorldWidth, WorldHeight];
        private readonly float[,] nextGerms = new float[WorldWidth, WorldHeight];
        private readonly float[,] plantGrowth = new float[WorldWidth, WorldHeight];
        private readonly float[,] cropTendedSeconds = new float[WorldWidth, WorldHeight];
        private readonly float[,] cropStress = new float[WorldWidth, WorldHeight];
        private readonly float[,] waterMass = new float[WorldWidth, WorldHeight];
        private readonly float[,] temperature = new float[WorldWidth, WorldHeight];
        private readonly float[,] nextTemperature = new float[WorldWidth, WorldHeight];
        private readonly float[,] equipmentCondition = new float[WorldWidth, WorldHeight];
        private readonly LooseResourceKind[,] looseResourceKind = new LooseResourceKind[WorldWidth, WorldHeight];
        private readonly float[,] looseResourceAmount = new float[WorldWidth, WorldHeight];
        private readonly bool[,] powerWire = new bool[WorldWidth, WorldHeight];
        private readonly bool[,] poweredWire = new bool[WorldWidth, WorldHeight];
        private readonly float[,] wireLoad = new float[WorldWidth, WorldHeight];
        private readonly bool[,] overloadedWire = new bool[WorldWidth, WorldHeight];
        private readonly float[,] wireOverloadStress = new float[WorldWidth, WorldHeight];
        private readonly bool[,] automationWire = new bool[WorldWidth, WorldHeight];
        private readonly bool[,] automationControlledWire = new bool[WorldWidth, WorldHeight];
        private readonly bool[,] automationSignalWire = new bool[WorldWidth, WorldHeight];
        private readonly bool[,] automationSwitchState = new bool[WorldWidth, WorldHeight];
        private readonly bool[,] airlockOpen = new bool[WorldWidth, WorldHeight];
        private readonly bool[,] shippingRail = new bool[WorldWidth, WorldHeight];
        private readonly LooseResourceKind[,] shippingRailKind = new LooseResourceKind[WorldWidth, WorldHeight];
        private readonly float[,] shippingRailAmount = new float[WorldWidth, WorldHeight];
        private readonly bool[,] liquidPipe = new bool[WorldWidth, WorldHeight];
        private readonly float[,] pipeWater = new float[WorldWidth, WorldHeight];
        private readonly float[,] liquidReservoirWater = new float[WorldWidth, WorldHeight];
        private readonly bool[,] gasPipe = new bool[WorldWidth, WorldHeight];
        private readonly float[,] gasPipeOxygen = new float[WorldWidth, WorldHeight];
        private readonly float[,] gasPipeCarbonDioxide = new float[WorldWidth, WorldHeight];
        private readonly float[,] gasPipePollutedOxygen = new float[WorldWidth, WorldHeight];
        private readonly float[,] gasPipeHydrogen = new float[WorldWidth, WorldHeight];
        private readonly float[,] gasPipeChlorine = new float[WorldWidth, WorldHeight];
        private readonly float[,] gasPipeNaturalGas = new float[WorldWidth, WorldHeight];
        private readonly float[,] gasPipeGerms = new float[WorldWidth, WorldHeight];
        private readonly float[,] gasReservoirOxygen = new float[WorldWidth, WorldHeight];
        private readonly float[,] gasReservoirCarbonDioxide = new float[WorldWidth, WorldHeight];
        private readonly float[,] gasReservoirPollutedOxygen = new float[WorldWidth, WorldHeight];
        private readonly float[,] gasReservoirHydrogen = new float[WorldWidth, WorldHeight];
        private readonly float[,] gasReservoirChlorine = new float[WorldWidth, WorldHeight];
        private readonly float[,] gasReservoirNaturalGas = new float[WorldWidth, WorldHeight];
        private readonly float[,] gasReservoirGerms = new float[WorldWidth, WorldHeight];
        private readonly int[,] roomIds = new int[WorldWidth, WorldHeight];

        private readonly List<Job> jobs = new List<Job>();
        private readonly List<Worker> workers = new List<Worker>();
        private readonly List<HatchCritter> hatches = new List<HatchCritter>();
        private readonly List<RoomInfo> rooms = new List<RoomInfo>();
        private readonly List<Vector2Int> pollutedWaterOffgasSources = new List<Vector2Int>();
        private readonly Dictionary<CommandMode, Button> modeButtons = new Dictionary<CommandMode, Button>();
        private readonly Dictionary<OverlayMode, Button> overlayButtons = new Dictionary<OverlayMode, Button>();
        private readonly Dictionary<TextElement, string> localizedStaticTexts = new Dictionary<TextElement, string>();
        private readonly HashSet<int> dragCells = new HashSet<int>();

        private Texture2D terrainTexture;
        private Texture2D gasTexture;
        private Texture2D overlayTexture;
        private SpriteRenderer terrainRenderer;
        private SpriteRenderer gasRenderer;
        private SpriteRenderer overlayRenderer;
        private Sprite workerSprite;
        private Sprite hatchSprite;
        private Camera gameCamera;
        private UIDocument uiDocument;
        private PanelSettings uiPanelSettings;
        private VisualElement uiRoot;
        private VisualElement overlayLegendRows;
        private Label statsText;
        private Label modeText;
        private Label scenarioText;
        private Label jobQueueText;
        private Label overlayLegendTitleText;
        private Label infoText;
        private Label logText;
        private Button priorityDownButton;
        private Button priorityUpButton;
        private Button cancelSelectedJobButton;
        private Button signalSwitchButton;
        private Button airlockToggleButton;
        private Button languageButton;
        private VisualElement endStatePanel;
        private Label endStateTitleText;
        private Label endStateBodyText;
        private Button endStateLoadButton;
        private Button endStateNewRunButton;
        private Button endStateContinueButton;
        private Font uiFont;

        private CommandMode currentMode = CommandMode.Inspect;
        private OverlayMode currentOverlayMode = OverlayMode.Gas;
        private OverlayMode renderedLegendOverlayMode = (OverlayMode)(-1);
        private ProjectONLanguage renderedLegendLanguage = (ProjectONLanguage)(-1);
        private ProjectONLanguage renderedJobQueueLanguage = (ProjectONLanguage)(-1);
        private float nextJobQueueRefreshTime = -1f;
        private string jobQueueTextCache = string.Empty;
        private ProjectONLanguage currentLanguage = ProjectONLanguage.Korean;
        private Vector2Int? inspectedCell;
        private string lastLog = "Colony online.";
        private string objectiveText = "Build beds, oxygen, power, and food production.";
        private string alertText = "Stable.";
        private int unreachableJobCount;
        private float dirt = 130f;
        private float metal = 80f;
        private float algae = 45f;
        private float coal;
        private float refinedMetal;
        private float suitOxygen;
        private float suitOxygenUsed;
        private int suitCheckpointUses;
        private int suitEntryDenials;
        private int sandFalls;
        private int sandStrikeInjuries;
        private float liquidFlowedMass;
        private int liquidFlowEvents;
        private float pipeBurstWater;
        private int pipeBurstEvents;
        private int frozenPipeBursts;
        private int boiledPipeBursts;
        private float reservoirBurstWater;
        private int reservoirBurstEvents;
        private int frozenReservoirBursts;
        private int boiledReservoirBursts;
        private int iceMeltedTiles;
        private int waterFrozenTiles;
        private float steamEvaporatedMass;
        private float steamCondensedMass;
        private float chlorineSterilizedGerms;
        private float chlorineExposureSeconds;
        private float chlorineHealthDamage;
        private float submergedEquipmentDamage;
        private int floodedWireFailures;
        private float overheatedEquipmentDamage;
        private int overheatedEquipmentFailures;
        private float overpressureExposureSeconds;
        private float overpressureHealthDamage;
        private float thermalExposureSeconds;
        private float thermalHealthDamage;
        private int heatStrokeCases;
        private int hypothermiaCases;
        private float moralePressureSeconds;
        private float moraleStressAdded;
        private int staleMealsEaten;
        private int foodPoisoningCases;
        private float printingPodProgress;
        private float water = 45f;
        private float pollutedWater;
        private float pollutedWaterOffgassedMass;
        private int pollutedWaterOffgasEvents;
        private float pollutedDirt;
        private float recycledWater;
        private float researchPoints;
        private float food = 3600f;
        private float foodFreshness = 0.82f;
        private float power = 35f;
        private float maxPower = 100f;
        private float elapsedTime;
        private float cycleTimer;
        private float sleepStartCycleTime = DefaultSleepStartCycleTime;
        private float sleepEndCycleTime = DefaultSleepEndCycleTime;
        private float gasTimer;
        private float thermalTimer;
        private float liquidTimer;
        private float sandTimer;
        private float maintenanceTimer;
        private float autosaveTimer;
        private float objectiveTimer;
        private int cycle = 1;
        private bool paused;
        private float simulationSpeed = 1f;
        private bool milestoneBasicShelter;
        private bool milestoneStableOxygen;
        private bool milestoneFoodProduction;
        private bool milestonePowerBuffer;
        private bool milestoneCycleFive;
        private bool milestoneResearchProgram;
        private bool milestoneWaterSupply;
        private bool milestoneFoodPreparation;
        private bool milestoneThermalControl;
        private bool milestonePowerGrid;
        private bool milestoneSanitation;
        private bool milestoneMoraleCare;
        private bool milestonePressureControl;
        private bool milestoneAirlockControl;
        private bool milestoneFoodStorage;
        private bool milestoneMaterialStorage;
        private bool milestonePlumbing;
        private bool milestoneVentilation;
        private bool milestoneAdvancedAtmosphere;
        private bool milestoneWaterRecycling;
        private bool milestoneDining;
        private bool milestoneSkilledLabor;
        private bool milestoneDecorComfort;
        private bool milestoneWasteProcessing;
        private bool milestoneAutomation;
        private bool milestoneFuelPower;
        private bool milestoneMetalRefining;
        private bool milestoneAtmoSuits;
        private bool milestoneInsulation;
        private bool milestoneRoomPlanning;
        private bool milestoneReconfiguration;
        private bool milestoneColonyExpansion;
        private bool milestoneSpillCleanup;
        private bool milestoneMaintenance;
        private bool milestoneEmergencyResponse;
        private bool milestoneResourceLogistics;
        private bool milestoneHydrogenPower;
        private bool milestoneHydrogenFiltering;
        private bool milestoneReservoirBuffering;
        private bool milestoneConduitAutomation;
        private bool milestoneRenewableVents;
        private bool milestoneRanching;
        private bool milestonePowerLoadManagement;
        private bool milestoneHygiene;
        private bool milestoneCropTending;
        private bool milestoneAutoSweeping;
        private bool milestoneShippingLogistics;
        private bool milestoneBottleEmptying;
        private bool milestoneSignalSwitching;
        private bool milestoneSteamPower;
        private bool milestoneSolarPower;
        private bool milestoneMeteorShielding;
        private bool milestoneSpaceScanning;
        private int mealsEatenAtTable;
        private float compostedPollutedDirt;
        private float coalPowerGenerated;
        private float refinedMetalProduced;
        private int deconstructionsCompleted;
        private int duplicantsPrinted;
        private float moppedLiquid;
        private int repairsCompleted;
        private int equipmentFailures;
        private int rescuesCompleted;
        private float sweptResources;
        private float hydrogenPowerGenerated;
        private float naturalGasPowerGenerated;
        private float steamTurbinePowerGenerated;
        private float steamTurbineWaterRecovered;
        private float solarPowerGenerated;
        private float solarBlockedSeconds;
        private float meteorShowerSeconds;
        private float meteorCooldownSeconds;
        private float meteorStrikeTimer;
        private int meteorStrikes;
        private int meteorImpactsBlocked;
        private int meteorDamageEvents;
        private float meteorRegolithDeposited;
        private float spaceScannerSignalSeconds;
        private float spaceScannerBlockedSeconds;
        private float hydrogenFilteredGas;
        private float reservoirBufferedMass;
        private float automatedConduitFlow;
        private float renewableWaterGenerated;
        private float renewableHydrogenGenerated;
        private float renewableNaturalGasGenerated;
        private float hatchCoalProduced;
        private int hatchesGroomed;
        private float transformedPowerDelivered;
        private float overloadedWireSeconds;
        private int handsWashed;
        private int cropsTended;
        private float cropStifledSeconds;
        private int cropsWilted;
        private float autoSweptResources;
        private float conveyorShippedResources;
        private float bottleEmptiedLiquid;
        private int signalSwitchesToggled;
        private int airlockToggles;
        private bool techAirSystems;
        private bool techFoodPreparation;
        private bool techPowerRegulation;
        private bool colonyVictory;
        private bool colonyVictoryAcknowledged;
        private bool colonyFailed;
        private bool terrainDirty = true;
        private bool gasDirty = true;
        private bool overlayDirty = true;
        private bool roomsDirty = true;
        private bool isDraggingCommand;
        private bool isPanningCamera;
        private Vector2 lastPanScreenPosition;

        private string SaveFilePath
        {
            get { return Path.Combine(Application.persistentDataPath, "projecton_colony_save.json"); }
        }

        private void Awake()
        {
            ConfigureCamera();
            CreateRenderLayers();
            GenerateWorld();
            CreateWorkers();
            BuildHud();
            RenderTerrain();
            RenderGas();
            RenderOverlay();
            UpdateColonyStatus(true);
            UpdateHud();
        }

        private void Update()
        {
            HandleKeyboardShortcuts();
            HandleCameraControls();
            HandlePointerCommands();
            EnsureWorldState();

            if (!paused)
            {
                float scaledDeltaTime = Mathf.Clamp(Time.deltaTime * simulationSpeed, 0f, 0.12f);
                SimulateColony(scaledDeltaTime);
                EnsureWorkerRecords();
                EnsureHatchTransforms();
                UpdateWorkers(scaledDeltaTime);
            }

            if (terrainDirty)
            {
                RenderTerrain();
            }

            if (gasDirty)
            {
                RenderGas();
            }

            if (overlayDirty)
            {
                RenderOverlay();
            }

            UpdateHud();
        }

        private void OnDestroy()
        {
            DestroyRuntimeObject(terrainTexture);
            DestroyRuntimeObject(gasTexture);
            DestroyRuntimeObject(overlayTexture);
            if (workerSprite != null)
            {
                DestroyRuntimeObject(workerSprite.texture);
                DestroyRuntimeObject(workerSprite);
            }

            if (hatchSprite != null)
            {
                DestroyRuntimeObject(hatchSprite.texture);
                DestroyRuntimeObject(hatchSprite);
            }
        }

        private void ConfigureCamera()
        {
            gameCamera = Camera.main;
            if (gameCamera == null)
            {
                GameObject cameraObject = new GameObject("Main Camera");
                cameraObject.tag = "MainCamera";
                gameCamera = cameraObject.AddComponent<Camera>();
                cameraObject.AddComponent<AudioListener>();
            }

            gameCamera.orthographic = true;
            gameCamera.orthographicSize = 15f;
            gameCamera.transform.position = new Vector3(WorldWidth * 0.5f, WorldHeight * 0.5f, -10f);
            gameCamera.backgroundColor = new Color(0.03f, 0.04f, 0.06f);
            gameCamera.clearFlags = CameraClearFlags.SolidColor;
        }

        private void CreateRenderLayers()
        {
            terrainTexture = CreateWorldTexture("Terrain Texture");
            gasTexture = CreateWorldTexture("Gas Texture");
            overlayTexture = CreateWorldTexture("Overlay Texture");

            terrainRenderer = CreateSpriteLayer("Terrain Layer", terrainTexture, 0);
            gasRenderer = CreateSpriteLayer("Gas Layer", gasTexture, 1);
            overlayRenderer = CreateSpriteLayer("Command Overlay", overlayTexture, 2);
            workerSprite = CreateWorkerSprite();
            hatchSprite = CreateHatchSprite();
        }

        private Texture2D CreateWorldTexture(string textureName)
        {
            Texture2D texture = new Texture2D(WorldWidth, WorldHeight, TextureFormat.RGBA32, false)
            {
                name = textureName,
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
            return texture;
        }

        private SpriteRenderer CreateSpriteLayer(string layerName, Texture2D texture, int sortingOrder)
        {
            GameObject layerObject = new GameObject(layerName);
            layerObject.transform.SetParent(transform, false);
            layerObject.transform.position = Vector3.zero;
            SpriteRenderer spriteRenderer = layerObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Sprite.Create(texture, new Rect(0f, 0f, WorldWidth, WorldHeight), Vector2.zero, 1f);
            spriteRenderer.sortingOrder = sortingOrder;
            return spriteRenderer;
        }

        private Sprite CreateWorkerSprite()
        {
            const int width = 18;
            const int height = 24;
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false)
            {
                name = "Duplicant Worker Sprite",
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            Color clear = new Color(0f, 0f, 0f, 0f);
            Color suit = new Color(0.28f, 0.72f, 0.95f, 1f);
            Color suitShadow = new Color(0.12f, 0.34f, 0.48f, 1f);
            Color face = new Color(0.96f, 0.68f, 0.48f, 1f);
            Color hair = new Color(0.18f, 0.11f, 0.06f, 1f);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    texture.SetPixel(x, y, clear);
                    float nx = (x - width * 0.5f) / (width * 0.5f);
                    float bodyNy = (y - 8f) / 9f;
                    float headNy = (y - 17f) / 6f;

                    if (nx * nx * 0.65f + bodyNy * bodyNy <= 1f && y < 17)
                    {
                        texture.SetPixel(x, y, x < width * 0.45f ? suitShadow : suit);
                    }

                    if (nx * nx + headNy * headNy <= 1f && y >= 12)
                    {
                        texture.SetPixel(x, y, face);
                    }

                    if (nx * nx + (headNy + 0.35f) * (headNy + 0.35f) <= 0.7f && y >= 18)
                    {
                        texture.SetPixel(x, y, hair);
                    }
                }
            }

            texture.Apply(false, false);
            return Sprite.Create(texture, new Rect(0f, 0f, width, height), new Vector2(0.5f, 0.08f), 18f);
        }

        private Sprite CreateHatchSprite()
        {
            const int width = 18;
            const int height = 14;
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false)
            {
                name = "Hatch Critter Sprite",
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            Color clear = new Color(0f, 0f, 0f, 0f);
            Color shell = new Color(0.46f, 0.34f, 0.20f, 1f);
            Color belly = new Color(0.68f, 0.52f, 0.32f, 1f);
            Color shadow = new Color(0.18f, 0.12f, 0.08f, 1f);
            Color eye = new Color(0.04f, 0.035f, 0.025f, 1f);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    texture.SetPixel(x, y, clear);
                    float nx = (x - width * 0.50f) / (width * 0.50f);
                    float ny = (y - height * 0.42f) / (height * 0.42f);
                    if (nx * nx * 0.90f + ny * ny <= 1f && y < height - 2)
                    {
                        texture.SetPixel(x, y, x < width * 0.42f ? shadow : shell);
                    }

                    float bny = (y - height * 0.33f) / (height * 0.22f);
                    if (nx * nx * 1.2f + bny * bny <= 1f && y < height * 0.56f)
                    {
                        texture.SetPixel(x, y, belly);
                    }

                    if ((x == 12 || x == 13) && (y == 8 || y == 9))
                    {
                        texture.SetPixel(x, y, eye);
                    }
                }
            }

            texture.Apply(false, false);
            return Sprite.Create(texture, new Rect(0f, 0f, width, height), new Vector2(0.5f, 0.05f), 18f);
        }

        private void GenerateWorld()
        {
            pollutedWater = 0f;
            pollutedWaterOffgassedMass = 0f;
            pollutedWaterOffgasEvents = 0;
            pollutedDirt = 0f;
            recycledWater = 0f;
            mealsEatenAtTable = 0;
            compostedPollutedDirt = 0f;
            coalPowerGenerated = 0f;
            refinedMetalProduced = 0f;
            suitOxygen = 0f;
            suitOxygenUsed = 0f;
            suitCheckpointUses = 0;
            suitEntryDenials = 0;
            sandFalls = 0;
            sandStrikeInjuries = 0;
            liquidFlowedMass = 0f;
            liquidFlowEvents = 0;
            pipeBurstWater = 0f;
            pipeBurstEvents = 0;
            frozenPipeBursts = 0;
            boiledPipeBursts = 0;
            reservoirBurstWater = 0f;
            reservoirBurstEvents = 0;
            frozenReservoirBursts = 0;
            boiledReservoirBursts = 0;
            iceMeltedTiles = 0;
            waterFrozenTiles = 0;
            steamEvaporatedMass = 0f;
            steamCondensedMass = 0f;
            chlorineSterilizedGerms = 0f;
            chlorineExposureSeconds = 0f;
            chlorineHealthDamage = 0f;
            submergedEquipmentDamage = 0f;
            floodedWireFailures = 0;
            overheatedEquipmentDamage = 0f;
            overheatedEquipmentFailures = 0;
            overpressureExposureSeconds = 0f;
            overpressureHealthDamage = 0f;
            thermalExposureSeconds = 0f;
            thermalHealthDamage = 0f;
            heatStrokeCases = 0;
            hypothermiaCases = 0;
            moralePressureSeconds = 0f;
            moraleStressAdded = 0f;
            staleMealsEaten = 0;
            foodPoisoningCases = 0;
            liquidTimer = 0f;
            printingPodProgress = 0f;
            duplicantsPrinted = 0;
            moppedLiquid = 0f;
            repairsCompleted = 0;
            equipmentFailures = 0;
            rescuesCompleted = 0;
            sweptResources = 0f;
            hydrogenPowerGenerated = 0f;
            naturalGasPowerGenerated = 0f;
            steamTurbinePowerGenerated = 0f;
            steamTurbineWaterRecovered = 0f;
            milestoneSteamPower = false;
            solarPowerGenerated = 0f;
            solarBlockedSeconds = 0f;
            milestoneSolarPower = false;
            meteorShowerSeconds = 0f;
            meteorCooldownSeconds = MeteorInitialDelaySeconds;
            meteorStrikeTimer = 0f;
            meteorStrikes = 0;
            meteorImpactsBlocked = 0;
            meteorDamageEvents = 0;
            meteorRegolithDeposited = 0f;
            milestoneMeteorShielding = false;
            spaceScannerSignalSeconds = 0f;
            spaceScannerBlockedSeconds = 0f;
            milestoneSpaceScanning = false;
            hydrogenFilteredGas = 0f;
            reservoirBufferedMass = 0f;
            automatedConduitFlow = 0f;
            renewableWaterGenerated = 0f;
            renewableHydrogenGenerated = 0f;
            renewableNaturalGasGenerated = 0f;
            hatchCoalProduced = 0f;
            hatchesGroomed = 0;
            transformedPowerDelivered = 0f;
            overloadedWireSeconds = 0f;
            handsWashed = 0;
            cropsTended = 0;
            cropStifledSeconds = 0f;
            cropsWilted = 0;
            autoSweptResources = 0f;
            conveyorShippedResources = 0f;
            bottleEmptiedLiquid = 0f;
            signalSwitchesToggled = 0;
            airlockToggles = 0;
            ClearHatches();
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    powerWire[x, y] = false;
                    poweredWire[x, y] = false;
                    wireLoad[x, y] = 0f;
                    overloadedWire[x, y] = false;
                    wireOverloadStress[x, y] = 0f;
                    automationWire[x, y] = false;
                    automationControlledWire[x, y] = false;
                    automationSignalWire[x, y] = false;
                    automationSwitchState[x, y] = false;
                    airlockOpen[x, y] = false;
                    shippingRail[x, y] = false;
                    shippingRailKind[x, y] = LooseResourceKind.None;
                    shippingRailAmount[x, y] = 0f;
                    liquidPipe[x, y] = false;
                    pipeWater[x, y] = 0f;
                    liquidReservoirWater[x, y] = 0f;
                    gasPipe[x, y] = false;
                    gasPipeOxygen[x, y] = 0f;
                    gasPipeCarbonDioxide[x, y] = 0f;
                    gasPipePollutedOxygen[x, y] = 0f;
                    gasPipeHydrogen[x, y] = 0f;
                    gasPipeChlorine[x, y] = 0f;
                    gasPipeNaturalGas[x, y] = 0f;
                    gasPipeGerms[x, y] = 0f;
                    steam[x, y] = 0f;
                    chlorine[x, y] = 0f;
                    naturalGas[x, y] = 0f;
                    gasReservoirOxygen[x, y] = 0f;
                    gasReservoirCarbonDioxide[x, y] = 0f;
                    gasReservoirPollutedOxygen[x, y] = 0f;
                    gasReservoirHydrogen[x, y] = 0f;
                    gasReservoirChlorine[x, y] = 0f;
                    gasReservoirNaturalGas[x, y] = 0f;
                    gasReservoirGerms[x, y] = 0f;
                    looseResourceKind[x, y] = LooseResourceKind.None;
                    looseResourceAmount[x, y] = 0f;
                    cropTendedSeconds[x, y] = 0f;
                    cropStress[x, y] = 0f;
                    bool inStartRoom = x >= 27 && x <= 53 && y >= 18 && y <= 27;
                    bool inUpperShaft = x >= 39 && x <= 41 && y >= 19 && y <= 34;

                    if (inStartRoom || inUpperShaft || y > 36)
                    {
                        cells[x, y] = CellKind.Empty;
                    }
                    else if (y <= 2)
                    {
                        cells[x, y] = CellKind.Rock;
                    }
                    else
                    {
                        float caveNoise = Mathf.PerlinNoise(x * 0.115f + 13.2f, y * 0.14f + 2.7f);
                        bool solid = y < 12 ? caveNoise > 0.34f : caveNoise > 0.47f;
                        if (!solid)
                        {
                            cells[x, y] = CellKind.Empty;
                        }
                        else
                        {
                            float oreNoise = Mathf.PerlinNoise(x * 0.27f + 7.1f, y * 0.21f + 19.4f);
                            float algaeNoise = Mathf.PerlinNoise(x * 0.31f + 41.6f, y * 0.24f + 5.3f);
                            float coalNoise = Mathf.PerlinNoise(x * 0.24f + 88.4f, y * 0.19f + 33.8f);
                            if (oreNoise > 0.71f)
                            {
                                cells[x, y] = CellKind.MetalOre;
                            }
                            else if (coalNoise > 0.73f && y < 24)
                            {
                                cells[x, y] = CellKind.Coal;
                            }
                            else if (algaeNoise > 0.74f && y > 14)
                            {
                                cells[x, y] = CellKind.Slime;
                                germs[x, y] = 0.85f;
                            }
                            else if (algaeNoise > 0.68f && y > 7)
                            {
                                cells[x, y] = CellKind.Algae;
                            }
                            else if (y < 10 && caveNoise > 0.62f)
                            {
                                cells[x, y] = CellKind.Sand;
                            }
                            else
                            {
                                cells[x, y] = y < 8 ? CellKind.Rock : CellKind.Dirt;
                            }
                        }
                    }
                }
            }

            SeedWaterPool(7, 4, 14, 7, 85f);
            SeedWaterPool(61, 5, 71, 9, 100f);
            SeedWaterPool(18, 11, 23, 13, 60f);
            SeedCoalPockets();
            SeedSlimePockets();
            SeedIcePockets();

            for (int x = 27; x <= 53; x++)
            {
                cells[x, 18] = CellKind.Floor;
            }

            for (int y = 18; y <= 34; y++)
            {
                cells[40, y] = CellKind.Ladder;
            }

            cells[30, 19] = CellKind.ManualGenerator;
            cells[32, 19] = CellKind.Battery;
            cells[35, 19] = CellKind.OxygenDiffuser;
            cells[38, 19] = CellKind.Planter;
            cells[43, 19] = CellKind.Bed;
            SeedStarterOuthouse();
            SeedStarterPrintingPod();
            SeedStarterPowerWire();
            maxPower += 60f;
            plantGrowth[38, 19] = 0.35f;

            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (IsPassable(x, y))
                    {
                        bool inStartRoom = x >= 27 && x <= 53 && y >= 18 && y <= 27;
                        oxygen[x, y] = inStartRoom ? 1.18f : Mathf.Lerp(0.1f, 0.78f, Mathf.InverseLerp(5f, 30f, y));
                        carbonDioxide[x, y] = y < 12 ? 0.42f : 0.05f;
                        pollutedOxygen[x, y] = 0f;
                        hydrogen[x, y] = 0f;
                        steam[x, y] = 0f;
                        chlorine[x, y] = 0f;
                        naturalGas[x, y] = 0f;
                        germs[x, y] = 0f;
                    }
                    else
                    {
                        oxygen[x, y] = 0f;
                        carbonDioxide[x, y] = 0f;
                        pollutedOxygen[x, y] = 0f;
                        hydrogen[x, y] = 0f;
                        steam[x, y] = 0f;
                        chlorine[x, y] = 0f;
                        naturalGas[x, y] = 0f;
                        if (cells[x, y] != CellKind.Slime)
                        {
                            germs[x, y] = 0f;
                        }
                    }

                    temperature[x, y] = InitialTemperature(x, y, cells[x, y]);
                    equipmentCondition[x, y] = DefaultEquipmentCondition(cells[x, y]);
                }
            }

            SeedHydrogenPockets();
            SeedChlorinePockets();
            SeedNaturalGasPockets();
            SeedNaturalVents();
            SeedHatches();
            terrainDirty = true;
            gasDirty = true;
        }

        private void SeedWaterPool(int minX, int minY, int maxX, int maxY, float mass)
        {
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (!IsInside(x, y))
                    {
                        continue;
                    }

                    cells[x, y] = CellKind.Water;
                    waterMass[x, y] = mass;
                    equipmentCondition[x, y] = 0f;
                    looseResourceKind[x, y] = LooseResourceKind.None;
                    looseResourceAmount[x, y] = 0f;
                    oxygen[x, y] = 0f;
                    carbonDioxide[x, y] = 0f;
                    pollutedOxygen[x, y] = 0f;
                    hydrogen[x, y] = 0f;
                    steam[x, y] = 0f;
                    chlorine[x, y] = 0f;
                    naturalGas[x, y] = 0f;
                    germs[x, y] = 0f;
                    powerWire[x, y] = false;
                    poweredWire[x, y] = false;
                }
            }
        }

        private void SeedHydrogenPockets()
        {
            SeedHydrogenPocket(5, 37, 24, 44, 0.55f);
            SeedHydrogenPocket(55, 36, 76, 44, 0.62f);
            SeedHydrogenPocket(30, 33, 48, 41, 0.42f);
        }

        private void SeedChlorinePockets()
        {
            SeedChlorinePocket(6, 28, 18, 35, 0.50f);
            SeedChlorinePocket(54, 25, 70, 32, 0.58f);
        }

        private void SeedNaturalGasPockets()
        {
            SeedNaturalGasPocket(4, 15, 17, 22, 0.48f);
            SeedNaturalGasPocket(58, 13, 75, 20, 0.54f);
        }

        private void SeedNaturalVents()
        {
            SeedNaturalVent(CellKind.SteamVent, new Vector2Int(13, 33), 7);
            SeedNaturalVent(CellKind.HydrogenVent, new Vector2Int(66, 34), 7);
            SeedNaturalVent(CellKind.NaturalGasVent, new Vector2Int(9, 18), 7);
        }

        private void SeedNaturalVent(CellKind ventKind, Vector2Int preferred, int searchRadius)
        {
            if (CountCells(ventKind) > 0)
            {
                return;
            }

            Vector2Int placement;
            if (!TryFindNaturalVentPlacement(preferred, searchRadius, out placement))
            {
                return;
            }

            CarveNaturalVentPocket(placement);
            cells[placement.x, placement.y] = ventKind;
            waterMass[placement.x, placement.y] = 0f;
            oxygen[placement.x, placement.y] = 0f;
            carbonDioxide[placement.x, placement.y] = 0f;
            pollutedOxygen[placement.x, placement.y] = 0f;
            hydrogen[placement.x, placement.y] = 0f;
            steam[placement.x, placement.y] = 0f;
            chlorine[placement.x, placement.y] = 0f;
            naturalGas[placement.x, placement.y] = 0f;
            germs[placement.x, placement.y] = 0f;
            temperature[placement.x, placement.y] = InitialTemperature(placement.x, placement.y, ventKind);
            equipmentCondition[placement.x, placement.y] = 0f;
            looseResourceKind[placement.x, placement.y] = LooseResourceKind.None;
            looseResourceAmount[placement.x, placement.y] = 0f;
            powerWire[placement.x, placement.y] = false;
            poweredWire[placement.x, placement.y] = false;
            automationWire[placement.x, placement.y] = false;
            liquidPipe[placement.x, placement.y] = false;
            pipeWater[placement.x, placement.y] = 0f;
            gasPipe[placement.x, placement.y] = false;
            gasPipeOxygen[placement.x, placement.y] = 0f;
            gasPipeCarbonDioxide[placement.x, placement.y] = 0f;
            gasPipePollutedOxygen[placement.x, placement.y] = 0f;
            gasPipeHydrogen[placement.x, placement.y] = 0f;
            gasPipeChlorine[placement.x, placement.y] = 0f;
            gasPipeNaturalGas[placement.x, placement.y] = 0f;
            gasPipeGerms[placement.x, placement.y] = 0f;
        }

        private bool TryFindNaturalVentPlacement(Vector2Int preferred, int searchRadius, out Vector2Int placement)
        {
            placement = new Vector2Int(-1, -1);
            for (int radius = 0; radius <= searchRadius; radius++)
            {
                for (int y = preferred.y - radius; y <= preferred.y + radius; y++)
                {
                    for (int x = preferred.x - radius; x <= preferred.x + radius; x++)
                    {
                        if (Mathf.Abs(x - preferred.x) != radius && Mathf.Abs(y - preferred.y) != radius)
                        {
                            continue;
                        }

                        if (CanPlaceNaturalVentAt(x, y))
                        {
                            placement = new Vector2Int(x, y);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool CanPlaceNaturalVentAt(int x, int y)
        {
            if (!IsInside(x, y) || x < 3 || x >= WorldWidth - 3 || y < 4 || y >= WorldHeight - 3)
            {
                return false;
            }

            bool inStartRoom = x >= 27 && x <= 53 && y >= 18 && y <= 27;
            bool inUpperShaft = x >= 39 && x <= 41 && y >= 19 && y <= 34;
            if (inStartRoom || inUpperShaft)
            {
                return false;
            }

            CellKind kind = cells[x, y];
            bool replaceable = kind == CellKind.Empty || IsNaturalSolid(kind) || kind == CellKind.Water;
            return replaceable &&
                !powerWire[x, y] &&
                !automationWire[x, y] &&
                !liquidPipe[x, y] &&
                !gasPipe[x, y] &&
                looseResourceKind[x, y] == LooseResourceKind.None;
        }

        private void CarveNaturalVentPocket(Vector2Int vent)
        {
            for (int dy = -1; dy <= 2; dy++)
            {
                for (int dx = -2; dx <= 2; dx++)
                {
                    int x = vent.x + dx;
                    int y = vent.y + dy;
                    if (!IsInside(x, y) || (dx == 0 && dy == 0))
                    {
                        continue;
                    }

                    if (cells[x, y] != CellKind.Empty && !IsNaturalSolid(cells[x, y]) && cells[x, y] != CellKind.Water)
                    {
                        continue;
                    }

                    cells[x, y] = CellKind.Empty;
                    waterMass[x, y] = 0f;
                    oxygen[x, y] = Mathf.Max(oxygen[x, y], 0.22f);
                    carbonDioxide[x, y] = Mathf.Max(carbonDioxide[x, y], 0.08f);
                    pollutedOxygen[x, y] = 0f;
                    steam[x, y] = 0f;
                    germs[x, y] = 0f;
                    equipmentCondition[x, y] = 0f;
                    looseResourceKind[x, y] = LooseResourceKind.None;
                    looseResourceAmount[x, y] = 0f;
                    temperature[x, y] = Mathf.Max(temperature[x, y], 18f);
                }
            }
        }

        private void SeedHatches()
        {
            if (hatches.Count > 0)
            {
                return;
            }

            TrySeedHatch("Pebble", new Vector2Int(18, 15), 8);
            TrySeedHatch("Nib", new Vector2Int(58, 15), 8);
            TrySeedHatch("Grub", new Vector2Int(11, 28), 8);
        }

        private bool TrySeedHatch(string hatchName, Vector2Int preferred, int searchRadius)
        {
            Vector2Int cell;
            if (!TryFindHatchSpawnCell(preferred, searchRadius, out cell))
            {
                return false;
            }

            SpawnHatch(hatchName, cell, 35f, 0f, HatchMoveIntervalSeconds, HatchEatIntervalSeconds * 0.5f, 0f);
            return true;
        }

        private bool TryFindHatchSpawnCell(Vector2Int preferred, int searchRadius, out Vector2Int spawnCell)
        {
            spawnCell = new Vector2Int(-1, -1);
            for (int radius = 0; radius <= searchRadius; radius++)
            {
                for (int y = preferred.y - radius; y <= preferred.y + radius; y++)
                {
                    for (int x = preferred.x - radius; x <= preferred.x + radius; x++)
                    {
                        if (Mathf.Abs(x - preferred.x) != radius && Mathf.Abs(y - preferred.y) != radius)
                        {
                            continue;
                        }

                        if (CanSpawnHatchAt(x, y))
                        {
                            spawnCell = new Vector2Int(x, y);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool CanSpawnHatchAt(int x, int y)
        {
            if (!IsInside(x, y) || !IsPassable(x, y) || HatchAt(new Vector2Int(x, y)) != null)
            {
                return false;
            }

            bool inStartRoom = x >= 27 && x <= 53 && y >= 18 && y <= 27;
            return !inStartRoom && waterMass[x, y] <= 0.05f && temperature[x, y] > -8f && temperature[x, y] < 48f;
        }

        private void SeedHydrogenPocket(int minX, int minY, int maxX, int maxY, float mass)
        {
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (!IsInside(x, y) || !IsPassable(x, y))
                    {
                        continue;
                    }

                    float shape = Mathf.PerlinNoise(x * 0.25f + 91.4f, y * 0.22f + 37.6f);
                    if (shape <= 0.34f)
                    {
                        continue;
                    }

                    hydrogen[x, y] = Mathf.Max(hydrogen[x, y], mass);
                    oxygen[x, y] *= 0.42f;
                    carbonDioxide[x, y] *= 0.18f;
                    pollutedOxygen[x, y] = 0f;
                    naturalGas[x, y] = 0f;
                    germs[x, y] = 0f;
                }
            }
        }

        private void SeedChlorinePocket(int minX, int minY, int maxX, int maxY, float mass)
        {
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (!IsInside(x, y) || !IsPassable(x, y))
                    {
                        continue;
                    }

                    float shape = Mathf.PerlinNoise(x * 0.19f + 24.8f, y * 0.27f + 66.1f);
                    if (shape <= 0.36f)
                    {
                        continue;
                    }

                    chlorine[x, y] = Mathf.Max(chlorine[x, y], mass);
                    oxygen[x, y] *= 0.30f;
                    carbonDioxide[x, y] *= 0.35f;
                    pollutedOxygen[x, y] = 0f;
                    hydrogen[x, y] = 0f;
                    steam[x, y] = 0f;
                    naturalGas[x, y] = 0f;
                    germs[x, y] = 0f;
                    temperature[x, y] = Mathf.Clamp(Mathf.Max(temperature[x, y], 22f), -30f, 120f);
                }
            }
        }

        private void SeedNaturalGasPocket(int minX, int minY, int maxX, int maxY, float mass)
        {
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (!IsInside(x, y) || !IsPassable(x, y))
                    {
                        continue;
                    }

                    float shape = Mathf.PerlinNoise(x * 0.23f + 58.2f, y * 0.18f + 11.7f);
                    if (shape <= 0.35f)
                    {
                        continue;
                    }

                    naturalGas[x, y] = Mathf.Max(naturalGas[x, y], mass);
                    oxygen[x, y] *= 0.30f;
                    carbonDioxide[x, y] *= 0.24f;
                    pollutedOxygen[x, y] = 0f;
                    hydrogen[x, y] = 0f;
                    steam[x, y] = 0f;
                    chlorine[x, y] = 0f;
                    germs[x, y] = 0f;
                    temperature[x, y] = Mathf.Clamp(Mathf.Max(temperature[x, y], 30f), -30f, 120f);
                }
            }
        }

        private void SeedIcePockets()
        {
            SeedIcePocket(4, 31, 14, 37);
            SeedIcePocket(63, 29, 74, 36);
        }

        private void SeedCoalPockets()
        {
            SeedCoalPocket(10, 8, 19, 13);
            SeedCoalPocket(55, 10, 67, 16);
            SeedCoalPocket(23, 5, 31, 10);
        }

        private void SeedStarterPowerWire()
        {
            for (int x = 30; x <= 43; x++)
            {
                if (IsInside(x, 19))
                {
                    powerWire[x, 19] = true;
                }
            }

            if (IsInside(35, 20))
            {
                powerWire[35, 20] = true;
            }
        }

        private void SeedStarterOuthouse()
        {
            Vector2Int[] candidates =
            {
                new Vector2Int(46, 19),
                new Vector2Int(47, 19),
                new Vector2Int(48, 19),
                new Vector2Int(49, 19),
                new Vector2Int(50, 19)
            };

            foreach (Vector2Int cell in candidates)
            {
                if (!IsInside(cell.x, cell.y) || cells[cell.x, cell.y] != CellKind.Empty)
                {
                    continue;
                }

                cells[cell.x, cell.y] = CellKind.Outhouse;
                plantGrowth[cell.x, cell.y] = 0f;
                cropStress[cell.x, cell.y] = 0f;
                waterMass[cell.x, cell.y] = 0f;
                powerWire[cell.x, cell.y] = false;
                poweredWire[cell.x, cell.y] = false;
                oxygen[cell.x, cell.y] = Mathf.Max(oxygen[cell.x, cell.y], 1.05f);
                carbonDioxide[cell.x, cell.y] = Mathf.Min(carbonDioxide[cell.x, cell.y], 0.1f);
            pollutedOxygen[cell.x, cell.y] = Mathf.Min(pollutedOxygen[cell.x, cell.y], 0.05f);
            hydrogen[cell.x, cell.y] = 0f;
            steam[cell.x, cell.y] = 0f;
            chlorine[cell.x, cell.y] = 0f;
            naturalGas[cell.x, cell.y] = 0f;
            germs[cell.x, cell.y] = Mathf.Min(germs[cell.x, cell.y], 0.05f);
                temperature[cell.x, cell.y] = InitialTemperature(cell.x, cell.y, CellKind.Outhouse);
                return;
            }
        }

        private void SeedStarterPrintingPod()
        {
            Vector2Int[] candidates =
            {
                new Vector2Int(50, 19),
                new Vector2Int(51, 19),
                new Vector2Int(52, 19),
                new Vector2Int(49, 19),
                new Vector2Int(48, 19)
            };

            foreach (Vector2Int cell in candidates)
            {
                if (!IsInside(cell.x, cell.y) || cells[cell.x, cell.y] != CellKind.Empty)
                {
                    continue;
                }

                cells[cell.x, cell.y] = CellKind.PrintingPod;
                plantGrowth[cell.x, cell.y] = 0f;
                cropStress[cell.x, cell.y] = 0f;
                waterMass[cell.x, cell.y] = 0f;
                oxygen[cell.x, cell.y] = Mathf.Max(oxygen[cell.x, cell.y], 1.05f);
                carbonDioxide[cell.x, cell.y] = Mathf.Min(carbonDioxide[cell.x, cell.y], 0.08f);
            pollutedOxygen[cell.x, cell.y] = Mathf.Min(pollutedOxygen[cell.x, cell.y], 0.02f);
            hydrogen[cell.x, cell.y] = 0f;
            steam[cell.x, cell.y] = 0f;
            chlorine[cell.x, cell.y] = 0f;
            naturalGas[cell.x, cell.y] = 0f;
            germs[cell.x, cell.y] = Mathf.Min(germs[cell.x, cell.y], 0.02f);
                temperature[cell.x, cell.y] = InitialTemperature(cell.x, cell.y, CellKind.PrintingPod);
                return;
            }
        }

        private void SeedIcePocket(int minX, int minY, int maxX, int maxY)
        {
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (!IsInside(x, y) || cells[x, y] == CellKind.Water)
                    {
                        continue;
                    }

                    float shape = Mathf.PerlinNoise(x * 0.26f + 12.4f, y * 0.32f + 53.1f);
                    if (shape > 0.34f)
                    {
                        cells[x, y] = CellKind.Ice;
                        oxygen[x, y] = 0f;
                        carbonDioxide[x, y] = 0f;
                        pollutedOxygen[x, y] = 0f;
                        hydrogen[x, y] = 0f;
                        steam[x, y] = 0f;
                        chlorine[x, y] = 0f;
                        naturalGas[x, y] = 0f;
                        germs[x, y] = 0f;
                        waterMass[x, y] = 0f;
                    }
                }
            }
        }

        private void SeedSlimePockets()
        {
            SeedSlimePocket(8, 23, 15, 28);
            SeedSlimePocket(58, 18, 66, 24);
            SeedSlimePocket(20, 30, 28, 35);
        }

        private void SeedSlimePocket(int minX, int minY, int maxX, int maxY)
        {
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (!IsInside(x, y) || cells[x, y] == CellKind.Water)
                    {
                        continue;
                    }

                    float shape = Mathf.PerlinNoise(x * 0.34f + 21.7f, y * 0.29f + 8.3f);
                    if (shape > 0.28f)
                    {
                        cells[x, y] = CellKind.Slime;
                        oxygen[x, y] = 0f;
                        carbonDioxide[x, y] = 0f;
                        pollutedOxygen[x, y] = 0f;
                        hydrogen[x, y] = 0f;
                        steam[x, y] = 0f;
                        chlorine[x, y] = 0f;
                        naturalGas[x, y] = 0f;
                        germs[x, y] = 0.9f;
                    }
                }
            }
        }

        private void SeedCoalPocket(int minX, int minY, int maxX, int maxY)
        {
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (!IsInside(x, y) || cells[x, y] == CellKind.Water || !IsNaturalSolid(cells[x, y]))
                    {
                        continue;
                    }

                    float shape = Mathf.PerlinNoise(x * 0.28f + 61.2f, y * 0.33f + 14.9f);
                    if (shape > 0.31f)
                    {
                        cells[x, y] = CellKind.Coal;
                        oxygen[x, y] = 0f;
                        carbonDioxide[x, y] = 0f;
                        pollutedOxygen[x, y] = 0f;
                        hydrogen[x, y] = 0f;
                        steam[x, y] = 0f;
                        chlorine[x, y] = 0f;
                        naturalGas[x, y] = 0f;
                        germs[x, y] = 0f;
                        waterMass[x, y] = 0f;
                    }
                }
            }
        }

        private float InitialTemperature(int x, int y, CellKind kind)
        {
            if (kind == CellKind.Ice)
            {
                return -6f;
            }

            if (kind == CellKind.Water)
            {
                return y < 8 ? 13f : 18f;
            }

            if (kind == CellKind.SteamVent)
            {
                return 86f;
            }

            if (kind == CellKind.HydrogenVent)
            {
                return 64f;
            }

            if (kind == CellKind.NaturalGasVent)
            {
                return 70f;
            }

            bool inStartRoom = x >= 27 && x <= 53 && y >= 18 && y <= 27;
            if (inStartRoom)
            {
                return 22f;
            }

            float variation = Mathf.PerlinNoise(x * 0.09f + 4.3f, y * 0.11f + 17.2f) * 5f - 2.5f;
            if (y <= 6)
            {
                return 45f + variation + (6 - y) * 2.2f;
            }

            if (y >= 30)
            {
                return 9f + variation - (y - 30) * 0.9f;
            }

            if (kind == CellKind.Slime)
            {
                return 31f + variation;
            }

            return 21f + variation;
        }

        private void CreateWorkers()
        {
            if (workers.Count > 0)
            {
                return;
            }

            SpawnWorker("Ari", new Vector2Int(34, 19), WorkerTint(0));
            SpawnWorker("Bomi", new Vector2Int(40, 20), WorkerTint(1));
            SpawnWorker("Chae", new Vector2Int(46, 19), WorkerTint(2));
        }

        private void EnsureWorkerRecords()
        {
            if (workers.Count > 0)
            {
                return;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (!child.name.StartsWith("Duplicant ", StringComparison.Ordinal))
                {
                    continue;
                }

                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if (spriteRenderer == null)
                {
                    continue;
                }

                Worker worker = new Worker
                {
                    Name = child.name.Substring("Duplicant ".Length),
                    Transform = child,
                    Renderer = spriteRenderer,
                    Cell = WorldToCell(child.position),
                    Calories = 2800f,
                    Health = 100f,
                    Morale = BaseWorkerMorale,
                    Fatigue = 25f,
                    Bladder = 20f,
                    Experience = 0f,
                    WorkSpeed = 1f,
                    MoveSpeed = 5f
                };

                if (!IsCharacterStandableCell(worker.Cell) && TryFindCharacterStandableCellNear(worker.Cell, 8, worker, out Vector2Int safeCell))
                {
                    worker.Cell = safeCell;
                    worker.Transform.position = CellCenter(safeCell);
                }

                workers.Add(worker);
            }

            if (workers.Count == 0)
            {
                CreateWorkers();
            }
        }

        private void EnsureWorldState()
        {
            bool hasNonEmptyCell = false;
            for (int y = 0; y < WorldHeight && !hasNonEmptyCell; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] != CellKind.Empty)
                    {
                        hasNonEmptyCell = true;
                        break;
                    }
                }
            }

            if (!hasNonEmptyCell)
            {
                maxPower = 100f;
                GenerateWorld();
                terrainDirty = true;
                gasDirty = true;
                overlayDirty = true;
                Log("World data restored.");
            }
        }

        private void StartNewRun()
        {
            ClearWorkers();
            jobs.Clear();
            rooms.Clear();
            pollutedWaterOffgasSources.Clear();
            dragCells.Clear();
            inspectedCell = null;
            isDraggingCommand = false;
            isPanningCamera = false;

            ResetRunState();
            GenerateWorld();
            CreateWorkers();

            SetMode(CommandMode.Inspect);
            SetOverlayMode(OverlayMode.Gas);
            RenderTerrain();
            RenderGas();
            RenderOverlay();
            UpdateColonyStatus(true);
            Log("New colony started.");
            UpdateHud();
        }

        private void ResetRunState()
        {
            lastLog = "Colony online.";
            objectiveText = "Build beds, oxygen, power, and food production.";
            alertText = "Stable.";
            unreachableJobCount = 0;
            dirt = 130f;
            metal = 80f;
            algae = 45f;
            coal = 0f;
            refinedMetal = 0f;
            water = 45f;
            pollutedWater = 0f;
            pollutedDirt = 0f;
            researchPoints = 0f;
            food = 3600f;
            foodFreshness = 0.82f;
            power = 35f;
            maxPower = 100f;
            elapsedTime = 0f;
            cycleTimer = 0f;
            gasTimer = 0f;
            thermalTimer = 0f;
            liquidTimer = 0f;
            sandTimer = 0f;
            maintenanceTimer = 0f;
            autosaveTimer = 0f;
            objectiveTimer = 0f;
            cycle = 1;
            paused = false;
            simulationSpeed = 1f;
            sleepStartCycleTime = DefaultSleepStartCycleTime;
            sleepEndCycleTime = DefaultSleepEndCycleTime;
            techAirSystems = false;
            techFoodPreparation = false;
            techPowerRegulation = false;
            colonyVictory = false;
            colonyVictoryAcknowledged = false;
            colonyFailed = false;

            milestoneBasicShelter = false;
            milestoneStableOxygen = false;
            milestoneFoodProduction = false;
            milestonePowerBuffer = false;
            milestoneCycleFive = false;
            milestoneResearchProgram = false;
            milestoneWaterSupply = false;
            milestoneFoodPreparation = false;
            milestoneThermalControl = false;
            milestonePowerGrid = false;
            milestoneSanitation = false;
            milestoneMoraleCare = false;
            milestonePressureControl = false;
            milestoneAirlockControl = false;
            milestoneFoodStorage = false;
            milestoneMaterialStorage = false;
            milestonePlumbing = false;
            milestoneVentilation = false;
            milestoneAdvancedAtmosphere = false;
            milestoneWaterRecycling = false;
            milestoneDining = false;
            milestoneSkilledLabor = false;
            milestoneDecorComfort = false;
            milestoneWasteProcessing = false;
            milestoneAutomation = false;
            milestoneFuelPower = false;
            milestoneMetalRefining = false;
            milestoneAtmoSuits = false;
            milestoneInsulation = false;
            milestoneRoomPlanning = false;
            milestoneReconfiguration = false;
            milestoneColonyExpansion = false;
            milestoneSpillCleanup = false;
            milestoneMaintenance = false;
            milestoneEmergencyResponse = false;
            milestoneResourceLogistics = false;
            milestoneHydrogenPower = false;
            milestoneHydrogenFiltering = false;
            milestoneReservoirBuffering = false;
            milestoneConduitAutomation = false;
            milestoneRenewableVents = false;
            milestoneRanching = false;
            milestonePowerLoadManagement = false;
            milestoneHygiene = false;
            milestoneCropTending = false;
            milestoneAutoSweeping = false;
            milestoneShippingLogistics = false;
            milestoneBottleEmptying = false;
            milestoneSignalSwitching = false;
            milestoneSteamPower = false;
            milestoneSolarPower = false;
            milestoneMeteorShielding = false;
            milestoneSpaceScanning = false;

            terrainDirty = true;
            gasDirty = true;
            overlayDirty = true;
            roomsDirty = true;
        }

        private void ClearWorkers()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                if (child.name.StartsWith("Duplicant ", StringComparison.Ordinal))
                {
                    DestroyRuntimeObject(child.gameObject);
                }
            }

            workers.Clear();
        }

        private void SpawnWorker(string workerName, Vector2Int cell, Color tint)
        {
            if (!IsCharacterStandableCell(cell) && TryFindCharacterStandableCellNear(cell, 8, null, out Vector2Int safeCell))
            {
                cell = safeCell;
            }

            GameObject workerObject = new GameObject("Duplicant " + workerName);
            workerObject.transform.SetParent(transform, false);
            workerObject.transform.position = CellCenter(cell);

            SpriteRenderer spriteRenderer = workerObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = workerSprite;
            spriteRenderer.color = tint;
            spriteRenderer.sortingOrder = 5;

            workers.Add(new Worker
            {
                Name = workerName,
                Transform = workerObject.transform,
                Renderer = spriteRenderer,
                Cell = cell,
                Morale = BaseWorkerMorale,
                Bladder = 20f,
                Experience = 0f,
                WorkSpeed = 0.95f + workers.Count * 0.1f,
                MoveSpeed = 4.6f + workers.Count * 0.35f
            });
        }

        private HatchCritter SpawnHatch(string hatchName, Vector2Int cell, float happiness, float groomedSeconds, float moveTimer, float eatTimer, float coalProduced)
        {
            string resolvedName = string.IsNullOrEmpty(hatchName) ? "Hatch " + (hatches.Count + 1) : hatchName;
            GameObject hatchObject = new GameObject("Hatch " + resolvedName);
            hatchObject.transform.SetParent(transform, false);
            hatchObject.transform.position = CellCenter(cell);

            SpriteRenderer spriteRenderer = hatchObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = hatchSprite;
            spriteRenderer.color = new Color(0.92f, 0.74f, 0.48f, 1f);
            spriteRenderer.sortingOrder = 4;

            HatchCritter hatch = new HatchCritter
            {
                Name = resolvedName,
                Transform = hatchObject.transform,
                Renderer = spriteRenderer,
                Cell = cell,
                TargetCell = cell,
                Happiness = Mathf.Clamp(happiness <= 0f ? 35f : happiness, 0f, 100f),
                GroomedSeconds = Mathf.Max(0f, groomedSeconds),
                MoveTimer = Mathf.Max(0.1f, moveTimer),
                EatTimer = Mathf.Max(0.1f, eatTimer),
                CoalProduced = Mathf.Max(0f, coalProduced)
            };
            hatches.Add(hatch);
            return hatch;
        }

        private void ClearHatches()
        {
            for (int i = hatches.Count - 1; i >= 0; i--)
            {
                if (hatches[i].Transform != null)
                {
                    DestroyRuntimeObject(hatches[i].Transform.gameObject);
                }
            }

            hatches.Clear();
        }

        private void EnsureHatchTransforms()
        {
            for (int i = hatches.Count - 1; i >= 0; i--)
            {
                HatchCritter hatch = hatches[i];
                if (hatch.Transform != null)
                {
                    continue;
                }

                SpawnHatch(hatch.Name, hatch.Cell, hatch.Happiness, hatch.GroomedSeconds, hatch.MoveTimer, hatch.EatTimer, hatch.CoalProduced);
                hatches.RemoveAt(i);
            }
        }

        private Color WorkerTint(int index)
        {
            switch (index % 8)
            {
                case 0:
                    return new Color(0.35f, 0.86f, 1f);
                case 1:
                    return new Color(0.7f, 0.95f, 0.45f);
                case 2:
                    return new Color(1f, 0.78f, 0.34f);
                case 3:
                    return new Color(0.95f, 0.50f, 0.68f);
                case 4:
                    return new Color(0.58f, 0.72f, 1f);
                case 5:
                    return new Color(0.72f, 0.58f, 0.95f);
                case 6:
                    return new Color(0.62f, 0.96f, 0.78f);
                default:
                    return new Color(1f, 0.62f, 0.42f);
            }
        }

        private void BuildHud()
        {
            EnsureEventSystem();

            uiFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (uiFont == null)
            {
                uiFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
            }

            VisualTreeAsset hudTree = Resources.Load<VisualTreeAsset>("UI/ProjectONHud");
            if (hudTree == null)
            {
                Debug.LogError("ProjectON HUD UXML not found at Resources/UI/ProjectONHud.uxml.");
                return;
            }

            GameObject uiObject = new GameObject("ProjectON HUD");
            uiPanelSettings = ScriptableObject.CreateInstance<PanelSettings>();
            uiPanelSettings.name = "ProjectON Runtime Panel Settings";
            uiPanelSettings.scaleMode = PanelScaleMode.ScaleWithScreenSize;
            uiPanelSettings.referenceResolution = new Vector2Int(1920, 1080);
            uiPanelSettings.screenMatchMode = PanelScreenMatchMode.MatchWidthOrHeight;
            uiPanelSettings.match = 0.5f;
            uiPanelSettings.sortingOrder = 100;
            uiPanelSettings.clearColor = false;
            ThemeStyleSheet runtimeTheme = Resources.Load<ThemeStyleSheet>("UnityDefaultRuntimeTheme");
            if (runtimeTheme != null)
            {
                uiPanelSettings.themeStyleSheet = runtimeTheme;
            }

            uiDocument = uiObject.AddComponent<UIDocument>();
            uiDocument.panelSettings = uiPanelSettings;
            uiDocument.visualTreeAsset = hudTree;
            uiDocument.rootVisualElement.pickingMode = PickingMode.Ignore;

            uiRoot = uiDocument.rootVisualElement.Q<VisualElement>("ProjectONHudRoot");
            if (uiRoot == null)
            {
                Debug.LogError("ProjectON HUD UXML is missing ProjectONHudRoot.");
                return;
            }

            uiRoot.pickingMode = PickingMode.Ignore;
            ApplyRuntimeFont(uiRoot);

            statsText = RequireElement<Label>("Stats");
            modeText = RequireElement<Label>("Mode");
            scenarioText = RequireElement<Label>("ScenarioText");
            jobQueueText = RequireElement<Label>("JobQueueText");
            overlayLegendTitleText = RequireElement<Label>("OverlayLegendTitleText");
            overlayLegendRows = RequireElement<VisualElement>("OverlayLegendRows");
            infoText = RequireElement<Label>("InspectText");
            logText = RequireElement<Label>("LogText");

            CreateToolbar(RequireElement<VisualElement>("CommandToolbar"));
            CreateOverlayToolbar(RequireElement<VisualElement>("OverlayToolbar"));

            ConfigureButton(RequireElement<Button>("ScheduleStartDownButton"), "Start-\nF6", () => AdjustSleepStart(-ScheduleStep));
            ConfigureButton(RequireElement<Button>("ScheduleStartUpButton"), "Start+\nF7", () => AdjustSleepStart(ScheduleStep));
            ConfigureButton(RequireElement<Button>("ScheduleWakeDownButton"), "Wake-\nF8", () => AdjustSleepEnd(-ScheduleStep));
            ConfigureButton(RequireElement<Button>("ScheduleWakeUpButton"), "Wake+\nF10", () => AdjustSleepEnd(ScheduleStep));
            ConfigureButton(RequireElement<Button>("SaveButton"), "Save\nF5", () => SaveGame(false));
            ConfigureButton(RequireElement<Button>("LoadButton"), "Load\nF9", () => LoadGame(false));
            ConfigureButton(RequireElement<Button>("PauseButton"), "Pause\nSpace", TogglePause);
            ConfigureButton(RequireElement<Button>("SpeedButton"), "Speed\n-/+", CycleSpeed);

            languageButton = RequireElement<Button>("LanguageButton");
            ConfigureButton(languageButton, LanguageButtonLabel(), ToggleLanguage);

            priorityDownButton = RequireElement<Button>("PriorityDownButton");
            priorityUpButton = RequireElement<Button>("PriorityUpButton");
            cancelSelectedJobButton = RequireElement<Button>("CancelSelectedButton");
            signalSwitchButton = RequireElement<Button>("SignalSwitchButton");
            airlockToggleButton = RequireElement<Button>("AirlockToggleButton");
            endStatePanel = RequireElement<VisualElement>("EndStatePanel");
            endStateTitleText = RequireElement<Label>("EndStateTitleText");
            endStateBodyText = RequireElement<Label>("EndStateBodyText");
            endStateLoadButton = RequireElement<Button>("EndStateLoadButton");
            endStateNewRunButton = RequireElement<Button>("EndStateNewRunButton");
            endStateContinueButton = RequireElement<Button>("EndStateContinueButton");

            ConfigureButton(priorityDownButton, "Pri -\n[", () => AdjustInspectedJobPriority(-1));
            ConfigureButton(priorityUpButton, "Pri +\n]", () => AdjustInspectedJobPriority(1));
            ConfigureButton(cancelSelectedJobButton, "Cancel\nDel", CancelInspectedJob);
            ConfigureButton(signalSwitchButton, "Switch\nOFF", ToggleInspectedSignalSwitch);
            ConfigureButton(airlockToggleButton, "Door\nOPEN", ToggleInspectedAirlock);
            ConfigureButton(endStateLoadButton, "Load Save", () => LoadGame(false));
            ConfigureButton(endStateNewRunButton, "New Run", StartNewRun);
            ConfigureButton(endStateContinueButton, "Continue", ContinueFreeplay);
            SetInspectControlsVisible(false);
            SetVisible(endStatePanel, false);

            SetMode(CommandMode.Inspect);
            SetOverlayMode(OverlayMode.Gas);
        }

        private void EnsureEventSystem()
        {
            EventSystem eventSystem = FindFirstObjectByType<EventSystem>();
            if (eventSystem == null)
            {
                GameObject eventObject = new GameObject("EventSystem");
                eventSystem = eventObject.AddComponent<EventSystem>();
            }

            StandaloneInputModule legacyInputModule = eventSystem.GetComponent<StandaloneInputModule>();
            if (legacyInputModule != null)
            {
                Destroy(legacyInputModule);
            }

            if (eventSystem.GetComponent<InputSystemUIInputModule>() == null)
            {
                eventSystem.gameObject.AddComponent<InputSystemUIInputModule>();
            }
        }

        private void CreateToolbar(VisualElement parent)
        {
            CommandMode[] modes =
            {
                CommandMode.Inspect,
                CommandMode.Dig,
                CommandMode.Ladder,
                CommandMode.Floor,
                CommandMode.OxygenDiffuser,
                CommandMode.ManualGenerator,
                CommandMode.Battery,
                CommandMode.SmartBattery,
                CommandMode.PowerTransformer,
                CommandMode.CoalGenerator,
                CommandMode.HydrogenGenerator,
                CommandMode.NaturalGasGenerator,
                CommandMode.SteamTurbine,
                CommandMode.SolarPanel,
                CommandMode.BunkerDoor,
                CommandMode.SpaceScanner,
                CommandMode.HydrogenFilter,
                CommandMode.RockCrusher,
                CommandMode.AtmoSuitDock,
                CommandMode.AtmoSuitCheckpoint,
                CommandMode.InsulatedTile,
                CommandMode.Deconstruct,
                CommandMode.Mop,
                CommandMode.Repair,
                CommandMode.Sweep,
                CommandMode.PrintingPod,
                CommandMode.Bed,
                CommandMode.Planter,
                CommandMode.FarmStation,
                CommandMode.WaterPump,
                CommandMode.BottleEmptier,
                CommandMode.ResearchStation,
                CommandMode.MicrobeMusher,
                CommandMode.AirDeodorizer,
                CommandMode.MedicalCot,
                CommandMode.SpaceHeater,
                CommandMode.ThermoRegulator,
                CommandMode.PowerWire,
                CommandMode.AutomationWire,
                CommandMode.Outhouse,
                CommandMode.WashBasin,
                CommandMode.Compost,
                CommandMode.MassageTable,
                CommandMode.ManualAirlock,
                CommandMode.Refrigerator,
                CommandMode.StorageBin,
                CommandMode.AutoSweeper,
                CommandMode.ShippingRail,
                CommandMode.ConveyorLoader,
                CommandMode.ConveyorChute,
                CommandMode.SignalSwitch,
                CommandMode.LiquidPipe,
                CommandMode.LiquidPipeSensor,
                CommandMode.LiquidShutoff,
                CommandMode.LiquidReservoir,
                CommandMode.LiquidVent,
                CommandMode.GasPump,
                CommandMode.GasPipe,
                CommandMode.GasPipeSensor,
                CommandMode.GasShutoff,
                CommandMode.GasReservoir,
                CommandMode.GasVent,
                CommandMode.RanchingStation,
                CommandMode.Electrolyzer,
                CommandMode.CarbonSkimmer,
                CommandMode.WaterSieve,
                CommandMode.MessTable,
                CommandMode.DecorPlant,
                CommandMode.Cancel
            };

            const int columns = 30;
            const float leftMargin = 18f;
            const float topMargin = 8f;
            const float stepX = 42f;
            const float stepY = 40f;
            const float buttonWidth = 38f;
            const float buttonHeight = 36f;

            for (int i = 0; i < modes.Length; i++)
            {
                CommandMode mode = modes[i];
                VisualElement slot = CreateSlot("Slot " + mode, parent);
                int column = i % columns;
                int row = i / columns;
                SetFixedSlot(slot, leftMargin + column * stepX, topMargin + row * stepY, buttonWidth, buttonHeight);

                Button button = CreateButton(ModeButtonLabel(mode), slot, () => SetMode(mode));
                button.AddToClassList("command-mode-button");
                modeButtons[mode] = button;
            }
        }

        private void CreateOverlayToolbar(VisualElement parent)
        {
            OverlayMode[] modes =
            {
                OverlayMode.Gas,
                OverlayMode.Temperature,
                OverlayMode.Power,
                OverlayMode.Germs,
                OverlayMode.Decor,
                OverlayMode.Rooms,
                OverlayMode.Plumbing,
                OverlayMode.Ventilation,
                OverlayMode.Logistics
            };

            for (int i = 0; i < modes.Length; i++)
            {
                OverlayMode mode = modes[i];
                VisualElement slot = CreateSlot("Overlay Slot " + mode, parent);
                float xMin = 8f + i * 68f;
                SetVerticalSlot(slot, xMin, 5f, 60f, 5f);

                Button button = CreateButton(OverlayButtonLabel(mode), slot, () => SetOverlayMode(mode));
                overlayButtons[mode] = button;
            }
        }

        private VisualElement CreateSlot(string slotName, VisualElement parent)
        {
            VisualElement slot = new VisualElement { name = slotName, pickingMode = PickingMode.Ignore };
            slot.AddToClassList("toolbar-slot");
            parent.Add(slot);
            return slot;
        }

        private Button CreateButton(string label, VisualElement parent, Action onClick)
        {
            Button button = new Button(onClick)
            {
                name = "Button " + label.Replace("\n", " "),
                text = Localize(label)
            };

            button.AddToClassList("projecton-button");
            button.AddToClassList("fill-button");
            ApplyRuntimeFont(button);
            localizedStaticTexts[button] = label;
            parent.Add(button);
            return button;
        }

        private void ConfigureButton(Button button, string label, Action onClick)
        {
            if (button == null)
            {
                return;
            }

            button.clicked += onClick;
            button.text = Localize(label);
            ApplyRuntimeFont(button);
            localizedStaticTexts[button] = label;
        }

        private T RequireElement<T>(string elementName) where T : VisualElement
        {
            T element = uiRoot == null ? null : uiRoot.Q<T>(elementName);
            if (element == null)
            {
                Debug.LogError("ProjectON HUD UXML is missing element: " + elementName);
            }

            return element;
        }

        private void ApplyRuntimeFont(VisualElement element)
        {
            if (element != null && uiFont != null)
            {
                element.style.unityFontDefinition = new StyleFontDefinition(uiFont);
            }
        }

        private string Localize(string text)
        {
            return ProjectONStringTable.LocalizeBlock(text, currentLanguage);
        }

        private string LanguageButtonLabel()
        {
            return currentLanguage == ProjectONLanguage.Korean ? "Language\nKorean" : "Language\nEnglish";
        }

        private void ToggleLanguage()
        {
            currentLanguage = currentLanguage == ProjectONLanguage.Korean ? ProjectONLanguage.English : ProjectONLanguage.Korean;
            Log(currentLanguage == ProjectONLanguage.Korean ? "Language changed to Korean." : "Language changed to English.");
            RefreshLocalizedStaticTexts();
            RefreshModeButtonLabels();
            RefreshLanguageButtonLabel();
            renderedLegendOverlayMode = (OverlayMode)(-1);
            renderedJobQueueLanguage = (ProjectONLanguage)(-1);
            nextJobQueueRefreshTime = -1f;
            overlayDirty = true;
            UpdateHud();
        }

        private void RefreshLocalizedStaticTexts()
        {
            foreach (KeyValuePair<TextElement, string> pair in localizedStaticTexts)
            {
                if (pair.Key != null)
                {
                    pair.Key.text = Localize(pair.Value);
                }
            }
        }

        private void RefreshLanguageButtonLabel()
        {
            if (languageButton == null)
            {
                return;
            }

            languageButton.text = Localize(LanguageButtonLabel());
        }

        private static void SetAbsolute(VisualElement element)
        {
            element.style.position = Position.Absolute;
        }

        private static void SetVerticalSlot(VisualElement element, float left, float top, float width, float bottom)
        {
            SetAbsolute(element);
            element.style.left = left;
            element.style.top = top;
            element.style.width = width;
            element.style.bottom = bottom;
        }

        private static void SetFixedSlot(VisualElement element, float left, float top, float width, float height)
        {
            SetAbsolute(element);
            element.style.left = left;
            element.style.top = top;
            element.style.width = width;
            element.style.height = height;
        }

        private static void SetVisible(VisualElement element, bool visible)
        {
            if (element != null)
            {
                element.EnableInClassList("hidden", !visible);
            }
        }

        private void HandleKeyboardShortcuts()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return;
            }

            if (keyboard.digit1Key.wasPressedThisFrame) SetMode(CommandMode.Inspect);
            if (keyboard.digit2Key.wasPressedThisFrame) SetMode(CommandMode.Dig);
            if (keyboard.digit3Key.wasPressedThisFrame) SetMode(CommandMode.Ladder);
            if (keyboard.digit4Key.wasPressedThisFrame) SetMode(CommandMode.Floor);
            if (keyboard.digit5Key.wasPressedThisFrame) SetMode(CommandMode.OxygenDiffuser);
            if (keyboard.digit6Key.wasPressedThisFrame) SetMode(CommandMode.ManualGenerator);
            if (keyboard.digit7Key.wasPressedThisFrame) SetMode(CommandMode.Battery);
            if (keyboard.digit8Key.wasPressedThisFrame) SetMode(CommandMode.Bed);
            if (keyboard.digit9Key.wasPressedThisFrame) SetMode(CommandMode.Planter);
            if (keyboard.digit0Key.wasPressedThisFrame) SetMode(CommandMode.Cancel);
            if (keyboard.dKey.wasPressedThisFrame) SetMode(CommandMode.Deconstruct);
            if (keyboard.sKey.wasPressedThisFrame) SetMode(CommandMode.Mop);
            if (keyboard.aKey.wasPressedThisFrame) SetMode(CommandMode.Repair);
            if (keyboard.periodKey.wasPressedThisFrame) SetMode(CommandMode.Sweep);
            if (keyboard.pKey.wasPressedThisFrame) SetMode(CommandMode.WaterPump);
            if (keyboard.rKey.wasPressedThisFrame) SetMode(CommandMode.ResearchStation);
            if (keyboard.mKey.wasPressedThisFrame) SetMode(CommandMode.MicrobeMusher);
            if (keyboard.oKey.wasPressedThisFrame) SetMode(CommandMode.AirDeodorizer);
            if (keyboard.hKey.wasPressedThisFrame) SetMode(CommandMode.MedicalCot);
            if (keyboard.tKey.wasPressedThisFrame) SetMode(CommandMode.SpaceHeater);
            if (keyboard.yKey.wasPressedThisFrame) SetMode(CommandMode.ThermoRegulator);
            if (keyboard.wKey.wasPressedThisFrame) SetMode(CommandMode.PowerWire);
            if (keyboard.uKey.wasPressedThisFrame) SetMode(CommandMode.Outhouse);
            if (keyboard.gKey.wasPressedThisFrame) SetMode(CommandMode.MassageTable);
            if (keyboard.qKey.wasPressedThisFrame) SetMode(CommandMode.ManualAirlock);
            if (keyboard.iKey.wasPressedThisFrame) SetMode(CommandMode.Refrigerator);
            if (keyboard.bKey.wasPressedThisFrame) SetMode(CommandMode.StorageBin);
            if (keyboard.lKey.wasPressedThisFrame) SetMode(CommandMode.LiquidPipe);
            if (keyboard.vKey.wasPressedThisFrame) SetMode(CommandMode.LiquidVent);
            if (keyboard.zKey.wasPressedThisFrame) SetMode(CommandMode.GasPump);
            if (keyboard.xKey.wasPressedThisFrame) SetMode(CommandMode.GasPipe);
            if (keyboard.cKey.wasPressedThisFrame) SetMode(CommandMode.GasVent);
            if (keyboard.eKey.wasPressedThisFrame) SetMode(CommandMode.Electrolyzer);
            if (keyboard.kKey.wasPressedThisFrame) SetMode(CommandMode.CarbonSkimmer);
            if (keyboard.jKey.wasPressedThisFrame) SetMode(CommandMode.WaterSieve);
            if (keyboard.nKey.wasPressedThisFrame) SetMode(CommandMode.MessTable);
            if (keyboard.fKey.wasPressedThisFrame) SetMode(CommandMode.DecorPlant);
            if (keyboard.f1Key.wasPressedThisFrame) SetOverlayMode(OverlayMode.Gas);
            if (keyboard.f2Key.wasPressedThisFrame) SetOverlayMode(OverlayMode.Temperature);
            if (keyboard.f3Key.wasPressedThisFrame) SetOverlayMode(OverlayMode.Power);
            if (keyboard.f4Key.wasPressedThisFrame) SetOverlayMode(OverlayMode.Germs);
            if (keyboard.f11Key.wasPressedThisFrame) SetOverlayMode(OverlayMode.Plumbing);
            if (keyboard.f12Key.wasPressedThisFrame) SetOverlayMode(OverlayMode.Ventilation);
            if (keyboard.f6Key.wasPressedThisFrame) AdjustSleepStart(-ScheduleStep);
            if (keyboard.f7Key.wasPressedThisFrame) AdjustSleepStart(ScheduleStep);
            if (keyboard.f8Key.wasPressedThisFrame) AdjustSleepEnd(-ScheduleStep);
            if (keyboard.f10Key.wasPressedThisFrame) AdjustSleepEnd(ScheduleStep);
            if (keyboard.f5Key.wasPressedThisFrame) SaveGame(false);
            if (keyboard.f9Key.wasPressedThisFrame) LoadGame(false);
            if (keyboard.leftBracketKey.wasPressedThisFrame) AdjustInspectedJobPriority(-1);
            if (keyboard.rightBracketKey.wasPressedThisFrame) AdjustInspectedJobPriority(1);
            if (keyboard.deleteKey.wasPressedThisFrame) CancelInspectedJob();

            if (keyboard.spaceKey.wasPressedThisFrame)
            {
                TogglePause();
            }

            if (keyboard.equalsKey.wasPressedThisFrame || keyboard.numpadPlusKey.wasPressedThisFrame)
            {
                simulationSpeed = Mathf.Min(4f, simulationSpeed + 1f);
                paused = false;
                Log("Simulation speed x" + simulationSpeed.ToString("0"));
            }

            if (keyboard.minusKey.wasPressedThisFrame || keyboard.numpadMinusKey.wasPressedThisFrame)
            {
                simulationSpeed = Mathf.Max(1f, simulationSpeed - 1f);
                Log("Simulation speed x" + simulationSpeed.ToString("0"));
            }
        }

        private void HandleCameraControls()
        {
            if (gameCamera == null)
            {
                return;
            }

            Keyboard keyboard = Keyboard.current;
            Vector3 movement = Vector3.zero;
            if (keyboard != null)
            {
                if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) movement.x -= 1f;
                if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) movement.x += 1f;
                if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) movement.y -= 1f;
                if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) movement.y += 1f;
            }

            if (movement.sqrMagnitude > 0f)
            {
                gameCamera.transform.position += movement.normalized * (gameCamera.orthographicSize * 1.25f * Time.deltaTime);
            }

            Mouse mouse = Mouse.current;
            if (mouse == null)
            {
                ClampCamera();
                return;
            }

            float scroll = mouse.scroll.ReadValue().y;
            if (Mathf.Abs(scroll) > 0.1f)
            {
                gameCamera.orthographicSize = Mathf.Clamp(gameCamera.orthographicSize - scroll * 0.015f, 7f, 28f);
            }

            Vector2 screenPosition = mouse.position.ReadValue();
            if (mouse.middleButton.wasPressedThisFrame)
            {
                isPanningCamera = true;
                lastPanScreenPosition = screenPosition;
            }

            if (mouse.middleButton.wasReleasedThisFrame)
            {
                isPanningCamera = false;
            }

            if (isPanningCamera && mouse.middleButton.isPressed)
            {
                Vector2 delta = screenPosition - lastPanScreenPosition;
                float worldUnitsPerPixel = gameCamera.orthographicSize * 2f / Mathf.Max(1f, Screen.height);
                gameCamera.transform.position -= new Vector3(delta.x, delta.y, 0f) * worldUnitsPerPixel;
                lastPanScreenPosition = screenPosition;
            }

            ClampCamera();
        }

        private void ClampCamera()
        {
            Vector3 position = gameCamera.transform.position;
            position.x = Mathf.Clamp(position.x, 2f, WorldWidth - 2f);
            position.y = Mathf.Clamp(position.y, 2f, WorldHeight - 2f);
            position.z = -10f;
            gameCamera.transform.position = position;
        }

        private void HandlePointerCommands()
        {
            Mouse mouse = Mouse.current;
            if (mouse == null || gameCamera == null)
            {
                return;
            }

            Vector2 screenPosition = mouse.position.ReadValue();
            if (mouse.leftButton.wasPressedThisFrame)
            {
                isDraggingCommand = !IsScreenPositionBlocked(screenPosition);
                dragCells.Clear();
            }

            if (mouse.leftButton.wasReleasedThisFrame)
            {
                isDraggingCommand = false;
                dragCells.Clear();
            }

            if (isDraggingCommand && mouse.leftButton.isPressed && TryScreenToCell(screenPosition, out Vector2Int cell))
            {
                ApplyCommand(cell);
            }

            if (mouse.rightButton.wasPressedThisFrame && !IsScreenPositionBlocked(screenPosition) && TryScreenToCell(screenPosition, out Vector2Int cancelCell))
            {
                CancelJobsAt(cancelCell, true);
            }
        }

        private bool IsScreenPositionBlocked(Vector2 screenPosition)
        {
            if (uiRoot == null || uiRoot.panel == null)
            {
                return false;
            }

            Vector2 panelPosition = RuntimePanelUtils.ScreenToPanel(uiRoot.panel, screenPosition);
            VisualElement picked = uiRoot.panel.Pick(panelPosition);
            return picked != null && picked != uiRoot;
        }

        private bool TryScreenToCell(Vector2 screenPosition, out Vector2Int cell)
        {
            Vector3 world = gameCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, Mathf.Abs(gameCamera.transform.position.z)));
            cell = new Vector2Int(Mathf.FloorToInt(world.x), Mathf.FloorToInt(world.y));
            return IsInside(cell.x, cell.y);
        }

        private void ApplyCommand(Vector2Int cell)
        {
            int key = Key(cell.x, cell.y);
            if (!dragCells.Add(key))
            {
                return;
            }

            inspectedCell = cell;

            switch (currentMode)
            {
                case CommandMode.Inspect:
                    overlayDirty = true;
                    break;
                case CommandMode.Dig:
                    QueueDig(cell);
                    break;
                case CommandMode.Cancel:
                    CancelJobsAt(cell, false);
                    break;
                case CommandMode.Deconstruct:
                    QueueDeconstruct(cell);
                    break;
                case CommandMode.Mop:
                    QueueMop(cell);
                    break;
                case CommandMode.Repair:
                    QueueRepair(cell);
                    break;
                case CommandMode.Sweep:
                    QueueSweep(cell);
                    break;
                default:
                    QueueBuild(cell, currentMode);
                    break;
            }
        }

        private void SetMode(CommandMode mode)
        {
            currentMode = mode;
            foreach (KeyValuePair<CommandMode, Button> pair in modeButtons)
            {
                pair.Value.EnableInClassList("mode-selected", pair.Key == currentMode);
            }

            overlayDirty = true;
        }

        private void SetOverlayMode(OverlayMode mode)
        {
            currentOverlayMode = mode;
            renderedLegendOverlayMode = (OverlayMode)(-1);
            foreach (KeyValuePair<OverlayMode, Button> pair in overlayButtons)
            {
                pair.Value.EnableInClassList("overlay-selected", pair.Key == currentOverlayMode);
            }

            gasDirty = true;
            overlayDirty = true;
        }

        private void InvalidateRooms()
        {
            roomsDirty = true;
            if (currentOverlayMode == OverlayMode.Rooms)
            {
                gasDirty = true;
            }

            overlayDirty = true;
        }

        private void AdjustSleepStart(float delta)
        {
            sleepStartCycleTime = NormalizeCycleTime(sleepStartCycleTime + delta);
            NormalizeSleepWindow();
            Log("Sleep starts at " + CycleClockLabel(sleepStartCycleTime) + ".");
        }

        private void AdjustSleepEnd(float delta)
        {
            sleepEndCycleTime = NormalizeCycleTime(sleepEndCycleTime + delta);
            NormalizeSleepWindow();
            Log("Wake time set to " + CycleClockLabel(sleepEndCycleTime) + ".");
        }

        private float NormalizeCycleTime(float value)
        {
            value %= 1f;
            return value < 0f ? value + 1f : value;
        }

        private float SleepWindowDuration()
        {
            return sleepStartCycleTime <= sleepEndCycleTime
                ? sleepEndCycleTime - sleepStartCycleTime
                : 1f - sleepStartCycleTime + sleepEndCycleTime;
        }

        private void NormalizeSleepWindow()
        {
            float duration = SleepWindowDuration();
            if (duration < 0.12f)
            {
                sleepEndCycleTime = NormalizeCycleTime(sleepStartCycleTime + 0.12f);
            }
            else if (duration > 0.56f)
            {
                sleepEndCycleTime = NormalizeCycleTime(sleepStartCycleTime + 0.56f);
            }
        }

        private bool TryGetInspectedJob(out Job job)
        {
            job = null;
            if (!inspectedCell.HasValue)
            {
                return false;
            }

            job = FindAnyJobAt(inspectedCell.Value);
            return job != null;
        }

        private void AdjustInspectedJobPriority(int delta)
        {
            if (!TryGetInspectedJob(out Job job))
            {
                return;
            }

            job.Priority = Mathf.Clamp(JobPriority(job) + delta, 1, 10);
            overlayDirty = true;
            Log(JobLabel(job) + " priority " + job.Priority + ".");
        }

        private void CancelInspectedJob()
        {
            if (!inspectedCell.HasValue || FindAnyJobAt(inspectedCell.Value) == null)
            {
                return;
            }

            CancelJobsAt(inspectedCell.Value, true);
        }

        private void SetInspectControlsVisible(bool visible)
        {
            if (priorityDownButton != null)
            {
                SetVisible(priorityDownButton, visible);
            }

            if (priorityUpButton != null)
            {
                SetVisible(priorityUpButton, visible);
            }

            if (cancelSelectedJobButton != null)
            {
                SetVisible(cancelSelectedJobButton, visible);
            }

            if (signalSwitchButton != null)
            {
                SetVisible(signalSwitchButton, false);
            }

            if (airlockToggleButton != null)
            {
                SetVisible(airlockToggleButton, false);
            }
        }

        private void UpdateInspectControls()
        {
            bool jobVisible = inspectedCell.HasValue && FindAnyJobAt(inspectedCell.Value) != null;
            SetInspectControlsVisible(jobVisible);
            if (jobVisible || !inspectedCell.HasValue)
            {
                return;
            }

            Vector2Int cell = inspectedCell.Value;
            bool inside = IsInside(cell.x, cell.y);
            if (signalSwitchButton != null)
            {
                bool switchVisible = inside && cells[cell.x, cell.y] == CellKind.SignalSwitch;
                SetVisible(signalSwitchButton, switchVisible);
                if (switchVisible)
                {
                    signalSwitchButton.text = Localize(automationSwitchState[cell.x, cell.y] ? "Switch\nON" : "Switch\nOFF");
                }
            }

            if (airlockToggleButton != null)
            {
                bool airlockVisible = inside && cells[cell.x, cell.y] == CellKind.ManualAirlock;
                SetVisible(airlockToggleButton, airlockVisible);
                if (airlockVisible)
                {
                    airlockToggleButton.text = Localize(airlockOpen[cell.x, cell.y] ? "Door\nOPEN" : "Door\nCLOSED");
                }
            }
        }

        private void ToggleInspectedSignalSwitch()
        {
            if (!inspectedCell.HasValue)
            {
                return;
            }

            ToggleSignalSwitch(inspectedCell.Value, true);
        }

        private void ToggleSignalSwitch(Vector2Int cell, bool countToggle)
        {
            if (!IsInside(cell.x, cell.y) || cells[cell.x, cell.y] != CellKind.SignalSwitch)
            {
                return;
            }

            automationSwitchState[cell.x, cell.y] = !automationSwitchState[cell.x, cell.y];
            if (countToggle)
            {
                signalSwitchesToggled++;
            }

            UpdateAutomationWires();
            milestoneSignalSwitching |= techPowerRegulation && signalSwitchesToggled > 0 && HasAutomationWireAccess(cell);
            terrainDirty = true;
            gasDirty = true;
            overlayDirty = true;
            Log("Signal Switch " + (automationSwitchState[cell.x, cell.y] ? "ON: green automation signal." : "OFF: red automation signal."));
        }

        private void ToggleInspectedAirlock()
        {
            if (!inspectedCell.HasValue)
            {
                return;
            }

            ToggleAirlock(inspectedCell.Value, true);
        }

        private void ToggleAirlock(Vector2Int cell, bool countToggle)
        {
            if (!IsInside(cell.x, cell.y) || cells[cell.x, cell.y] != CellKind.ManualAirlock)
            {
                return;
            }

            airlockOpen[cell.x, cell.y] = !airlockOpen[cell.x, cell.y];
            if (countToggle)
            {
                airlockToggles++;
            }

            if (!airlockOpen[cell.x, cell.y])
            {
                oxygen[cell.x, cell.y] = 0f;
                carbonDioxide[cell.x, cell.y] = 0f;
                pollutedOxygen[cell.x, cell.y] = 0f;
                hydrogen[cell.x, cell.y] = 0f;
                steam[cell.x, cell.y] = 0f;
                chlorine[cell.x, cell.y] = 0f;
                naturalGas[cell.x, cell.y] = 0f;
                germs[cell.x, cell.y] = 0f;
            }

            milestoneAirlockControl |= airlockToggles > 0 && AnyClosedAirlock();
            terrainDirty = true;
            gasDirty = true;
            overlayDirty = true;
            InvalidateRooms();
            Log("Manual Airlock " + (airlockOpen[cell.x, cell.y] ? "opened: duplicants and gas can pass." : "closed: pathing and pressure are sealed."));
        }

        private void TogglePause()
        {
            paused = !paused;
            Log(paused ? "Simulation paused." : "Simulation resumed.");
        }

        private void ContinueFreeplay()
        {
            if (colonyVictory && !colonyFailed)
            {
                colonyVictoryAcknowledged = true;
                paused = false;
                Log("Freeplay continues.");
                SetVisible(endStatePanel, false);
            }
        }

        private void CycleSpeed()
        {
            if (paused)
            {
                paused = false;
                simulationSpeed = 1f;
            }
            else if (simulationSpeed < 2f)
            {
                simulationSpeed = 2f;
            }
            else if (simulationSpeed < 4f)
            {
                simulationSpeed = 4f;
            }
            else
            {
                simulationSpeed = 1f;
            }

            Log("Simulation speed x" + simulationSpeed.ToString("0"));
        }

        private void QueueDig(Vector2Int cell)
        {
            if (!IsNaturalSolid(cells[cell.x, cell.y]))
            {
                return;
            }

            if (FindJobAt(cell, JobType.Dig) != null)
            {
                return;
            }

            Job job = new Job(JobType.Dig, cell, DigWorkRequired(cells[cell.x, cell.y]))
            {
                Priority = 5
            };
            jobs.Add(job);
            overlayDirty = true;
            Log("Dig queued at " + cell.x + ", " + cell.y + ".");
        }

        private void QueueMop(Vector2Int cell)
        {
            if (!IsMoppableSpill(cell))
            {
                if (IsInside(cell.x, cell.y) && cells[cell.x, cell.y] == CellKind.Water && waterMass[cell.x, cell.y] > MoppableSpillMaxMass)
                {
                    Log("Mop only targets shallow spills under " + MoppableSpillMaxMass.ToString("0") + " kg. Use a pump for deeper water.");
                }

                return;
            }

            if (FindAnyJobAt(cell) != null)
            {
                return;
            }

            Job job = new Job(JobType.Mop, cell, MopWorkRequired(cell))
            {
                Priority = 6
            };
            jobs.Add(job);
            overlayDirty = true;
            Log("Mop queued at " + cell.x + ", " + cell.y + ".");
        }

        private bool IsMoppableSpill(Vector2Int cell)
        {
            return IsInside(cell.x, cell.y) &&
                (cells[cell.x, cell.y] == CellKind.Water || IsFloodableCellKind(cells[cell.x, cell.y])) &&
                waterMass[cell.x, cell.y] > 0.5f &&
                waterMass[cell.x, cell.y] <= MoppableSpillMaxMass &&
                HasAdjacentPassableCell(cell);
        }

        private bool HasAdjacentPassableCell(Vector2Int cell)
        {
            return (IsInside(cell.x + 1, cell.y) && IsPassable(cell.x + 1, cell.y)) ||
                (IsInside(cell.x - 1, cell.y) && IsPassable(cell.x - 1, cell.y)) ||
                (IsInside(cell.x, cell.y + 1) && IsPassable(cell.x, cell.y + 1)) ||
                (IsInside(cell.x, cell.y - 1) && IsPassable(cell.x, cell.y - 1));
        }

        private float MopWorkRequired(Vector2Int cell)
        {
            float mass = IsInside(cell.x, cell.y) ? Mathf.Max(0f, waterMass[cell.x, cell.y]) : 0f;
            float work = Mathf.Lerp(0.9f, 3.2f, Mathf.Clamp01(mass / MoppableSpillMaxMass));
            if (IsPollutedMopCell(cell))
            {
                work += 0.55f;
            }

            return work;
        }

        private bool IsPollutedMopCell(Vector2Int cell)
        {
            return IsInside(cell.x, cell.y) &&
                (germs[cell.x, cell.y] > 0.18f || pollutedOxygen[cell.x, cell.y] > 0.1f);
        }

        private void QueueSweep(Vector2Int cell)
        {
            if (!HasLooseResource(cell))
            {
                return;
            }

            if (FindAnyJobAt(cell) != null)
            {
                return;
            }

            if (DryResourceFreeSpace() <= 0.01f)
            {
                Log("Dry storage is full. Build Storage Bins before sweeping.");
                return;
            }

            Job job = new Job(JobType.Sweep, cell, SweepWorkRequired(cell))
            {
                Priority = 4
            };
            jobs.Add(job);
            overlayDirty = true;
            Log("Sweep queued: " + LooseResourceLabel(looseResourceKind[cell.x, cell.y]) + ".");
        }

        private bool HasLooseResource(Vector2Int cell)
        {
            return IsInside(cell.x, cell.y) &&
                looseResourceKind[cell.x, cell.y] != LooseResourceKind.None &&
                looseResourceAmount[cell.x, cell.y] > 0.05f;
        }

        private float SweepWorkRequired(Vector2Int cell)
        {
            float amount = HasLooseResource(cell) ? looseResourceAmount[cell.x, cell.y] : 0f;
            return Mathf.Lerp(0.8f, 2.8f, Mathf.Clamp01(amount / 24f));
        }

        private void QueueRepair(Vector2Int cell)
        {
            if (!NeedsRepair(cell))
            {
                if (IsInside(cell.x, cell.y) && IsRepairableEquipment(cells[cell.x, cell.y]))
                {
                    Log(CellLabel(cells[cell.x, cell.y]) + " does not need repair.");
                }

                return;
            }

            if (FindAnyJobAt(cell) != null)
            {
                return;
            }

            if (metal < RepairMetalCost)
            {
                Log("Not enough metal to repair " + CellLabel(cells[cell.x, cell.y]) + ".");
                return;
            }

            Job job = new Job(JobType.Repair, cell, RepairWorkRequired(cell))
            {
                BuildKind = cells[cell.x, cell.y],
                Priority = IsBrokenEquipment(cell) ? 8 : 5
            };
            jobs.Add(job);
            overlayDirty = true;
            Log("Repair queued: " + CellLabel(job.BuildKind) + ".");
        }

        private bool NeedsRepair(Vector2Int cell)
        {
            return IsInside(cell.x, cell.y) &&
                IsRepairableEquipment(cells[cell.x, cell.y]) &&
                equipmentCondition[cell.x, cell.y] < 0.995f;
        }

        private bool NeedsAutoRepair(Vector2Int cell)
        {
            return NeedsRepair(cell) &&
                (IsBrokenEquipment(cell) || equipmentCondition[cell.x, cell.y] <= EquipmentAutoRepairThreshold);
        }

        private float RepairWorkRequired(Vector2Int cell)
        {
            float missing = IsInside(cell.x, cell.y) ? 1f - Mathf.Clamp01(equipmentCondition[cell.x, cell.y]) : 1f;
            return Mathf.Lerp(1.2f, 4.6f, missing) + (IsBrokenEquipment(cell) ? 0.8f : 0f);
        }

        private void QueueBuild(Vector2Int cell, CommandMode mode)
        {
            if (mode == CommandMode.PowerWire)
            {
                QueuePowerWire(cell);
                return;
            }

            if (mode == CommandMode.AutomationWire)
            {
                QueueAutomationWire(cell);
                return;
            }

            if (mode == CommandMode.LiquidPipe)
            {
                QueueLiquidPipe(cell);
                return;
            }

            if (mode == CommandMode.GasPipe)
            {
                QueueGasPipe(cell);
                return;
            }

            if (mode == CommandMode.ShippingRail)
            {
                QueueShippingRail(cell);
                return;
            }

            BuildSpec spec = BuildSpecForMode(mode);
            if (spec.Kind == CellKind.Empty)
            {
                return;
            }

            if (!IsBuildUnlocked(spec.Kind, true))
            {
                return;
            }

            if (!CanPlaceBuild(cell, spec.Kind))
            {
                if (spec.Kind == CellKind.WaterPump)
                {
                    Log("Water Pump must be built beside a water tile.");
                }
                else if (RequiresFloorSupport(spec.Kind))
                {
                    Log(spec.Label + " needs floor support.");
                }
                else if (spec.Kind == CellKind.LiquidPipeSensor || spec.Kind == CellKind.LiquidShutoff)
                {
                    Log(spec.Label + " must be built on an existing Liquid Pipe.");
                }
                else if (spec.Kind == CellKind.GasPipeSensor || spec.Kind == CellKind.GasShutoff)
                {
                    Log(spec.Label + " must be built on an existing Gas Pipe.");
                }

                return;
            }

            if (FindAnyJobAt(cell) != null)
            {
                return;
            }

            if (!SpendCost(spec))
            {
                Log("Not enough resources for " + spec.Label + ".");
                return;
            }

            Job job = new Job(JobType.Build, cell, spec.Work)
            {
                BuildKind = spec.Kind,
                DirtCost = spec.Dirt,
                MetalCost = spec.Metal,
                AlgaeCost = spec.Algae,
                RefinedMetalCost = spec.RefinedMetal,
                Priority = 5
            };
            jobs.Add(job);
            overlayDirty = true;
            Log(spec.Label + " build queued.");
        }

        private void QueuePowerWire(Vector2Int cell)
        {
            if (!CanPlacePowerWire(cell))
            {
                if (cells[cell.x, cell.y] == CellKind.Water)
                {
                    Log("Power Wire cannot be built through water.");
                }

                return;
            }

            if (FindAnyJobAt(cell) != null)
            {
                return;
            }

            if (metal < 1f)
            {
                Log("Not enough metal for Power Wire.");
                return;
            }

            metal -= 1f;
            Job job = new Job(JobType.BuildWire, cell, 0.8f)
            {
                MetalCost = 1f,
                Priority = 5,
                BuildWire = true
            };
            jobs.Add(job);
            gasDirty = true;
            overlayDirty = true;
            Log("Power Wire build queued.");
        }

        private bool CanPlacePowerWire(Vector2Int cell)
        {
            return IsInside(cell.x, cell.y) && cells[cell.x, cell.y] != CellKind.Water && !powerWire[cell.x, cell.y];
        }

        private void QueueAutomationWire(Vector2Int cell)
        {
            if (!techPowerRegulation)
            {
                Log("Research Power Regulation before building Automation Wire.");
                return;
            }

            if (!CanPlaceAutomationWire(cell))
            {
                if (cells[cell.x, cell.y] == CellKind.Water)
                {
                    Log("Automation Wire cannot be built through water.");
                }

                return;
            }

            if (FindAnyJobAt(cell) != null)
            {
                return;
            }

            if (metal < 1f)
            {
                Log("Not enough metal for Automation Wire.");
                return;
            }

            metal -= 1f;
            Job job = new Job(JobType.BuildAutomationWire, cell, 0.75f)
            {
                MetalCost = 1f,
                Priority = 5
            };
            jobs.Add(job);
            overlayDirty = true;
            Log("Automation Wire build queued.");
        }

        private bool CanPlaceAutomationWire(Vector2Int cell)
        {
            return IsInside(cell.x, cell.y) && cells[cell.x, cell.y] != CellKind.Water && !automationWire[cell.x, cell.y];
        }

        private void QueueLiquidPipe(Vector2Int cell)
        {
            if (!CanPlaceLiquidPipe(cell))
            {
                if (cells[cell.x, cell.y] == CellKind.Water)
                {
                    Log("Liquid Pipe cannot be built through open water.");
                }

                return;
            }

            if (FindAnyJobAt(cell) != null)
            {
                return;
            }

            if (metal < 1f)
            {
                Log("Not enough metal for Liquid Pipe.");
                return;
            }

            metal -= 1f;
            Job job = new Job(JobType.BuildPipe, cell, 0.8f)
            {
                MetalCost = 1f,
                Priority = 5,
                BuildPipe = true
            };
            jobs.Add(job);
            overlayDirty = true;
            Log("Liquid Pipe build queued.");
        }

        private bool CanPlaceLiquidPipe(Vector2Int cell)
        {
            return IsInside(cell.x, cell.y) && cells[cell.x, cell.y] != CellKind.Water && !liquidPipe[cell.x, cell.y];
        }

        private void QueueGasPipe(Vector2Int cell)
        {
            if (!techAirSystems)
            {
                Log("Research Air Systems before building Gas Pipe.");
                return;
            }

            if (!CanPlaceGasPipe(cell))
            {
                if (cells[cell.x, cell.y] == CellKind.Water)
                {
                    Log("Gas Pipe cannot be built through open water.");
                }

                return;
            }

            if (FindAnyJobAt(cell) != null)
            {
                return;
            }

            if (metal < 1f)
            {
                Log("Not enough metal for Gas Pipe.");
                return;
            }

            metal -= 1f;
            Job job = new Job(JobType.BuildGasPipe, cell, 0.75f)
            {
                MetalCost = 1f,
                Priority = 5,
                BuildGasPipe = true
            };
            jobs.Add(job);
            overlayDirty = true;
            Log("Gas Pipe build queued.");
        }

        private bool CanPlaceGasPipe(Vector2Int cell)
        {
            return IsInside(cell.x, cell.y) && cells[cell.x, cell.y] != CellKind.Water && !gasPipe[cell.x, cell.y];
        }

        private void QueueShippingRail(Vector2Int cell)
        {
            if (!techPowerRegulation)
            {
                Log("Research Power Regulation before building Shipping Rail.");
                return;
            }

            if (!CanPlaceShippingRail(cell))
            {
                if (cells[cell.x, cell.y] == CellKind.Water)
                {
                    Log("Shipping Rail cannot be built through open water.");
                }

                return;
            }

            if (FindAnyJobAt(cell) != null)
            {
                return;
            }

            if (metal < 1f)
            {
                Log("Not enough metal for Shipping Rail.");
                return;
            }

            metal -= 1f;
            Job job = new Job(JobType.BuildShippingRail, cell, 0.75f)
            {
                MetalCost = 1f,
                Priority = 5,
                BuildShippingRail = true
            };
            jobs.Add(job);
            overlayDirty = true;
            Log("Shipping Rail build queued.");
        }

        private bool CanPlaceShippingRail(Vector2Int cell)
        {
            return IsInside(cell.x, cell.y) && cells[cell.x, cell.y] != CellKind.Water && !shippingRail[cell.x, cell.y];
        }

        private void QueueDeconstruct(Vector2Int cell)
        {
            if (!TryGetDeconstructTarget(cell, out CellKind targetKind, out bool removePowerWire, out bool removeAutomationWire, out bool removeLiquidPipe, out bool removeGasPipe, out bool removeShippingRail))
            {
                Log("Nothing deconstructable at " + cell.x + ", " + cell.y + ".");
                return;
            }

            if (FindAnyJobAt(cell) != null)
            {
                return;
            }

            Job job = new Job(JobType.Deconstruct, cell, DeconstructWorkRequired(targetKind, removePowerWire, removeAutomationWire, removeLiquidPipe, removeGasPipe, removeShippingRail))
            {
                BuildKind = targetKind,
                RemovePowerWire = removePowerWire,
                RemoveAutomationWire = removeAutomationWire,
                RemoveLiquidPipe = removeLiquidPipe,
                RemoveGasPipe = removeGasPipe,
                RemoveShippingRail = removeShippingRail,
                Priority = 5
            };
            jobs.Add(job);
            overlayDirty = true;
            Log("Deconstruct queued: " + DeconstructTargetLabel(job) + ".");
        }

        private bool TryGetDeconstructTarget(Vector2Int cell, out CellKind targetKind, out bool removePowerWire, out bool removeAutomationWire, out bool removeLiquidPipe, out bool removeGasPipe, out bool removeShippingRail)
        {
            targetKind = CellKind.Empty;
            removePowerWire = false;
            removeAutomationWire = false;
            removeLiquidPipe = false;
            removeGasPipe = false;
            removeShippingRail = false;

            if (!IsInside(cell.x, cell.y))
            {
                return false;
            }

            if (IsDeconstructableBuilding(cells[cell.x, cell.y]))
            {
                targetKind = cells[cell.x, cell.y];
                return true;
            }

            if (powerWire[cell.x, cell.y])
            {
                removePowerWire = true;
                return true;
            }

            if (automationWire[cell.x, cell.y])
            {
                removeAutomationWire = true;
                return true;
            }

            if (liquidPipe[cell.x, cell.y])
            {
                removeLiquidPipe = true;
                return true;
            }

            if (gasPipe[cell.x, cell.y])
            {
                removeGasPipe = true;
                return true;
            }

            if (shippingRail[cell.x, cell.y])
            {
                removeShippingRail = true;
                return true;
            }

            return false;
        }

        private float DeconstructWorkRequired(CellKind targetKind, bool removePowerWire, bool removeAutomationWire, bool removeLiquidPipe, bool removeGasPipe, bool removeShippingRail)
        {
            if (targetKind != CellKind.Empty)
            {
                BuildSpec spec = BuildSpecForKind(targetKind);
                return Mathf.Max(0.8f, spec.Work * 0.65f);
            }

            return (removePowerWire || removeAutomationWire || removeLiquidPipe || removeGasPipe || removeShippingRail) ? 0.65f : 1f;
        }

        private bool IsDeconstructableBuilding(CellKind kind)
        {
            return BuildSpecForKind(kind).Kind != CellKind.Empty;
        }

        private bool CanPlaceBuild(Vector2Int cell, CellKind buildKind)
        {
            CellKind current = cells[cell.x, cell.y];
            if (IsSolidTile(current))
            {
                return false;
            }

            if (current != CellKind.Empty)
            {
                return false;
            }

            if (HasLooseResource(cell))
            {
                return false;
            }

            if (RequiresFloorSupport(buildKind) && !HasFloorSupport(cell))
            {
                return false;
            }

            if (buildKind == CellKind.WaterPump && !HasAdjacentWater(cell))
            {
                return false;
            }

            if ((buildKind == CellKind.LiquidPipeSensor || buildKind == CellKind.LiquidShutoff) && !liquidPipe[cell.x, cell.y])
            {
                return false;
            }

            if ((buildKind == CellKind.GasPipeSensor || buildKind == CellKind.GasShutoff) && !gasPipe[cell.x, cell.y])
            {
                return false;
            }

            return true;
        }

        private bool RequiresFloorSupport(CellKind buildKind)
        {
            return buildKind == CellKind.Planter ||
                buildKind == CellKind.DecorPlant;
        }

        private bool HasFloorSupport(Vector2Int cell)
        {
            if (cell.y <= 0)
            {
                return true;
            }

            CellKind below = cells[cell.x, cell.y - 1];
            return IsSolidTile(below) ||
                below == CellKind.Floor ||
                below == CellKind.ManualAirlock ||
                below == CellKind.BunkerDoor;
        }

        private void CancelJobsAt(Vector2Int cell, bool forceLog)
        {
            bool cancelled = false;
            for (int i = jobs.Count - 1; i >= 0; i--)
            {
                Job job = jobs[i];
                if (job.Cell != cell)
                {
                    continue;
                }

                RefundJobCost(job);
                if (job.AssignedWorker != null)
                {
                    ClearAssignment(job.AssignedWorker);
                }

                jobs.RemoveAt(i);
                cancelled = true;
            }

            if (cancelled)
            {
                overlayDirty = true;
                Log("Job cancelled at " + cell.x + ", " + cell.y + ".");
            }
            else if (powerWire[cell.x, cell.y])
            {
                powerWire[cell.x, cell.y] = false;
                poweredWire[cell.x, cell.y] = false;
                wireLoad[cell.x, cell.y] = 0f;
                overloadedWire[cell.x, cell.y] = false;
                wireOverloadStress[cell.x, cell.y] = 0f;
                float recovered = StoreDryResource(ref metal, 0.5f);
                gasDirty = true;
                overlayDirty = true;
                Log(recovered > 0f ? "Power Wire removed at " + cell.x + ", " + cell.y + "." : "Power Wire removed, but dry storage is full.");
            }
            else if (automationWire[cell.x, cell.y])
            {
                automationWire[cell.x, cell.y] = false;
                automationControlledWire[cell.x, cell.y] = false;
                automationSignalWire[cell.x, cell.y] = false;
                float recovered = StoreDryResource(ref metal, 0.5f);
                overlayDirty = true;
                Log(recovered > 0f ? "Automation Wire removed at " + cell.x + ", " + cell.y + "." : "Automation Wire removed, but dry storage is full.");
            }
            else if (liquidPipe[cell.x, cell.y])
            {
                float drained = pipeWater[cell.x, cell.y];
                liquidPipe[cell.x, cell.y] = false;
                pipeWater[cell.x, cell.y] = 0f;
                water += drained;
                float recovered = StoreDryResource(ref metal, 0.5f);
                overlayDirty = true;
                Log(recovered > 0f ? "Liquid Pipe removed at " + cell.x + ", " + cell.y + "." : "Liquid Pipe removed, but dry storage is full.");
            }
            else if (gasPipe[cell.x, cell.y])
            {
                ReleaseGasPipeContents(cell);
                gasPipe[cell.x, cell.y] = false;
                gasPipeOxygen[cell.x, cell.y] = 0f;
                gasPipeCarbonDioxide[cell.x, cell.y] = 0f;
                gasPipePollutedOxygen[cell.x, cell.y] = 0f;
                gasPipeHydrogen[cell.x, cell.y] = 0f;
                gasPipeChlorine[cell.x, cell.y] = 0f;
                gasPipeNaturalGas[cell.x, cell.y] = 0f;
                gasPipeGerms[cell.x, cell.y] = 0f;
                float recovered = StoreDryResource(ref metal, 0.5f);
                gasDirty = true;
                overlayDirty = true;
                Log(recovered > 0f ? "Gas Pipe removed at " + cell.x + ", " + cell.y + "." : "Gas Pipe removed, but dry storage is full.");
            }
            else if (shippingRail[cell.x, cell.y])
            {
                ReleaseShippingRailContents(cell);
                shippingRail[cell.x, cell.y] = false;
                shippingRailKind[cell.x, cell.y] = LooseResourceKind.None;
                shippingRailAmount[cell.x, cell.y] = 0f;
                float recovered = StoreDryResource(ref metal, 0.5f);
                overlayDirty = true;
                Log(recovered > 0f ? "Shipping Rail removed at " + cell.x + ", " + cell.y + "." : "Shipping Rail removed, but dry storage is full.");
            }
            else if (forceLog)
            {
                Log("No job at " + cell.x + ", " + cell.y + ".");
            }
        }

        private void CancelQueuedJobsAt(Vector2Int cell)
        {
            bool cancelled = false;
            for (int i = jobs.Count - 1; i >= 0; i--)
            {
                Job job = jobs[i];
                if (job.Cell != cell)
                {
                    continue;
                }

                RefundJobCost(job);
                if (job.AssignedWorker != null)
                {
                    ClearAssignment(job.AssignedWorker);
                }

                jobs.RemoveAt(i);
                cancelled = true;
            }

            if (cancelled)
            {
                overlayDirty = true;
                Log("Job cancelled at " + cell.x + ", " + cell.y + ".");
            }
        }

        private void SimulateColony(float deltaTime)
        {
            if (colonyFailed)
            {
                return;
            }

            elapsedTime += deltaTime;
            cycleTimer += deltaTime;
            gasTimer += deltaTime;
            thermalTimer += deltaTime;
            sandTimer += deltaTime;
            maintenanceTimer += deltaTime;
            autosaveTimer += deltaTime;
            objectiveTimer += deltaTime;
            UpdateJobAges(deltaTime);

            if (cycleTimer >= CycleLengthSeconds)
            {
                cycleTimer -= CycleLengthSeconds;
                cycle++;
                Log("Cycle " + cycle + " started.");
            }

            if (objectiveTimer >= ObjectiveRefreshSeconds)
            {
                objectiveTimer = 0f;
                UpdateColonyStatus(false);
            }

            if (autosaveTimer >= AutosaveIntervalSeconds)
            {
                autosaveTimer = 0f;
                SaveGame(true);
            }

            if (maintenanceTimer >= 0.75f)
            {
                maintenanceTimer = 0f;
                EnsureMaintenanceJobs();
            }

            UpdatePoweredWires();
            UpdateAutomationWires();
            UpdatePowerLoad(deltaTime, true);
            if (currentOverlayMode == OverlayMode.Power)
            {
                gasDirty = true;
            }

            SimulateBuildings(deltaTime);
            SimulatePrintingPods(deltaTime);
            SimulateNaturalVents(deltaTime);
            SimulateMeteorShowers(deltaTime);
            SimulateHatches(deltaTime);
            SimulateShipping(deltaTime);
            SimulatePlumbing(deltaTime);
            SimulateFlooding(deltaTime);
            SimulateVentilation(deltaTime);
            SimulateFoodSpoilage(deltaTime);
            SimulatePollutedDirtOffgas(deltaTime);
            SimulatePollutedWaterOffgas(deltaTime);

            if (sandTimer >= SandFallInterval)
            {
                sandTimer = 0f;
                SimulateFallingSand();
            }

            if (gasTimer >= 0.18f)
            {
                float gasStep = gasTimer;
                gasTimer = 0f;
                StepGas(gasStep);
                gasDirty = true;
            }

            if (thermalTimer >= 0.5f)
            {
                float thermalStep = thermalTimer;
                thermalTimer = 0f;
                StepTemperature(thermalStep);
                SimulateLiquidPipePhaseRuptures(thermalStep);
                SimulateLiquidReservoirPhaseRuptures(thermalStep);
                SimulateEquipmentOverheating(thermalStep);
                gasDirty = true;
            }
        }

        private void SimulateBuildings(float deltaTime)
        {
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    CellKind kind = cells[x, y];
                    Vector2Int cell = new Vector2Int(x, y);
                    if (IsBrokenEquipment(cell))
                    {
                        continue;
                    }

                    if (kind == CellKind.OxygenDiffuser && algae > 0.02f && CanPoweredMachineRun(cell))
                    {
                        float spendPower = Mathf.Min(power, 0.95f * deltaTime);
                        float spendAlgae = Mathf.Min(algae, 0.075f * deltaTime);
                        float efficiency = Mathf.Min(spendPower / Mathf.Max(0.001f, 0.95f * deltaTime), spendAlgae / Mathf.Max(0.001f, 0.075f * deltaTime));
                        float airSystemsBonus = techAirSystems ? 1.35f : 1f;
                        power -= spendPower * efficiency;
                        algae -= spendAlgae * efficiency;
                        AddOxygen(cell, 0.82f * deltaTime * efficiency * airSystemsBonus);
                        AddHeat(cell, 0.08f * deltaTime * efficiency, 1);
                        WearEquipment(cell, 0.00034f * deltaTime * efficiency);
                    }
                    else if (kind == CellKind.Planter)
                    {
                        float localOxygen = oxygen[x, y];
                        float localTemperature = temperature[x, y];
                        float localPressure = TileGasTotal(x, y);
                        bool irrigated = water > 0.05f;
                        bool flooded = waterMass[x, y] >= CropFloodWaterMass;
                        bool overpressured = localPressure > OverpressureDamageThreshold;
                        bool comfortableTemperature = localTemperature >= 8f && localTemperature <= 34f;
                        bool lethalTemperature = localTemperature < -2f || localTemperature > 45f;
                        bool healthyCrop = localOxygen > 0.18f && irrigated && comfortableTemperature && !flooded && !overpressured;
                        float previousStress = cropStress[x, y];
                        if (healthyCrop)
                        {
                            cropStress[x, y] = Mathf.Max(0f, cropStress[x, y] - CropStressRecoveryRate * deltaTime);
                        }
                        else
                        {
                            cropStress[x, y] = Mathf.Min(CropWiltThresholdSeconds + 12f, cropStress[x, y] + deltaTime);
                            cropStifledSeconds += deltaTime;
                        }

                        float stressPenalty = Mathf.InverseLerp(CropStressThresholdSeconds, CropWiltThresholdSeconds, cropStress[x, y]);
                        float growthRate = healthyCrop ? 0.021f * Mathf.Lerp(1f, 0.22f, stressPenalty) : cropStress[x, y] < CropStressThresholdSeconds ? 0.003f : 0f;
                        bool tended = cropTendedSeconds[x, y] > 0f;
                        if (tended && growthRate > 0f)
                        {
                            growthRate *= CropTendedGrowthMultiplier;
                        }

                        if (lethalTemperature || flooded || overpressured)
                        {
                            growthRate = -0.012f * Mathf.Lerp(1f, 1.8f, stressPenalty);
                        }

                        if (irrigated)
                        {
                            water = Mathf.Max(0f, water - 0.006f * deltaTime);
                        }

                        plantGrowth[x, y] = Mathf.Clamp01(plantGrowth[x, y] + growthRate * deltaTime);
                        cropTendedSeconds[x, y] = Mathf.Max(0f, cropTendedSeconds[x, y] - deltaTime);
                        if ((previousStress < CropStressThresholdSeconds && cropStress[x, y] >= CropStressThresholdSeconds) ||
                            (previousStress >= CropStressThresholdSeconds && cropStress[x, y] < CropStressThresholdSeconds) ||
                            (previousStress < CropWiltThresholdSeconds && cropStress[x, y] >= CropWiltThresholdSeconds) ||
                            (previousStress >= CropWiltThresholdSeconds && cropStress[x, y] < CropWiltThresholdSeconds))
                        {
                            terrainDirty = true;
                            overlayDirty = true;
                        }

                        if (previousStress < CropWiltThresholdSeconds && cropStress[x, y] >= CropWiltThresholdSeconds)
                        {
                            plantGrowth[x, y] = Mathf.Max(0f, plantGrowth[x, y] - 0.22f);
                            cropsWilted++;
                            terrainDirty = true;
                            overlayDirty = true;
                            Log("A planter crop is wilting from " + CropStressReason(new Vector2Int(x, y)) + ".");
                        }
                    }
                    else if (kind == CellKind.Slime)
                    {
                        VentPollutedOxygen(new Vector2Int(x, y), 0.035f * deltaTime, 0.16f * deltaTime);
                    }
                    else if (kind == CellKind.AirDeodorizer && dirt > 0.01f && CanPoweredMachineRun(cell))
                    {
                        RunAirDeodorizer(cell, deltaTime);
                    }
                    else if (kind == CellKind.Electrolyzer && CanPoweredMachineRun(cell))
                    {
                        RunElectrolyzer(cell, deltaTime);
                    }
                    else if (kind == CellKind.CarbonSkimmer && CanPoweredMachineRun(cell))
                    {
                        RunCarbonSkimmer(cell, deltaTime);
                    }
                    else if (kind == CellKind.WaterSieve && CanPoweredMachineRun(cell))
                    {
                        RunWaterSieve(cell, deltaTime);
                    }
                    else if (kind == CellKind.Compost && pollutedDirt > 0.1f)
                    {
                        VentPollutedOxygen(cell, 0.004f * deltaTime, 0.018f * deltaTime);
                        WearEquipment(cell, 0.00012f * deltaTime);
                    }
                    else if (kind == CellKind.SpaceHeater && CanPoweredMachineRun(cell))
                    {
                        RunSpaceHeater(cell, deltaTime);
                    }
                    else if (kind == CellKind.ThermoRegulator && CanPoweredMachineRun(cell))
                    {
                        RunThermoRegulator(cell, deltaTime);
                    }
                    else if (kind == CellKind.Refrigerator && CanPoweredMachineRun(cell))
                    {
                        RunRefrigerator(cell, deltaTime);
                    }
                    else if (kind == CellKind.AutoSweeper && CanPoweredMachineRun(cell))
                    {
                        RunAutoSweeper(cell, deltaTime);
                    }
                    else if (kind == CellKind.CoalGenerator)
                    {
                        RunCoalGenerator(cell, deltaTime);
                    }
                    else if (kind == CellKind.HydrogenGenerator)
                    {
                        RunHydrogenGenerator(cell, deltaTime);
                    }
                    else if (kind == CellKind.NaturalGasGenerator)
                    {
                        RunNaturalGasGenerator(cell, deltaTime);
                    }
                    else if (kind == CellKind.SteamTurbine)
                    {
                        RunSteamTurbine(cell, deltaTime);
                    }
                    else if (kind == CellKind.SolarPanel)
                    {
                        RunSolarPanel(cell, deltaTime);
                    }
                    else if (kind == CellKind.SpaceScanner)
                    {
                        RunSpaceScanner(cell, deltaTime);
                    }
                    else if (kind == CellKind.HydrogenFilter && CanPoweredMachineRun(cell))
                    {
                        RunHydrogenFilter(cell, deltaTime);
                    }
                    else if (kind == CellKind.AtmoSuitDock)
                    {
                        RunAtmoSuitDock(cell, deltaTime);
                    }
                    else if (kind == CellKind.Battery)
                    {
                        AddHeat(cell, 0.015f * deltaTime, 1);
                    }
                    else if (kind == CellKind.SmartBattery)
                    {
                        AddHeat(cell, 0.008f * deltaTime, 1);
                    }
                }
            }
        }

        private void SimulatePrintingPods(float deltaTime)
        {
            if (CountCells(CellKind.PrintingPod) == 0)
            {
                printingPodProgress = 0f;
                return;
            }

            if (workers.Count >= MaxWorkers || CountActiveWorkers() == 0)
            {
                return;
            }

            printingPodProgress = Mathf.Clamp01(printingPodProgress + deltaTime / PrintingPodChargeSeconds);
            if (printingPodProgress >= 1f && TryPrintDuplicant())
            {
                printingPodProgress = 0f;
            }
        }

        private bool TryPrintDuplicant()
        {
            if (workers.Count >= MaxWorkers)
            {
                return false;
            }

            if (!TryFindPrintingPodSpawn(out Vector2Int spawnCell))
            {
                Log("Printing Pod is ready, but nearby space is blocked.");
                return false;
            }

            string workerName = NextDuplicantName();
            SpawnWorker(workerName, spawnCell, WorkerTint(workers.Count));
            Worker worker = workers[workers.Count - 1];
            worker.Stress = 6f;
            worker.Fatigue = 12f;
            worker.Bladder = 12f;
            AddFreshFood(700f, 0.92f);
            water += 8f;
            duplicantsPrinted++;
            milestoneColonyExpansion = true;
            terrainDirty = true;
            overlayDirty = true;
            Log("Printing Pod welcomed " + workerName + ". Colony population is now " + workers.Count + ".");
            return true;
        }

        private bool TryFindPrintingPodSpawn(out Vector2Int spawnCell)
        {
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] != CellKind.PrintingPod)
                    {
                        continue;
                    }

                    if (TryFindOpenSpawnNear(new Vector2Int(x, y), out spawnCell))
                    {
                        return true;
                    }
                }
            }

            spawnCell = Vector2Int.zero;
            return false;
        }

        private bool TryFindOpenSpawnNear(Vector2Int center, out Vector2Int spawnCell)
        {
            Vector2Int[] offsets =
            {
                new Vector2Int(0, 1),
                new Vector2Int(1, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(0, -1),
                new Vector2Int(1, 1),
                new Vector2Int(-1, 1),
                new Vector2Int(1, -1),
                new Vector2Int(-1, -1),
                new Vector2Int(2, 0),
                new Vector2Int(-2, 0)
            };

            for (int i = 0; i < offsets.Length; i++)
            {
                Vector2Int candidate = center + offsets[i];
                if (IsCharacterStandableCell(candidate) && WorkerAt(candidate) == null)
                {
                    spawnCell = candidate;
                    return true;
                }
            }

            spawnCell = Vector2Int.zero;
            return false;
        }

        private bool TryFindCharacterStandableCellNear(Vector2Int center, int radius, Worker ignoreWorker, out Vector2Int standableCell)
        {
            for (int distance = 0; distance <= radius; distance++)
            {
                for (int dy = -distance; dy <= distance; dy++)
                {
                    int dxLimit = distance - Mathf.Abs(dy);
                    for (int dx = -dxLimit; dx <= dxLimit; dx++)
                    {
                        if (Mathf.Abs(dx) + Mathf.Abs(dy) != distance)
                        {
                            continue;
                        }

                        Vector2Int candidate = center + new Vector2Int(dx, dy);
                        Worker occupant = WorkerAt(candidate);
                        if (IsCharacterStandableCell(candidate) && (occupant == null || occupant == ignoreWorker))
                        {
                            standableCell = candidate;
                            return true;
                        }
                    }
                }
            }

            standableCell = Vector2Int.zero;
            return false;
        }

        private string NextDuplicantName()
        {
            string[] names =
            {
                "Duri",
                "Eun",
                "Haru",
                "Jin",
                "Kira",
                "Miro",
                "Nari",
                "Oren",
                "Pia",
                "Rin",
                "Sol",
                "Tae"
            };

            for (int i = 0; i < names.Length; i++)
            {
                if (FindWorkerByName(names[i]) == null)
                {
                    return names[i];
                }
            }

            return "Dup " + (workers.Count + duplicantsPrinted + 1);
        }

        private void SimulateNaturalVents(float deltaTime)
        {
            bool terrainChanged = false;
            bool gasChanged = false;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    CellKind kind = cells[x, y];
                    if (kind != CellKind.SteamVent && kind != CellKind.HydrogenVent && kind != CellKind.NaturalGasVent)
                    {
                        continue;
                    }

                    Vector2Int vent = new Vector2Int(x, y);
                    if (!NaturalVentActive(vent))
                    {
                        continue;
                    }

                    if (kind == CellKind.SteamVent)
                    {
                        terrainChanged |= RunSteamVent(vent, deltaTime);
                    }
                    else if (kind == CellKind.HydrogenVent)
                    {
                        gasChanged |= RunHydrogenVent(vent, deltaTime);
                    }
                    else
                    {
                        gasChanged |= RunNaturalGasVent(vent, deltaTime);
                    }
                }
            }

            if (terrainChanged)
            {
                terrainDirty = true;
                overlayDirty = true;
            }

            if (gasChanged)
            {
                gasDirty = true;
                overlayDirty = true;
            }
        }

        private bool RunSteamVent(Vector2Int vent, float deltaTime)
        {
            Vector2Int output;
            if (!TryFindNaturalVentLiquidOutput(vent, out output))
            {
                AddHeat(vent, 0.12f * deltaTime, 2);
                return false;
            }

            float amount = Mathf.Min(SteamVentWaterRate * deltaTime, 140f - waterMass[output.x, output.y]);
            if (amount <= 0.001f)
            {
                AddHeat(vent, 0.12f * deltaTime, 2);
                return false;
            }

            ReleaseWaterToCell(output, amount);
            temperature[output.x, output.y] = Mathf.Clamp(Mathf.Max(temperature[output.x, output.y], 72f), -30f, 120f);
            AddHeat(vent, 0.36f * deltaTime, 2);
            renewableWaterGenerated += amount;
            return true;
        }

        private bool RunHydrogenVent(Vector2Int vent, float deltaTime)
        {
            Vector2Int output;
            if (!TryFindNaturalVentGasOutput(vent, out output))
            {
                AddHeat(vent, 0.08f * deltaTime, 2);
                return false;
            }

            float amount = Mathf.Min(HydrogenVentRate * deltaTime, NaturalVentOutputPressure - TileGasTotal(output.x, output.y));
            if (amount <= 0.001f)
            {
                AddHeat(vent, 0.08f * deltaTime, 2);
                return false;
            }

            AddGasToTile(output.x, output.y, 0f, 0f, 0f, amount, 0f, 0f, 0f);
            temperature[output.x, output.y] = Mathf.Clamp(Mathf.Max(temperature[output.x, output.y], 48f), -30f, 120f);
            AddHeat(vent, 0.20f * deltaTime, 2);
            renewableHydrogenGenerated += amount;
            return true;
        }

        private bool RunNaturalGasVent(Vector2Int vent, float deltaTime)
        {
            Vector2Int output;
            if (!TryFindNaturalVentGasOutput(vent, out output))
            {
                AddHeat(vent, 0.09f * deltaTime, 2);
                return false;
            }

            float amount = Mathf.Min(NaturalGasVentRate * deltaTime, NaturalVentOutputPressure - TileGasTotal(output.x, output.y));
            if (amount <= 0.001f)
            {
                AddHeat(vent, 0.09f * deltaTime, 2);
                return false;
            }

            AddGasToTile(output.x, output.y, 0f, 0f, 0f, 0f, 0f, amount, 0f);
            temperature[output.x, output.y] = Mathf.Clamp(Mathf.Max(temperature[output.x, output.y], 58f), -30f, 120f);
            AddHeat(vent, 0.22f * deltaTime, 2);
            renewableNaturalGasGenerated += amount;
            return true;
        }

        private bool NaturalVentActive(Vector2Int vent)
        {
            float offset = Mathf.Abs(vent.x * 13.37f + vent.y * 3.19f);
            float phase = Mathf.Repeat(elapsedTime + offset, NaturalVentCycleSeconds);
            return phase < NaturalVentActiveSeconds;
        }

        private string NaturalVentStateText(Vector2Int vent)
        {
            float offset = Mathf.Abs(vent.x * 13.37f + vent.y * 3.19f);
            float phase = Mathf.Repeat(elapsedTime + offset, NaturalVentCycleSeconds);
            if (phase < NaturalVentActiveSeconds)
            {
                return "Active for " + (NaturalVentActiveSeconds - phase).ToString("0") + "s.";
            }

            return "Dormant for " + (NaturalVentCycleSeconds - phase).ToString("0") + "s.";
        }

        private void SimulateHatches(float deltaTime)
        {
            for (int i = 0; i < hatches.Count; i++)
            {
                HatchCritter hatch = hatches[i];
                if (hatch.Transform == null)
                {
                    continue;
                }

                hatch.GroomedSeconds = Mathf.Max(0f, hatch.GroomedSeconds - deltaTime);
                float targetHappiness = hatch.GroomedSeconds > 0f ? 86f : 34f;
                hatch.Happiness = Mathf.MoveTowards(hatch.Happiness, targetHappiness, (hatch.GroomedSeconds > 0f ? 4.2f : 1.8f) * deltaTime);

                Vector3 targetPosition = CellCenter(hatch.TargetCell);
                hatch.Transform.position = Vector3.MoveTowards(hatch.Transform.position, targetPosition, HatchMoveSpeed * deltaTime);
                if ((hatch.Transform.position - targetPosition).sqrMagnitude <= 0.0025f)
                {
                    hatch.Transform.position = targetPosition;
                    hatch.Cell = hatch.TargetCell;
                    hatch.MoveTimer -= deltaTime;
                    if (hatch.MoveTimer <= 0f)
                    {
                        hatch.TargetCell = ChooseHatchMoveTarget(hatch);
                        hatch.MoveTimer = HatchMoveIntervalSeconds + Mathf.Repeat(hatch.Cell.x * 0.37f + hatch.Cell.y * 0.19f + elapsedTime, 1.1f);
                    }
                }

                hatch.EatTimer -= deltaTime;
                if (hatch.EatTimer <= 0f)
                {
                    hatch.EatTimer = HatchEatIntervalSeconds;
                    TryFeedHatch(hatch);
                }

                if (hatch.Renderer != null)
                {
                    float groomed = Mathf.Clamp01(hatch.GroomedSeconds / HatchGroomedSeconds);
                    hatch.Renderer.color = Color.Lerp(new Color(0.70f, 0.52f, 0.32f, 1f), new Color(1f, 0.82f, 0.48f, 1f), groomed);
                }
            }
        }

        private Vector2Int ChooseHatchMoveTarget(HatchCritter hatch)
        {
            Vector2Int foodCell;
            if (TryFindHatchFoodCell(hatch, out foodCell) && IsPassable(foodCell.x, foodCell.y))
            {
                return foodCell;
            }

            Vector2Int[] offsets =
            {
                new Vector2Int(1, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, -1),
                new Vector2Int(1, 1),
                new Vector2Int(-1, 1),
                new Vector2Int(1, -1),
                new Vector2Int(-1, -1)
            };

            int start = Mathf.Abs(Mathf.RoundToInt(hatch.Cell.x * 3 + hatch.Cell.y * 5 + elapsedTime * 2f)) % offsets.Length;
            for (int i = 0; i < offsets.Length; i++)
            {
                Vector2Int candidate = hatch.Cell + offsets[(start + i) % offsets.Length];
                if (CanHatchMoveTo(candidate))
                {
                    return candidate;
                }
            }

            return hatch.Cell;
        }

        private bool CanHatchMoveTo(Vector2Int cell)
        {
            return IsInside(cell.x, cell.y) &&
                IsPassable(cell.x, cell.y) &&
                waterMass[cell.x, cell.y] <= 0.05f &&
                temperature[cell.x, cell.y] > -12f &&
                temperature[cell.x, cell.y] < 54f;
        }

        private bool TryFeedHatch(HatchCritter hatch)
        {
            Vector2Int foodCell;
            if (!TryFindHatchFoodCell(hatch, out foodCell))
            {
                hatch.Happiness = Mathf.Max(0f, hatch.Happiness - 4f);
                return false;
            }

            float eaten = Mathf.Min(HatchEatAmount, looseResourceAmount[foodCell.x, foodCell.y]);
            if (eaten <= 0.05f)
            {
                return false;
            }

            looseResourceAmount[foodCell.x, foodCell.y] -= eaten;
            if (looseResourceAmount[foodCell.x, foodCell.y] <= 0.05f)
            {
                looseResourceAmount[foodCell.x, foodCell.y] = 0f;
                looseResourceKind[foodCell.x, foodCell.y] = LooseResourceKind.None;
            }

            float happinessScale = Mathf.Lerp(0.65f, 1.35f, Mathf.Clamp01(hatch.Happiness / 100f));
            float coalAmount = eaten * HatchCoalYield * happinessScale;
            AddLooseResource(hatch.Cell, LooseResourceKind.Coal, coalAmount);
            hatch.CoalProduced += coalAmount;
            hatchCoalProduced += coalAmount;
            hatch.Happiness = Mathf.Min(100f, hatch.Happiness + 3f);
            terrainDirty = true;
            overlayDirty = true;
            return true;
        }

        private bool TryFindHatchFoodCell(HatchCritter hatch, out Vector2Int foodCell)
        {
            foodCell = new Vector2Int(-1, -1);
            Vector2Int[] candidates =
            {
                hatch.Cell,
                hatch.TargetCell,
                new Vector2Int(hatch.Cell.x + 1, hatch.Cell.y),
                new Vector2Int(hatch.Cell.x - 1, hatch.Cell.y),
                new Vector2Int(hatch.Cell.x, hatch.Cell.y + 1),
                new Vector2Int(hatch.Cell.x, hatch.Cell.y - 1)
            };

            float bestAmount = 0.05f;
            for (int i = 0; i < candidates.Length; i++)
            {
                Vector2Int candidate = candidates[i];
                if (!IsInside(candidate.x, candidate.y) || !IsHatchEdible(looseResourceKind[candidate.x, candidate.y]))
                {
                    continue;
                }

                float amount = looseResourceAmount[candidate.x, candidate.y];
                if (amount <= bestAmount)
                {
                    continue;
                }

                bestAmount = amount;
                foodCell = candidate;
            }

            return foodCell.x >= 0;
        }

        private bool IsHatchEdible(LooseResourceKind kind)
        {
            return kind == LooseResourceKind.Dirt ||
                kind == LooseResourceKind.Algae ||
                kind == LooseResourceKind.PollutedDirt;
        }

        private HatchCritter HatchAt(Vector2Int cell)
        {
            for (int i = 0; i < hatches.Count; i++)
            {
                HatchCritter hatch = hatches[i];
                if (hatch.Cell == cell || hatch.TargetCell == cell)
                {
                    return hatch;
                }
            }

            return null;
        }

        private void SimulateShipping(float deltaTime)
        {
            bool changed = false;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    Vector2Int cell = new Vector2Int(x, y);
                    if (cells[x, y] == CellKind.ConveyorLoader && CanPoweredMachineRun(cell))
                    {
                        changed |= RunConveyorLoader(cell, deltaTime);
                    }
                }
            }

            float railStep = deltaTime / 3f;
            for (int i = 0; i < 3; i++)
            {
                changed |= MoveShippingRailPackets(railStep);
            }

            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] == CellKind.ConveyorChute)
                    {
                        changed |= RunConveyorChute(new Vector2Int(x, y), deltaTime);
                    }
                }
            }

            if (changed)
            {
                terrainDirty = true;
                overlayDirty = true;
            }
        }

        private bool RunConveyorLoader(Vector2Int loader, float deltaTime)
        {
            if (!TrySelectDryResourceForShipping(out LooseResourceKind kind, out float available) ||
                !TryFindShippingRailWithSpace(loader, kind, out Vector2Int rail))
            {
                return false;
            }

            float requested = ConveyorLoaderTransferRate * deltaTime;
            float target = Mathf.Min(requested, Mathf.Min(available, ShippingRailFreeSpace(rail, kind)));
            if (target <= 0.001f)
            {
                return false;
            }

            float targetPower = ConveyorLoaderPowerRate * deltaTime * (target / Mathf.Max(0.001f, requested));
            float powerUsed = Mathf.Min(power, targetPower);
            float efficiency = powerUsed / Mathf.Max(0.001f, targetPower);
            float consumed = ConsumeDryResource(kind, target * efficiency);
            float loaded = AddShippingRailPacket(rail, kind, consumed);
            if (loaded <= 0.001f)
            {
                StoreLooseResource(kind, consumed);
                return false;
            }

            power -= powerUsed * Mathf.Clamp01(loaded / Mathf.Max(0.001f, consumed));
            if (loaded < consumed)
            {
                StoreLooseResource(kind, consumed - loaded);
            }

            AddHeat(loader, 0.025f * deltaTime * efficiency, 1);
            WearEquipment(loader, 0.00024f * deltaTime * efficiency);
            return true;
        }

        private bool MoveShippingRailPackets(float deltaTime)
        {
            if (deltaTime <= 0f)
            {
                return false;
            }

            bool changed = false;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (!shippingRail[x, y] || shippingRailAmount[x, y] <= 0.001f || shippingRailKind[x, y] == LooseResourceKind.None)
                    {
                        continue;
                    }

                    Vector2Int rail = new Vector2Int(x, y);
                    LooseResourceKind kind = shippingRailKind[x, y];
                    if (!TryFindNextShippingRailTowardChute(rail, kind, out Vector2Int nextRail))
                    {
                        continue;
                    }

                    float move = Mathf.Min(shippingRailAmount[x, y], Mathf.Min(ShippingRailMoveRate * deltaTime, ShippingRailFreeSpace(nextRail, kind)));
                    if (move <= 0.001f)
                    {
                        continue;
                    }

                    shippingRailAmount[x, y] = Mathf.Max(0f, shippingRailAmount[x, y] - move);
                    if (shippingRailAmount[x, y] <= 0.001f)
                    {
                        shippingRailAmount[x, y] = 0f;
                        shippingRailKind[x, y] = LooseResourceKind.None;
                    }

                    AddShippingRailPacket(nextRail, kind, move);
                    changed = true;
                }
            }

            return changed;
        }

        private bool RunConveyorChute(Vector2Int chute, float deltaTime)
        {
            if (!TryFindAdjacentShippingRailWithPacket(chute, out Vector2Int rail))
            {
                return false;
            }

            LooseResourceKind kind = shippingRailKind[rail.x, rail.y];
            float amount = Mathf.Min(shippingRailAmount[rail.x, rail.y], ConveyorChuteDropRate * deltaTime);
            float dropped = DropLooseResourceNear(chute, kind, amount);
            if (dropped <= 0.001f)
            {
                return false;
            }

            shippingRailAmount[rail.x, rail.y] = Mathf.Max(0f, shippingRailAmount[rail.x, rail.y] - dropped);
            if (shippingRailAmount[rail.x, rail.y] <= 0.001f)
            {
                shippingRailAmount[rail.x, rail.y] = 0f;
                shippingRailKind[rail.x, rail.y] = LooseResourceKind.None;
            }

            conveyorShippedResources += dropped;
            milestoneShippingLogistics |= techPowerRegulation && conveyorShippedResources >= 6f;
            return true;
        }

        private bool TrySelectDryResourceForShipping(out LooseResourceKind kind, out float amount)
        {
            kind = LooseResourceKind.None;
            amount = 0f;
            TrySelectDryResourceCandidate(LooseResourceKind.Metal, metal, ref kind, ref amount);
            TrySelectDryResourceCandidate(LooseResourceKind.Coal, coal, ref kind, ref amount);
            TrySelectDryResourceCandidate(LooseResourceKind.RefinedMetal, refinedMetal, ref kind, ref amount);
            TrySelectDryResourceCandidate(LooseResourceKind.Algae, algae, ref kind, ref amount);
            TrySelectDryResourceCandidate(LooseResourceKind.Dirt, dirt, ref kind, ref amount);
            TrySelectDryResourceCandidate(LooseResourceKind.PollutedDirt, pollutedDirt, ref kind, ref amount);
            return kind != LooseResourceKind.None && amount > 0.001f;
        }

        private void TrySelectDryResourceCandidate(LooseResourceKind candidateKind, float candidateAmount, ref LooseResourceKind bestKind, ref float bestAmount)
        {
            if (candidateAmount <= bestAmount)
            {
                return;
            }

            bestKind = candidateKind;
            bestAmount = candidateAmount;
        }

        private float ConsumeDryResource(LooseResourceKind kind, float amount)
        {
            if (amount <= 0f)
            {
                return 0f;
            }

            switch (kind)
            {
                case LooseResourceKind.Dirt:
                    return ConsumeDryResource(ref dirt, amount);
                case LooseResourceKind.Metal:
                    return ConsumeDryResource(ref metal, amount);
                case LooseResourceKind.Algae:
                    return ConsumeDryResource(ref algae, amount);
                case LooseResourceKind.Coal:
                    return ConsumeDryResource(ref coal, amount);
                case LooseResourceKind.RefinedMetal:
                    return ConsumeDryResource(ref refinedMetal, amount);
                case LooseResourceKind.PollutedDirt:
                    return ConsumeDryResource(ref pollutedDirt, amount);
                default:
                    return 0f;
            }
        }

        private float ConsumeDryResource(ref float resource, float amount)
        {
            float consumed = Mathf.Min(Mathf.Max(0f, resource), Mathf.Max(0f, amount));
            resource = Mathf.Max(0f, resource - consumed);
            return consumed;
        }

        private bool TryFindShippingRailWithSpace(Vector2Int center, LooseResourceKind kind, out Vector2Int rail)
        {
            Vector2Int[] candidates =
            {
                center,
                new Vector2Int(center.x + 1, center.y),
                new Vector2Int(center.x - 1, center.y),
                new Vector2Int(center.x, center.y + 1),
                new Vector2Int(center.x, center.y - 1)
            };

            rail = new Vector2Int(-1, -1);
            float bestSpace = 0f;
            for (int i = 0; i < candidates.Length; i++)
            {
                Vector2Int candidate = candidates[i];
                float space = ShippingRailFreeSpace(candidate, kind);
                if (space <= bestSpace)
                {
                    continue;
                }

                bestSpace = space;
                rail = candidate;
            }

            return rail.x >= 0;
        }

        private bool TryFindAdjacentShippingRailWithPacket(Vector2Int center, out Vector2Int rail)
        {
            Vector2Int[] candidates =
            {
                center,
                new Vector2Int(center.x + 1, center.y),
                new Vector2Int(center.x - 1, center.y),
                new Vector2Int(center.x, center.y + 1),
                new Vector2Int(center.x, center.y - 1)
            };

            rail = new Vector2Int(-1, -1);
            float bestAmount = 0f;
            for (int i = 0; i < candidates.Length; i++)
            {
                Vector2Int candidate = candidates[i];
                if (!IsInside(candidate.x, candidate.y) ||
                    !shippingRail[candidate.x, candidate.y] ||
                    shippingRailKind[candidate.x, candidate.y] == LooseResourceKind.None ||
                    shippingRailAmount[candidate.x, candidate.y] <= bestAmount)
                {
                    continue;
                }

                bestAmount = shippingRailAmount[candidate.x, candidate.y];
                rail = candidate;
            }

            return rail.x >= 0;
        }

        private bool TryFindNextShippingRailTowardChute(Vector2Int rail, LooseResourceKind kind, out Vector2Int nextRail)
        {
            nextRail = new Vector2Int(-1, -1);
            int currentDistance = DistanceToNearestConveyorChute(rail);
            if (currentDistance == int.MaxValue)
            {
                return false;
            }

            Vector2Int[] candidates =
            {
                new Vector2Int(rail.x + 1, rail.y),
                new Vector2Int(rail.x - 1, rail.y),
                new Vector2Int(rail.x, rail.y + 1),
                new Vector2Int(rail.x, rail.y - 1)
            };

            int bestDistance = currentDistance;
            float bestSpace = 0f;
            for (int i = 0; i < candidates.Length; i++)
            {
                Vector2Int candidate = candidates[i];
                float space = ShippingRailFreeSpace(candidate, kind);
                if (space <= 0.001f)
                {
                    continue;
                }

                int distance = DistanceToNearestConveyorChute(candidate);
                if (distance > bestDistance || (distance == bestDistance && space <= bestSpace))
                {
                    continue;
                }

                bestDistance = distance;
                bestSpace = space;
                nextRail = candidate;
            }

            return nextRail.x >= 0 && bestDistance < currentDistance;
        }

        private int DistanceToNearestConveyorChute(Vector2Int cell)
        {
            int best = int.MaxValue;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] != CellKind.ConveyorChute)
                    {
                        continue;
                    }

                    int distance = Mathf.Abs(cell.x - x) + Mathf.Abs(cell.y - y);
                    if (distance < best)
                    {
                        best = distance;
                    }
                }
            }

            return best;
        }

        private float ShippingRailFreeSpace(Vector2Int rail, LooseResourceKind kind)
        {
            if (!IsInside(rail.x, rail.y) || !shippingRail[rail.x, rail.y])
            {
                return 0f;
            }

            LooseResourceKind currentKind = shippingRailKind[rail.x, rail.y];
            if (currentKind != LooseResourceKind.None && currentKind != kind)
            {
                return 0f;
            }

            return Mathf.Max(0f, ShippingRailCapacity - shippingRailAmount[rail.x, rail.y]);
        }

        private float AddShippingRailPacket(Vector2Int rail, LooseResourceKind kind, float amount)
        {
            if (kind == LooseResourceKind.None || amount <= 0f || ShippingRailFreeSpace(rail, kind) <= 0.001f)
            {
                return 0f;
            }

            float loaded = Mathf.Min(amount, ShippingRailCapacity - shippingRailAmount[rail.x, rail.y]);
            shippingRailKind[rail.x, rail.y] = kind;
            shippingRailAmount[rail.x, rail.y] += loaded;
            return loaded;
        }

        private float DropLooseResourceNear(Vector2Int origin, LooseResourceKind kind, float amount)
        {
            if (kind == LooseResourceKind.None || amount <= 0f || !TryFindLooseResourceDropCell(origin, kind, out Vector2Int dropCell))
            {
                return 0f;
            }

            float capacity = looseResourceKind[dropCell.x, dropCell.y] == LooseResourceKind.None ? 80f : 80f - looseResourceAmount[dropCell.x, dropCell.y];
            float dropped = Mathf.Min(amount, Mathf.Max(0f, capacity));
            if (dropped <= 0.001f)
            {
                return 0f;
            }

            looseResourceKind[dropCell.x, dropCell.y] = kind;
            looseResourceAmount[dropCell.x, dropCell.y] += dropped;
            return dropped;
        }

        private void ReleaseShippingRailContents(Vector2Int rail)
        {
            if (!IsInside(rail.x, rail.y) || shippingRailKind[rail.x, rail.y] == LooseResourceKind.None || shippingRailAmount[rail.x, rail.y] <= 0.001f)
            {
                return;
            }

            float dropped = DropLooseResourceNear(rail, shippingRailKind[rail.x, rail.y], shippingRailAmount[rail.x, rail.y]);
            if (dropped < shippingRailAmount[rail.x, rail.y])
            {
                StoreLooseResource(shippingRailKind[rail.x, rail.y], shippingRailAmount[rail.x, rail.y] - dropped);
            }
        }

        private void SimulatePlumbing(float deltaTime)
        {
            bool changed = false;
            changed |= RunPipedWaterPumps(deltaTime);
            changed |= EqualizePipeWater(deltaTime);
            changed |= RunLiquidReservoirs(deltaTime);
            changed |= RunLiquidVents(deltaTime);
            changed |= StepWorldLiquids(deltaTime);

            if (changed)
            {
                terrainDirty = true;
                gasDirty = true;
                overlayDirty = true;
            }
        }

        private bool RunPipedWaterPumps(float deltaTime)
        {
            bool changed = false;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] != CellKind.WaterPump)
                    {
                        continue;
                    }

                    Vector2Int pump = new Vector2Int(x, y);
                    if (IsBrokenEquipment(pump) ||
                        !CanPoweredMachineRun(pump) ||
                        !TryFindAdjacentWater(pump, out Vector2Int source) ||
                        !TryFindAdjacentLiquidPipeWithSpace(pump, out Vector2Int pipe))
                    {
                        continue;
                    }

                    float powerUsed = Mathf.Min(power, 0.24f * deltaTime);
                    float efficiency = powerUsed / Mathf.Max(0.001f, 0.24f * deltaTime);
                    float amount = Mathf.Min(
                        waterMass[source.x, source.y],
                        Mathf.Min(LiquidPumpRate * deltaTime * efficiency, LiquidPipeCapacity - pipeWater[pipe.x, pipe.y]));
                    if (amount <= 0.001f)
                    {
                        continue;
                    }

                    power -= powerUsed;
                    waterMass[source.x, source.y] -= amount;
                    pipeWater[pipe.x, pipe.y] += amount;
                    AddHeat(pump, 0.035f * deltaTime * efficiency, 1);
                    WearEquipment(pump, 0.00030f * deltaTime * efficiency);
                    changed = true;

                    if (waterMass[source.x, source.y] <= 0.5f)
                    {
                        waterMass[source.x, source.y] = 0f;
                        cells[source.x, source.y] = CellKind.Empty;
                        equipmentCondition[source.x, source.y] = 0f;
                        oxygen[source.x, source.y] = NeighborAverage(oxygen, source.x, source.y, 0.12f);
                        carbonDioxide[source.x, source.y] = NeighborAverage(carbonDioxide, source.x, source.y, 0.04f);
                        hydrogen[source.x, source.y] = NeighborAverage(hydrogen, source.x, source.y, 0f);
                        chlorine[source.x, source.y] = NeighborAverage(chlorine, source.x, source.y, 0f);
                        naturalGas[source.x, source.y] = NeighborAverage(naturalGas, source.x, source.y, 0f);
                    }
                }
            }

            return changed;
        }

        private bool EqualizePipeWater(float deltaTime)
        {
            bool changed = false;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (!liquidPipe[x, y])
                    {
                        continue;
                    }

                    changed |= EqualizePipePair(x, y, x + 1, y, deltaTime);
                    changed |= EqualizePipePair(x, y, x, y + 1, deltaTime);
                }
            }

            return changed;
        }

        private bool EqualizePipePair(int ax, int ay, int bx, int by, float deltaTime)
        {
            if (!IsInside(bx, by) ||
                !liquidPipe[ax, ay] ||
                !liquidPipe[bx, by] ||
                IsLiquidConduitBlocked(ax, ay) ||
                IsLiquidConduitBlocked(bx, by))
            {
                return false;
            }

            float difference = pipeWater[ax, ay] - pipeWater[bx, by];
            if (Mathf.Abs(difference) < 0.02f)
            {
                return false;
            }

            float flow = Mathf.Clamp(difference * 0.35f * deltaTime, -LiquidPipeCapacity * 0.35f, LiquidPipeCapacity * 0.35f);
            if (flow > 0f)
            {
                flow = Mathf.Min(flow, pipeWater[ax, ay], LiquidPipeCapacity - pipeWater[bx, by]);
            }
            else
            {
                float reverse = Mathf.Min(-flow, pipeWater[bx, by], LiquidPipeCapacity - pipeWater[ax, ay]);
                flow = -reverse;
            }

            if (Mathf.Abs(flow) <= 0.001f)
            {
                return false;
            }

            pipeWater[ax, ay] -= flow;
            pipeWater[bx, by] += flow;
            TrackAutomatedConduitFlow(ax, ay, bx, by, Mathf.Abs(flow), false);
            return true;
        }

        private bool RunLiquidReservoirs(float deltaTime)
        {
            bool changed = false;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] != CellKind.LiquidReservoir)
                    {
                        continue;
                    }

                    Vector2Int reservoir = new Vector2Int(x, y);
                    if (IsBrokenEquipment(reservoir))
                    {
                        continue;
                    }

                    Vector2Int inputPipe = new Vector2Int(-1, -1);
                    if (liquidReservoirWater[x, y] < LiquidReservoirCapacity - 0.001f &&
                        TryFindAdjacentLiquidPipeWithWater(reservoir, out inputPipe))
                    {
                        float amount = Mathf.Min(
                            pipeWater[inputPipe.x, inputPipe.y],
                            Mathf.Min(LiquidReservoirRate * deltaTime, LiquidReservoirCapacity - liquidReservoirWater[x, y]));
                        if (amount > 0.001f)
                        {
                            pipeWater[inputPipe.x, inputPipe.y] -= amount;
                            liquidReservoirWater[x, y] += amount;
                            reservoirBufferedMass += amount;
                            WearEquipment(reservoir, 0.00008f * deltaTime);
                            changed = true;
                        }
                    }

                    if (liquidReservoirWater[x, y] > 0.001f &&
                        TryFindAdjacentLiquidPipeWithSpaceExcluding(reservoir, inputPipe, out Vector2Int outputPipe))
                    {
                        float amount = Mathf.Min(
                            liquidReservoirWater[x, y],
                            Mathf.Min(LiquidReservoirRate * deltaTime, LiquidPipeCapacity - pipeWater[outputPipe.x, outputPipe.y]));
                        if (amount > 0.001f)
                        {
                            liquidReservoirWater[x, y] -= amount;
                            pipeWater[outputPipe.x, outputPipe.y] += amount;
                            WearEquipment(reservoir, 0.00006f * deltaTime);
                            changed = true;
                        }
                    }
                }
            }

            return changed;
        }

        private bool RunLiquidVents(float deltaTime)
        {
            bool changed = false;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] != CellKind.LiquidVent)
                    {
                        continue;
                    }

                    Vector2Int vent = new Vector2Int(x, y);
                    if (IsBrokenEquipment(vent))
                    {
                        continue;
                    }

                    if (!TryFindAdjacentLiquidPipeWithWater(vent, out Vector2Int pipe) ||
                        !TryFindLiquidVentOutput(vent, out Vector2Int output))
                    {
                        continue;
                    }

                    float amount = Mathf.Min(pipeWater[pipe.x, pipe.y], LiquidVentRate * deltaTime);
                    amount = Mathf.Min(amount, LiquidTileCapacity - waterMass[output.x, output.y]);
                    if (amount <= 0.001f)
                    {
                        continue;
                    }

                    pipeWater[pipe.x, pipe.y] -= amount;
                    ReleaseWaterToCell(output, amount);
                    WearEquipment(vent, 0.00012f * deltaTime);
                    changed = true;
                }
            }

            return changed;
        }

        private void ReleaseWaterToCell(Vector2Int cell, float amount)
        {
            EnsureWaterCell(cell);
            waterMass[cell.x, cell.y] = Mathf.Min(LiquidTileCapacity, waterMass[cell.x, cell.y] + amount);
            oxygen[cell.x, cell.y] = Mathf.Max(0f, oxygen[cell.x, cell.y] - amount * 0.004f);
            carbonDioxide[cell.x, cell.y] = Mathf.Max(0f, carbonDioxide[cell.x, cell.y] - amount * 0.002f);
            pollutedOxygen[cell.x, cell.y] = Mathf.Max(0f, pollutedOxygen[cell.x, cell.y] - amount * 0.003f);
            hydrogen[cell.x, cell.y] = Mathf.Max(0f, hydrogen[cell.x, cell.y] - amount * 0.004f);
            steam[cell.x, cell.y] = Mathf.Max(0f, steam[cell.x, cell.y] - amount * 0.004f);
            chlorine[cell.x, cell.y] = Mathf.Max(0f, chlorine[cell.x, cell.y] - amount * 0.004f);
            naturalGas[cell.x, cell.y] = Mathf.Max(0f, naturalGas[cell.x, cell.y] - amount * 0.004f);
            DisplaceWorkersFromLiquid(cell);
        }

        private void SimulateFlooding(float deltaTime)
        {
            if (deltaTime <= 0f)
            {
                return;
            }

            bool changed = false;
            bool brokeWire = false;
            int firstWireBreakX = -1;
            int firstWireBreakY = -1;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    float mass = Mathf.Max(0f, waterMass[x, y]);
                    if (mass <= 0.5f)
                    {
                        continue;
                    }

                    Vector2Int cell = new Vector2Int(x, y);
                    if (IsEquipmentSubmerged(cell))
                    {
                        float before = Mathf.Clamp01(equipmentCondition[x, y]);
                        float damage = SubmergedEquipmentDamageRate * deltaTime * Mathf.Clamp01(mass / LiquidTileCapacity);
                        DamageEquipment(cell, damage);
                        float after = Mathf.Clamp01(equipmentCondition[x, y]);
                        submergedEquipmentDamage += Mathf.Max(0f, before - after);
                        changed |= after < before;
                    }

                    if (IsPowerWireFlooded(x, y))
                    {
                        float stress = FloodedWireStressRate * deltaTime * Mathf.Clamp01(mass / LiquidTileCapacity);
                        wireOverloadStress[x, y] += stress;
                        AddHeat(cell, 0.012f * deltaTime, 1);
                        overlayDirty = true;
                        gasDirty = true;

                        if (wireOverloadStress[x, y] >= WireOverloadBreakStress)
                        {
                            powerWire[x, y] = false;
                            poweredWire[x, y] = false;
                            overloadedWire[x, y] = false;
                            wireLoad[x, y] = 0f;
                            wireOverloadStress[x, y] = 0f;
                            floodedWireFailures++;
                            if (!brokeWire)
                            {
                                firstWireBreakX = x;
                                firstWireBreakY = y;
                            }

                            brokeWire = true;
                            changed = true;
                        }
                    }
                }
            }

            if (brokeWire)
            {
                UpdatePoweredWires();
                Log("Flooded Power Wire shorted out at " + firstWireBreakX + ", " + firstWireBreakY + ".");
            }

            if (changed)
            {
                terrainDirty = true;
                overlayDirty = true;
            }
        }

        private void SimulateEquipmentOverheating(float deltaTime)
        {
            if (deltaTime <= 0f)
            {
                return;
            }

            bool changed = false;
            bool failed = false;
            Vector2Int firstFailure = new Vector2Int(-1, -1);
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    CellKind kind = cells[x, y];
                    if (!IsRepairableEquipment(kind))
                    {
                        continue;
                    }

                    Vector2Int cell = new Vector2Int(x, y);
                    if (IsBrokenEquipment(cell))
                    {
                        continue;
                    }

                    float severity = EquipmentOverheatSeverity(x, y);
                    if (severity <= 0.001f)
                    {
                        continue;
                    }

                    float before = Mathf.Clamp01(equipmentCondition[x, y]);
                    DamageEquipment(cell, EquipmentOverheatDamageRate * severity * deltaTime);
                    float after = Mathf.Clamp01(equipmentCondition[x, y]);
                    float damage = Mathf.Max(0f, before - after);
                    if (damage <= 0f)
                    {
                        continue;
                    }

                    overheatedEquipmentDamage += damage;
                    AddHeat(cell, 0.015f * severity * deltaTime, 1);
                    changed = true;
                    if (before > EquipmentBrokenThreshold && after <= EquipmentBrokenThreshold)
                    {
                        overheatedEquipmentFailures++;
                        if (!failed)
                        {
                            firstFailure = cell;
                        }

                        failed = true;
                    }
                }
            }

            if (failed)
            {
                Log(CellLabel(cells[firstFailure.x, firstFailure.y]) + " failed from overheating. Cool or insulate the machinery.");
            }

            if (changed)
            {
                terrainDirty = true;
                overlayDirty = true;
                gasDirty = true;
            }
        }

        private void SimulateLiquidPipePhaseRuptures(float deltaTime)
        {
            if (deltaTime <= 0f)
            {
                return;
            }

            bool changed = false;
            bool ruptured = false;
            bool firstBoiled = false;
            Vector2Int firstRupture = new Vector2Int(-1, -1);
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (!liquidPipe[x, y] || pipeWater[x, y] <= PipePhaseRuptureMinimumMass)
                    {
                        continue;
                    }

                    bool boiling = temperature[x, y] > PipeBoilTemperature;
                    bool freezing = temperature[x, y] < PipeFreezeTemperature;
                    if (!boiling && !freezing)
                    {
                        continue;
                    }

                    Vector2Int pipe = new Vector2Int(x, y);
                    float amount = pipeWater[x, y];
                    liquidPipe[x, y] = false;
                    pipeWater[x, y] = 0f;
                    pipeBurstWater += amount;
                    pipeBurstEvents++;
                    if (boiling)
                    {
                        boiledPipeBursts++;
                        ReleaseBoilingPipeWater(pipe, amount);
                    }
                    else
                    {
                        frozenPipeBursts++;
                        ReleaseFrozenPipeWater(pipe, amount);
                    }

                    if (!ruptured)
                    {
                        firstRupture = pipe;
                        firstBoiled = boiling;
                    }

                    ruptured = true;
                    changed = true;
                }
            }

            if (ruptured)
            {
                Log((firstBoiled ? "Boiling" : "Freezing") + " water ruptured a Liquid Pipe at " + firstRupture.x + ", " + firstRupture.y + ".");
            }

            if (changed)
            {
                terrainDirty = true;
                gasDirty = true;
                overlayDirty = true;
            }
        }

        private void SimulateLiquidReservoirPhaseRuptures(float deltaTime)
        {
            if (deltaTime <= 0f)
            {
                return;
            }

            bool changed = false;
            bool ruptured = false;
            bool firstBoiled = false;
            Vector2Int firstRupture = new Vector2Int(-1, -1);
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] != CellKind.LiquidReservoir || liquidReservoirWater[x, y] <= PipePhaseRuptureMinimumMass)
                    {
                        continue;
                    }

                    bool boiling = temperature[x, y] > PipeBoilTemperature;
                    bool freezing = temperature[x, y] < PipeFreezeTemperature;
                    if (!boiling && !freezing)
                    {
                        continue;
                    }

                    Vector2Int reservoir = new Vector2Int(x, y);
                    float amount = liquidReservoirWater[x, y];
                    liquidReservoirWater[x, y] = 0f;
                    reservoirBurstWater += amount;
                    reservoirBurstEvents++;
                    if (boiling)
                    {
                        boiledReservoirBursts++;
                        ReleaseBoilingPipeWater(reservoir, amount);
                    }
                    else
                    {
                        frozenReservoirBursts++;
                        ReleaseFrozenPipeWater(reservoir, amount);
                    }

                    DamageEquipment(reservoir, 1f);

                    if (!ruptured)
                    {
                        firstRupture = reservoir;
                        firstBoiled = boiling;
                    }

                    ruptured = true;
                    changed = true;
                }
            }

            if (ruptured)
            {
                Log((firstBoiled ? "Boiling" : "Freezing") + " water ruptured a Liquid Reservoir at " + firstRupture.x + ", " + firstRupture.y + ".");
            }

            if (changed)
            {
                terrainDirty = true;
                gasDirty = true;
                overlayDirty = true;
            }
        }

        private void ReleaseBoilingPipeWater(Vector2Int pipe, float amount)
        {
            if (TryFindPipeSteamOutput(pipe, out Vector2Int output))
            {
                float released = AddSteamToTile(output, amount);
                steamEvaporatedMass += released;
                temperature[output.x, output.y] = Mathf.Clamp(Mathf.Max(temperature[output.x, output.y], PipeBoilTemperature + released * 0.2f), -30f, 120f);
                if (released >= amount - 0.001f)
                {
                    return;
                }

                amount -= released;
            }

            if (TryFindPipeLiquidOutput(pipe, out Vector2Int liquidOutput))
            {
                ReleaseWaterToCell(liquidOutput, amount);
                temperature[liquidOutput.x, liquidOutput.y] = Mathf.Clamp(Mathf.Max(temperature[liquidOutput.x, liquidOutput.y], PipeBoilTemperature), -30f, 120f);
            }
        }

        private void ReleaseFrozenPipeWater(Vector2Int pipe, float amount)
        {
            Vector2Int output;
            if (!TryFindPipeLiquidOutput(pipe, out output))
            {
                return;
            }

            if (cells[output.x, output.y] == CellKind.Empty || cells[output.x, output.y] == CellKind.Water)
            {
                cells[output.x, output.y] = CellKind.Ice;
                waterMass[output.x, output.y] = 0f;
                equipmentCondition[output.x, output.y] = 0f;
                plantGrowth[output.x, output.y] = 0f;
                cropTendedSeconds[output.x, output.y] = 0f;
                cropStress[output.x, output.y] = 0f;
                oxygen[output.x, output.y] = 0f;
                carbonDioxide[output.x, output.y] = 0f;
                pollutedOxygen[output.x, output.y] = 0f;
                hydrogen[output.x, output.y] = 0f;
                steam[output.x, output.y] = 0f;
                chlorine[output.x, output.y] = 0f;
                naturalGas[output.x, output.y] = 0f;
                germs[output.x, output.y] = 0f;
                temperature[output.x, output.y] = Mathf.Clamp(Mathf.Min(-1.2f, temperature[pipe.x, pipe.y]), -30f, 120f);
                waterFrozenTiles++;
                CancelQueuedJobsAt(output);
                return;
            }

            ReleaseWaterToCell(output, amount);
            temperature[output.x, output.y] = Mathf.Clamp(Mathf.Min(temperature[output.x, output.y], PipeFreezeTemperature), -30f, 120f);
        }

        private bool TryFindPipeSteamOutput(Vector2Int pipe, out Vector2Int output)
        {
            output = new Vector2Int(-1, -1);
            Vector2Int[] candidates =
            {
                pipe,
                new Vector2Int(pipe.x, pipe.y + 1),
                new Vector2Int(pipe.x + 1, pipe.y),
                new Vector2Int(pipe.x - 1, pipe.y),
                new Vector2Int(pipe.x, pipe.y - 1)
            };

            float bestPressure = float.MaxValue;
            for (int i = 0; i < candidates.Length; i++)
            {
                Vector2Int candidate = candidates[i];
                if (!IsInside(candidate.x, candidate.y) || !IsPassable(candidate.x, candidate.y))
                {
                    continue;
                }

                float pressure = TileGasTotal(candidate.x, candidate.y);
                if (pressure >= 2.78f || pressure >= bestPressure)
                {
                    continue;
                }

                bestPressure = pressure;
                output = candidate;
            }

            return output.x >= 0;
        }

        private bool TryFindPipeLiquidOutput(Vector2Int pipe, out Vector2Int output)
        {
            output = new Vector2Int(-1, -1);
            Vector2Int[] candidates =
            {
                new Vector2Int(pipe.x, pipe.y - 1),
                pipe,
                new Vector2Int(pipe.x + 1, pipe.y),
                new Vector2Int(pipe.x - 1, pipe.y),
                new Vector2Int(pipe.x, pipe.y + 1)
            };

            float bestMass = float.MaxValue;
            for (int i = 0; i < candidates.Length; i++)
            {
                Vector2Int candidate = candidates[i];
                if (!IsInside(candidate.x, candidate.y) || !CanLiquidOccupy(candidate.x, candidate.y))
                {
                    continue;
                }

                float free = LiquidFreeCapacity(candidate.x, candidate.y);
                float mass = LiquidMassAt(candidate.x, candidate.y);
                if (free <= LiquidMinimumRetainedMass || mass >= bestMass)
                {
                    continue;
                }

                bestMass = mass;
                output = candidate;
            }

            return output.x >= 0;
        }

        private float EquipmentOverheatSeverity(int x, int y)
        {
            if (!IsInside(x, y))
            {
                return 0f;
            }

            float temperatureSeverity = temperature[x, y] <= EquipmentOverheatTemperature ? 0f :
                Mathf.Clamp01((temperature[x, y] - EquipmentOverheatTemperature) / Mathf.Max(0.001f, EquipmentCriticalOverheatTemperature - EquipmentOverheatTemperature));
            float steamSeverity = steam[x, y] <= EquipmentSteamOverheatMass || temperature[x, y] <= 52f ? 0f :
                Mathf.Clamp01(steam[x, y] / 2.8f) * Mathf.Clamp01((temperature[x, y] - 52f) / 48f);
            return Mathf.Clamp01(temperatureSeverity + steamSeverity);
        }

        private bool StepWorldLiquids(float deltaTime)
        {
            liquidTimer += deltaTime;
            if (liquidTimer < LiquidWorldStepInterval)
            {
                return false;
            }

            int steps = Mathf.Clamp(Mathf.FloorToInt(liquidTimer / LiquidWorldStepInterval), 1, 3);
            liquidTimer -= steps * LiquidWorldStepInterval;
            bool changed = false;
            for (int i = 0; i < steps; i++)
            {
                changed |= StepWorldLiquidOnce();
            }

            return changed;
        }

        private bool StepWorldLiquidOnce()
        {
            float movedMass = 0f;
            int movedEvents = 0;
            for (int y = 1; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (!HasWorldLiquidAt(x, y))
                    {
                        continue;
                    }

                    float freeCapacity = LiquidFreeCapacity(x, y - 1);
                    if (freeCapacity <= LiquidMinimumRetainedMass)
                    {
                        continue;
                    }

                    float amount = Mathf.Min(LiquidVerticalStepMass, freeCapacity, waterMass[x, y]);
                    if (waterMass[x, y] - amount <= LiquidMinimumRetainedMass)
                    {
                        amount = waterMass[x, y];
                    }

                    float moved = MoveWorldLiquidMass(new Vector2Int(x, y), new Vector2Int(x, y - 1), amount);
                    if (moved > 0.001f)
                    {
                        movedMass += moved;
                        movedEvents++;
                    }
                }
            }

            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (!HasWorldLiquidAt(x, y) || CanLiquidFlowDown(x, y))
                    {
                        continue;
                    }

                    if (!TryFindLiquidSideTarget(x, y, out Vector2Int target))
                    {
                        continue;
                    }

                    float targetMass = LiquidMassAt(target.x, target.y);
                    float equalizingMass = Mathf.Max(0f, (waterMass[x, y] - targetMass) * 0.5f);
                    float amount = Mathf.Min(LiquidSideStepMass, equalizingMass, LiquidFreeCapacity(target.x, target.y));
                    if (amount <= 0.001f)
                    {
                        continue;
                    }

                    float moved = MoveWorldLiquidMass(new Vector2Int(x, y), target, amount);
                    if (moved > 0.001f)
                    {
                        movedMass += moved;
                        movedEvents++;
                    }
                }
            }

            if (movedMass <= 0.001f)
            {
                return false;
            }

            liquidFlowedMass += movedMass;
            liquidFlowEvents += movedEvents;
            return true;
        }

        private bool TryFindLiquidSideTarget(int x, int y, out Vector2Int target)
        {
            target = new Vector2Int(-1, -1);
            float sourceMass = LiquidMassAt(x, y);
            float bestMass = sourceMass;
            int firstDirection = ((x + y + liquidFlowEvents) & 1) == 0 ? -1 : 1;
            int secondDirection = -firstDirection;

            if (CanLiquidSpreadTo(x, y, x + firstDirection, y))
            {
                bestMass = LiquidMassAt(x + firstDirection, y);
                target = new Vector2Int(x + firstDirection, y);
            }

            if (CanLiquidSpreadTo(x, y, x + secondDirection, y))
            {
                float secondMass = LiquidMassAt(x + secondDirection, y);
                if (target.x < 0 || secondMass < bestMass - 0.001f)
                {
                    target = new Vector2Int(x + secondDirection, y);
                }
            }

            return target.x >= 0;
        }

        private float MoveWorldLiquidMass(Vector2Int from, Vector2Int to, float requestedAmount)
        {
            if (!IsInside(from.x, from.y) ||
                !IsInside(to.x, to.y) ||
                !HasWorldLiquidAt(from.x, from.y) ||
                !CanLiquidOccupy(to.x, to.y) ||
                requestedAmount <= 0.001f)
            {
                return 0f;
            }

            float sourceMass = waterMass[from.x, from.y];
            float targetMass = LiquidMassAt(to.x, to.y);
            float amount = Mathf.Min(requestedAmount, sourceMass, LiquidTileCapacity - targetMass);
            if (amount <= 0.001f)
            {
                return 0f;
            }

            bool targetWasEmpty = cells[to.x, to.y] == CellKind.Empty;
            float displacedOxygen = oxygen[to.x, to.y];
            float displacedCarbon = carbonDioxide[to.x, to.y];
            float displacedPolluted = pollutedOxygen[to.x, to.y];
            float displacedHydrogen = hydrogen[to.x, to.y];
            float displacedChlorine = chlorine[to.x, to.y];
            float displacedNaturalGas = naturalGas[to.x, to.y];
            float displacedGerms = germs[to.x, to.y];
            float sourceTemperature = temperature[from.x, from.y];
            float targetTemperature = temperature[to.x, to.y];
            float sourceGerms = germs[from.x, from.y];
            float targetGerms = targetWasEmpty ? 0f : germs[to.x, to.y];

            EnsureWaterCell(to);
            waterMass[from.x, from.y] = Mathf.Max(0f, sourceMass - amount);
            waterMass[to.x, to.y] = Mathf.Min(LiquidTileCapacity, targetMass + amount);

            float combinedMass = Mathf.Max(0.001f, targetMass + amount);
            temperature[to.x, to.y] = Mathf.Clamp(
                (targetTemperature * targetMass + sourceTemperature * amount) / combinedMass,
                -30f,
                120f);
            germs[to.x, to.y] = Mathf.Clamp01((targetGerms * targetMass + sourceGerms * amount) / combinedMass);
            oxygen[to.x, to.y] = 0f;
            carbonDioxide[to.x, to.y] = 0f;
            pollutedOxygen[to.x, to.y] = 0f;
            hydrogen[to.x, to.y] = 0f;
            steam[to.x, to.y] = 0f;
            chlorine[to.x, to.y] = 0f;
            naturalGas[to.x, to.y] = 0f;

            if (waterMass[from.x, from.y] <= LiquidMinimumRetainedMass)
            {
                DryWaterCell(
                    from,
                    targetWasEmpty ? displacedOxygen : NeighborAverage(oxygen, from.x, from.y, 0.12f),
                    targetWasEmpty ? displacedCarbon : NeighborAverage(carbonDioxide, from.x, from.y, 0.04f),
                    targetWasEmpty ? displacedPolluted : NeighborAverage(pollutedOxygen, from.x, from.y, 0f),
                    targetWasEmpty ? displacedHydrogen : NeighborAverage(hydrogen, from.x, from.y, 0f),
                    targetWasEmpty ? displacedChlorine : NeighborAverage(chlorine, from.x, from.y, 0f),
                    targetWasEmpty ? displacedNaturalGas : NeighborAverage(naturalGas, from.x, from.y, 0f),
                    targetWasEmpty ? displacedGerms : NeighborAverage(germs, from.x, from.y, 0f),
                    Mathf.Lerp(sourceTemperature, targetTemperature, 0.35f));
                CancelQueuedJobsAt(from);
            }

            CancelQueuedJobsAt(to);
            DisplaceWorkersFromLiquid(to);
            return amount;
        }

        private void EnsureWaterCell(Vector2Int cell)
        {
            if (cells[cell.x, cell.y] != CellKind.Empty)
            {
                return;
            }

            cells[cell.x, cell.y] = CellKind.Water;
            waterMass[cell.x, cell.y] = 0f;
            equipmentCondition[cell.x, cell.y] = 0f;
            plantGrowth[cell.x, cell.y] = 0f;
            cropTendedSeconds[cell.x, cell.y] = 0f;
            cropStress[cell.x, cell.y] = 0f;
            looseResourceKind[cell.x, cell.y] = LooseResourceKind.None;
            looseResourceAmount[cell.x, cell.y] = 0f;
        }

        private void DryWaterCell(Vector2Int cell, float restoredOxygen, float restoredCarbon, float restoredPolluted, float restoredHydrogen, float restoredChlorine, float restoredNaturalGas, float restoredGerms, float restoredTemperature)
        {
            if (cells[cell.x, cell.y] == CellKind.Water)
            {
                cells[cell.x, cell.y] = CellKind.Empty;
                equipmentCondition[cell.x, cell.y] = 0f;
            }

            waterMass[cell.x, cell.y] = 0f;
            oxygen[cell.x, cell.y] = Mathf.Max(0f, restoredOxygen);
            carbonDioxide[cell.x, cell.y] = Mathf.Max(0f, restoredCarbon);
            pollutedOxygen[cell.x, cell.y] = Mathf.Max(0f, restoredPolluted);
            hydrogen[cell.x, cell.y] = Mathf.Max(0f, restoredHydrogen);
            steam[cell.x, cell.y] = 0f;
            chlorine[cell.x, cell.y] = Mathf.Max(0f, restoredChlorine);
            naturalGas[cell.x, cell.y] = Mathf.Max(0f, restoredNaturalGas);
            germs[cell.x, cell.y] = Mathf.Clamp01(restoredGerms);
            temperature[cell.x, cell.y] = Mathf.Clamp(restoredTemperature, -30f, 120f);
        }

        private bool CanLiquidOccupy(int x, int y)
        {
            return IsInside(x, y) &&
                (cells[x, y] == CellKind.Empty ||
                    cells[x, y] == CellKind.Water ||
                    IsFloodableCellKind(cells[x, y]));
        }

        private bool HasWorldLiquidAt(int x, int y)
        {
            return IsInside(x, y) &&
                waterMass[x, y] > LiquidMinimumRetainedMass &&
                (cells[x, y] == CellKind.Water || IsFloodableCellKind(cells[x, y]));
        }

        private float LiquidMassAt(int x, int y)
        {
            return CanLiquidOccupy(x, y) ? Mathf.Max(0f, waterMass[x, y]) : 0f;
        }

        private float LiquidFreeCapacity(int x, int y)
        {
            return CanLiquidOccupy(x, y) ? Mathf.Max(0f, LiquidTileCapacity - LiquidMassAt(x, y)) : 0f;
        }

        private bool CanLiquidFlowDown(int x, int y)
        {
            return IsInside(x, y) &&
                HasWorldLiquidAt(x, y) &&
                waterMass[x, y] > LiquidMinimumRetainedMass &&
                y > 0 &&
                LiquidFreeCapacity(x, y - 1) > LiquidMinimumRetainedMass;
        }

        private bool CanLiquidSpreadSideways(int x, int y)
        {
            return IsInside(x, y) &&
                HasWorldLiquidAt(x, y) &&
                waterMass[x, y] > LiquidMinimumRetainedMass &&
                !CanLiquidFlowDown(x, y) &&
                (CanLiquidSpreadTo(x, y, x - 1, y) || CanLiquidSpreadTo(x, y, x + 1, y));
        }

        private bool CanLiquidSpreadTo(int fromX, int fromY, int toX, int toY)
        {
            return CanLiquidOccupy(toX, toY) &&
                LiquidFreeCapacity(toX, toY) > LiquidMinimumRetainedMass &&
                LiquidMassAt(fromX, fromY) > LiquidMassAt(toX, toY) + 1.5f;
        }

        private int CountFlowingLiquidTiles()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (HasWorldLiquidAt(x, y) && (CanLiquidFlowDown(x, y) || CanLiquidSpreadSideways(x, y)))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private string LiquidFlowStateText(Vector2Int cell)
        {
            if (!IsInside(cell.x, cell.y) || !HasWorldLiquidAt(cell.x, cell.y))
            {
                return string.Empty;
            }

            if (CanLiquidFlowDown(cell.x, cell.y))
            {
                return "Flowing: falling into open space below.";
            }

            if (CanLiquidSpreadSideways(cell.x, cell.y))
            {
                return "Flowing: spreading across the floor.";
            }

            return "Settled liquid.";
        }

        private bool IsFloodableCellKind(CellKind kind)
        {
            return kind == CellKind.Ladder || IsRepairableEquipment(kind);
        }

        private bool IsWaterTolerantEquipment(CellKind kind)
        {
            return kind == CellKind.WaterPump ||
                kind == CellKind.LiquidVent ||
                kind == CellKind.BottleEmptier ||
                kind == CellKind.LiquidReservoir ||
                kind == CellKind.LiquidPipeSensor ||
                kind == CellKind.LiquidShutoff;
        }

        private bool IsEquipmentSubmerged(Vector2Int cell)
        {
            return IsInside(cell.x, cell.y) &&
                IsRepairableEquipment(cells[cell.x, cell.y]) &&
                !IsWaterTolerantEquipment(cells[cell.x, cell.y]) &&
                waterMass[cell.x, cell.y] >= SubmergedEquipmentWaterMass;
        }

        private bool IsPowerWireFlooded(int x, int y)
        {
            return IsInside(x, y) &&
                powerWire[x, y] &&
                waterMass[x, y] >= FloodedWireWaterMass;
        }

        private int CountSubmergedEquipment()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (IsEquipmentSubmerged(new Vector2Int(x, y)))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountFloodedPowerWires()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (IsPowerWireFlooded(x, y))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private void DisplaceWorkersFromLiquid(Vector2Int cell)
        {
            Worker worker = WorkerAt(cell);
            if (worker == null || worker.Health <= 0f)
            {
                return;
            }

            ClearAssignment(worker);
            if (TryFindOpenSpawnNear(cell, out Vector2Int safeCell))
            {
                worker.Cell = safeCell;
                if (worker.Transform != null)
                {
                    worker.Transform.position = CellCenter(safeCell);
                }

                worker.Stress = Mathf.Min(100f, worker.Stress + 2.5f);
                worker.Activity = "Avoiding Water";
                return;
            }

            worker.Stress = Mathf.Min(100f, worker.Stress + 5f);
            worker.Health = Mathf.Max(0f, worker.Health - 1f);
            worker.Activity = "Trapped in Water";
        }

        private void SimulateVentilation(float deltaTime)
        {
            bool changed = false;
            changed |= RunGasPumps(deltaTime);
            changed |= EqualizeGasPipes(deltaTime);
            changed |= RunGasReservoirs(deltaTime);
            changed |= RunGasVents(deltaTime);

            if (changed)
            {
                gasDirty = true;
                overlayDirty = true;
            }
        }

        private bool RunGasPumps(float deltaTime)
        {
            bool changed = false;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] != CellKind.GasPump)
                    {
                        continue;
                    }

                    Vector2Int pump = new Vector2Int(x, y);
                    if (IsBrokenEquipment(pump) ||
                        !CanPoweredMachineRun(pump) ||
                        !TryFindGasSource(pump, out Vector2Int source) ||
                        !TryFindAdjacentGasPipeWithSpace(pump, out Vector2Int pipe))
                    {
                        continue;
                    }

                    float sourceTotal = TilePumpableGasTotal(source.x, source.y);
                    float pipeFree = GasPipeCapacity - GasPipeTotal(pipe.x, pipe.y);
                    float powerUsed = Mathf.Min(power, 0.20f * deltaTime);
                    float efficiency = powerUsed / Mathf.Max(0.001f, 0.20f * deltaTime);
                    float amount = Mathf.Min(sourceTotal, Mathf.Min(pipeFree, GasPumpRate * deltaTime * efficiency));
                    if (amount <= 0.001f)
                    {
                        continue;
                    }

                    MoveTileGasToPipe(source, pipe, amount);
                    power -= powerUsed;
                    AddHeat(pump, 0.025f * deltaTime * efficiency, 1);
                    WearEquipment(pump, 0.00028f * deltaTime * efficiency);
                    changed = true;
                }
            }

            return changed;
        }

        private bool EqualizeGasPipes(float deltaTime)
        {
            bool changed = false;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (!gasPipe[x, y])
                    {
                        continue;
                    }

                    changed |= EqualizeGasPipePair(x, y, x + 1, y, deltaTime);
                    changed |= EqualizeGasPipePair(x, y, x, y + 1, deltaTime);
                }
            }

            return changed;
        }

        private bool EqualizeGasPipePair(int ax, int ay, int bx, int by, float deltaTime)
        {
            if (!IsInside(bx, by) ||
                !gasPipe[ax, ay] ||
                !gasPipe[bx, by] ||
                IsGasConduitBlocked(ax, ay) ||
                IsGasConduitBlocked(bx, by))
            {
                return false;
            }

            float aTotal = GasPipeTotal(ax, ay);
            float bTotal = GasPipeTotal(bx, by);
            float difference = aTotal - bTotal;
            if (Mathf.Abs(difference) < 0.01f)
            {
                return false;
            }

            float flow = Mathf.Clamp(difference * 0.45f * deltaTime, -GasPipeCapacity * 0.45f, GasPipeCapacity * 0.45f);
            if (flow > 0f)
            {
                flow = Mathf.Min(flow, aTotal, GasPipeCapacity - bTotal);
                MoveGasPipeToPipe(ax, ay, bx, by, flow);
            }
            else
            {
                float reverse = Mathf.Min(-flow, bTotal, GasPipeCapacity - aTotal);
                MoveGasPipeToPipe(bx, by, ax, ay, reverse);
                flow = -reverse;
            }

            float moved = Mathf.Abs(flow);
            if (moved > 0.001f)
            {
                TrackAutomatedConduitFlow(ax, ay, bx, by, moved, true);
            }

            return moved > 0.001f;
        }

        private bool RunGasVents(float deltaTime)
        {
            bool changed = false;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] != CellKind.GasVent)
                    {
                        continue;
                    }

                    Vector2Int vent = new Vector2Int(x, y);
                    if (IsBrokenEquipment(vent) ||
                        !TryFindAdjacentGasPipeWithGas(vent, out Vector2Int pipe) ||
                        !TryFindGasVentOutput(vent, out Vector2Int output))
                    {
                        continue;
                    }

                    float outputFree = 2.8f - TileGasTotal(output.x, output.y);
                    float amount = Mathf.Min(GasPipeTotal(pipe.x, pipe.y), Mathf.Min(GasVentRate * deltaTime, outputFree));
                    if (amount <= 0.001f)
                    {
                        continue;
                    }

                    MoveGasPipeToTile(pipe, output, amount);
                    WearEquipment(vent, 0.00012f * deltaTime);
                    changed = true;
                }
            }

            return changed;
        }

        private bool RunGasReservoirs(float deltaTime)
        {
            bool changed = false;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] != CellKind.GasReservoir)
                    {
                        continue;
                    }

                    Vector2Int reservoir = new Vector2Int(x, y);
                    if (IsBrokenEquipment(reservoir))
                    {
                        continue;
                    }

                    Vector2Int inputPipe = new Vector2Int(-1, -1);
                    float stored = GasReservoirTotal(x, y);
                    if (stored < GasReservoirCapacity - 0.001f &&
                        TryFindAdjacentGasPipeWithGas(reservoir, out inputPipe))
                    {
                        float amount = Mathf.Min(
                            GasPipeTotal(inputPipe.x, inputPipe.y),
                            Mathf.Min(GasReservoirRate * deltaTime, GasReservoirCapacity - stored));
                        if (amount > 0.001f)
                        {
                            MoveGasPipeToReservoir(inputPipe, reservoir, amount);
                            stored = GasReservoirTotal(x, y);
                            reservoirBufferedMass += amount;
                            WearEquipment(reservoir, 0.00008f * deltaTime);
                            changed = true;
                        }
                    }

                    if (stored > 0.001f &&
                        TryFindAdjacentGasPipeWithSpaceExcluding(reservoir, inputPipe, out Vector2Int outputPipe))
                    {
                        float amount = Mathf.Min(
                            stored,
                            Mathf.Min(GasReservoirRate * deltaTime, GasPipeCapacity - GasPipeTotal(outputPipe.x, outputPipe.y)));
                        if (amount > 0.001f)
                        {
                            MoveGasReservoirToPipe(reservoir, outputPipe, amount);
                            WearEquipment(reservoir, 0.00006f * deltaTime);
                            changed = true;
                        }
                    }
                }
            }

            return changed;
        }

        private bool IsLiquidConduitBlocked(int x, int y)
        {
            return cells[x, y] == CellKind.LiquidShutoff && !IsConduitShutoffOpen(new Vector2Int(x, y));
        }

        private bool IsGasConduitBlocked(int x, int y)
        {
            return cells[x, y] == CellKind.GasShutoff && !IsConduitShutoffOpen(new Vector2Int(x, y));
        }

        private bool IsConduitShutoffOpen(Vector2Int cell)
        {
            if (IsBrokenEquipment(cell))
            {
                return false;
            }

            if (!HasCachedAutomationControl(cell))
            {
                return true;
            }

            return HasCachedAutomationSignal(cell);
        }

        private void TrackAutomatedConduitFlow(int ax, int ay, int bx, int by, float amount, bool gasConduit)
        {
            bool controlled = false;
            if (gasConduit)
            {
                controlled |= TrackAutomatedConduitEndpoint(ax, ay, CellKind.GasShutoff, amount);
                controlled |= TrackAutomatedConduitEndpoint(bx, by, CellKind.GasShutoff, amount);
            }
            else
            {
                controlled |= TrackAutomatedConduitEndpoint(ax, ay, CellKind.LiquidShutoff, amount);
                controlled |= TrackAutomatedConduitEndpoint(bx, by, CellKind.LiquidShutoff, amount);
            }

            if (controlled)
            {
                automatedConduitFlow += amount;
            }
        }

        private bool TrackAutomatedConduitEndpoint(int x, int y, CellKind shutoffKind, float amount)
        {
            if (cells[x, y] != shutoffKind || amount <= 0.001f || !HasCachedAutomationControl(new Vector2Int(x, y)))
            {
                return false;
            }

            WearEquipment(new Vector2Int(x, y), 0.00005f * amount);
            return true;
        }

        private void MoveTileGasToPipe(Vector2Int source, Vector2Int pipe, float amount)
        {
            float sourceTotal = TilePumpableGasTotal(source.x, source.y);
            if (sourceTotal <= 0.001f)
            {
                return;
            }

            float oxygenAmount = Mathf.Min(oxygen[source.x, source.y], amount * oxygen[source.x, source.y] / sourceTotal);
            float carbonAmount = Mathf.Min(carbonDioxide[source.x, source.y], amount * carbonDioxide[source.x, source.y] / sourceTotal);
            float pollutedAmount = Mathf.Min(pollutedOxygen[source.x, source.y], amount * pollutedOxygen[source.x, source.y] / sourceTotal);
            float hydrogenAmount = Mathf.Min(hydrogen[source.x, source.y], amount * hydrogen[source.x, source.y] / sourceTotal);
            float chlorineAmount = Mathf.Min(chlorine[source.x, source.y], amount * chlorine[source.x, source.y] / sourceTotal);
            float naturalGasAmount = Mathf.Min(naturalGas[source.x, source.y], amount * naturalGas[source.x, source.y] / sourceTotal);
            AddGasToPipe(pipe.x, pipe.y, oxygenAmount, carbonAmount, pollutedAmount, hydrogenAmount, chlorineAmount, naturalGasAmount, pollutedAmount > 0.001f ? germs[source.x, source.y] : 0f);
            oxygen[source.x, source.y] = Mathf.Max(0f, oxygen[source.x, source.y] - oxygenAmount);
            carbonDioxide[source.x, source.y] = Mathf.Max(0f, carbonDioxide[source.x, source.y] - carbonAmount);
            pollutedOxygen[source.x, source.y] = Mathf.Max(0f, pollutedOxygen[source.x, source.y] - pollutedAmount);
            hydrogen[source.x, source.y] = Mathf.Max(0f, hydrogen[source.x, source.y] - hydrogenAmount);
            chlorine[source.x, source.y] = Mathf.Max(0f, chlorine[source.x, source.y] - chlorineAmount);
            naturalGas[source.x, source.y] = Mathf.Max(0f, naturalGas[source.x, source.y] - naturalGasAmount);
            if (pollutedOxygen[source.x, source.y] <= 0.01f)
            {
                germs[source.x, source.y] = Mathf.Max(0f, germs[source.x, source.y] - 0.08f);
            }
        }

        private void MoveGasPipeToPipe(int fromX, int fromY, int toX, int toY, float amount)
        {
            float fromTotal = GasPipeTotal(fromX, fromY);
            if (fromTotal <= 0.001f || amount <= 0.001f)
            {
                return;
            }

            float oxygenAmount = Mathf.Min(gasPipeOxygen[fromX, fromY], amount * gasPipeOxygen[fromX, fromY] / fromTotal);
            float carbonAmount = Mathf.Min(gasPipeCarbonDioxide[fromX, fromY], amount * gasPipeCarbonDioxide[fromX, fromY] / fromTotal);
            float pollutedAmount = Mathf.Min(gasPipePollutedOxygen[fromX, fromY], amount * gasPipePollutedOxygen[fromX, fromY] / fromTotal);
            float hydrogenAmount = Mathf.Min(gasPipeHydrogen[fromX, fromY], amount * gasPipeHydrogen[fromX, fromY] / fromTotal);
            float chlorineAmount = Mathf.Min(gasPipeChlorine[fromX, fromY], amount * gasPipeChlorine[fromX, fromY] / fromTotal);
            float naturalGasAmount = Mathf.Min(gasPipeNaturalGas[fromX, fromY], amount * gasPipeNaturalGas[fromX, fromY] / fromTotal);
            AddGasToPipe(toX, toY, oxygenAmount, carbonAmount, pollutedAmount, hydrogenAmount, chlorineAmount, naturalGasAmount, pollutedAmount > 0.001f ? gasPipeGerms[fromX, fromY] : 0f);
            gasPipeOxygen[fromX, fromY] = Mathf.Max(0f, gasPipeOxygen[fromX, fromY] - oxygenAmount);
            gasPipeCarbonDioxide[fromX, fromY] = Mathf.Max(0f, gasPipeCarbonDioxide[fromX, fromY] - carbonAmount);
            gasPipePollutedOxygen[fromX, fromY] = Mathf.Max(0f, gasPipePollutedOxygen[fromX, fromY] - pollutedAmount);
            gasPipeHydrogen[fromX, fromY] = Mathf.Max(0f, gasPipeHydrogen[fromX, fromY] - hydrogenAmount);
            gasPipeChlorine[fromX, fromY] = Mathf.Max(0f, gasPipeChlorine[fromX, fromY] - chlorineAmount);
            gasPipeNaturalGas[fromX, fromY] = Mathf.Max(0f, gasPipeNaturalGas[fromX, fromY] - naturalGasAmount);
            if (gasPipePollutedOxygen[fromX, fromY] <= 0.001f)
            {
                gasPipeGerms[fromX, fromY] = 0f;
            }
        }

        private void MoveGasPipeToTile(Vector2Int pipe, Vector2Int output, float amount)
        {
            float pipeTotal = GasPipeTotal(pipe.x, pipe.y);
            if (pipeTotal <= 0.001f || amount <= 0.001f)
            {
                return;
            }

            float oxygenAmount = Mathf.Min(gasPipeOxygen[pipe.x, pipe.y], amount * gasPipeOxygen[pipe.x, pipe.y] / pipeTotal);
            float carbonAmount = Mathf.Min(gasPipeCarbonDioxide[pipe.x, pipe.y], amount * gasPipeCarbonDioxide[pipe.x, pipe.y] / pipeTotal);
            float pollutedAmount = Mathf.Min(gasPipePollutedOxygen[pipe.x, pipe.y], amount * gasPipePollutedOxygen[pipe.x, pipe.y] / pipeTotal);
            float hydrogenAmount = Mathf.Min(gasPipeHydrogen[pipe.x, pipe.y], amount * gasPipeHydrogen[pipe.x, pipe.y] / pipeTotal);
            float chlorineAmount = Mathf.Min(gasPipeChlorine[pipe.x, pipe.y], amount * gasPipeChlorine[pipe.x, pipe.y] / pipeTotal);
            float naturalGasAmount = Mathf.Min(gasPipeNaturalGas[pipe.x, pipe.y], amount * gasPipeNaturalGas[pipe.x, pipe.y] / pipeTotal);
            AddGasToTile(output.x, output.y, oxygenAmount, carbonAmount, pollutedAmount, hydrogenAmount, chlorineAmount, naturalGasAmount, pollutedAmount > 0.001f ? gasPipeGerms[pipe.x, pipe.y] : 0f);
            gasPipeOxygen[pipe.x, pipe.y] = Mathf.Max(0f, gasPipeOxygen[pipe.x, pipe.y] - oxygenAmount);
            gasPipeCarbonDioxide[pipe.x, pipe.y] = Mathf.Max(0f, gasPipeCarbonDioxide[pipe.x, pipe.y] - carbonAmount);
            gasPipePollutedOxygen[pipe.x, pipe.y] = Mathf.Max(0f, gasPipePollutedOxygen[pipe.x, pipe.y] - pollutedAmount);
            gasPipeHydrogen[pipe.x, pipe.y] = Mathf.Max(0f, gasPipeHydrogen[pipe.x, pipe.y] - hydrogenAmount);
            gasPipeChlorine[pipe.x, pipe.y] = Mathf.Max(0f, gasPipeChlorine[pipe.x, pipe.y] - chlorineAmount);
            gasPipeNaturalGas[pipe.x, pipe.y] = Mathf.Max(0f, gasPipeNaturalGas[pipe.x, pipe.y] - naturalGasAmount);
            if (gasPipePollutedOxygen[pipe.x, pipe.y] <= 0.001f)
            {
                gasPipeGerms[pipe.x, pipe.y] = 0f;
            }
        }

        private void MoveGasPipeToReservoir(Vector2Int pipe, Vector2Int reservoir, float amount)
        {
            float pipeTotal = GasPipeTotal(pipe.x, pipe.y);
            float free = GasReservoirCapacity - GasReservoirTotal(reservoir.x, reservoir.y);
            amount = Mathf.Min(amount, Mathf.Min(pipeTotal, free));
            if (pipeTotal <= 0.001f || amount <= 0.001f)
            {
                return;
            }

            float oxygenAmount = Mathf.Min(gasPipeOxygen[pipe.x, pipe.y], amount * gasPipeOxygen[pipe.x, pipe.y] / pipeTotal);
            float carbonAmount = Mathf.Min(gasPipeCarbonDioxide[pipe.x, pipe.y], amount * gasPipeCarbonDioxide[pipe.x, pipe.y] / pipeTotal);
            float pollutedAmount = Mathf.Min(gasPipePollutedOxygen[pipe.x, pipe.y], amount * gasPipePollutedOxygen[pipe.x, pipe.y] / pipeTotal);
            float hydrogenAmount = Mathf.Min(gasPipeHydrogen[pipe.x, pipe.y], amount * gasPipeHydrogen[pipe.x, pipe.y] / pipeTotal);
            float chlorineAmount = Mathf.Min(gasPipeChlorine[pipe.x, pipe.y], amount * gasPipeChlorine[pipe.x, pipe.y] / pipeTotal);
            float naturalGasAmount = Mathf.Min(gasPipeNaturalGas[pipe.x, pipe.y], amount * gasPipeNaturalGas[pipe.x, pipe.y] / pipeTotal);
            AddGasToReservoir(reservoir.x, reservoir.y, oxygenAmount, carbonAmount, pollutedAmount, hydrogenAmount, chlorineAmount, naturalGasAmount, pollutedAmount > 0.001f ? gasPipeGerms[pipe.x, pipe.y] : 0f);
            gasPipeOxygen[pipe.x, pipe.y] = Mathf.Max(0f, gasPipeOxygen[pipe.x, pipe.y] - oxygenAmount);
            gasPipeCarbonDioxide[pipe.x, pipe.y] = Mathf.Max(0f, gasPipeCarbonDioxide[pipe.x, pipe.y] - carbonAmount);
            gasPipePollutedOxygen[pipe.x, pipe.y] = Mathf.Max(0f, gasPipePollutedOxygen[pipe.x, pipe.y] - pollutedAmount);
            gasPipeHydrogen[pipe.x, pipe.y] = Mathf.Max(0f, gasPipeHydrogen[pipe.x, pipe.y] - hydrogenAmount);
            gasPipeChlorine[pipe.x, pipe.y] = Mathf.Max(0f, gasPipeChlorine[pipe.x, pipe.y] - chlorineAmount);
            gasPipeNaturalGas[pipe.x, pipe.y] = Mathf.Max(0f, gasPipeNaturalGas[pipe.x, pipe.y] - naturalGasAmount);
            if (gasPipePollutedOxygen[pipe.x, pipe.y] <= 0.001f)
            {
                gasPipeGerms[pipe.x, pipe.y] = 0f;
            }
        }

        private void MoveGasReservoirToPipe(Vector2Int reservoir, Vector2Int pipe, float amount)
        {
            float reservoirTotal = GasReservoirTotal(reservoir.x, reservoir.y);
            float pipeFree = GasPipeCapacity - GasPipeTotal(pipe.x, pipe.y);
            amount = Mathf.Min(amount, Mathf.Min(reservoirTotal, pipeFree));
            if (reservoirTotal <= 0.001f || amount <= 0.001f)
            {
                return;
            }

            float oxygenAmount = Mathf.Min(gasReservoirOxygen[reservoir.x, reservoir.y], amount * gasReservoirOxygen[reservoir.x, reservoir.y] / reservoirTotal);
            float carbonAmount = Mathf.Min(gasReservoirCarbonDioxide[reservoir.x, reservoir.y], amount * gasReservoirCarbonDioxide[reservoir.x, reservoir.y] / reservoirTotal);
            float pollutedAmount = Mathf.Min(gasReservoirPollutedOxygen[reservoir.x, reservoir.y], amount * gasReservoirPollutedOxygen[reservoir.x, reservoir.y] / reservoirTotal);
            float hydrogenAmount = Mathf.Min(gasReservoirHydrogen[reservoir.x, reservoir.y], amount * gasReservoirHydrogen[reservoir.x, reservoir.y] / reservoirTotal);
            float chlorineAmount = Mathf.Min(gasReservoirChlorine[reservoir.x, reservoir.y], amount * gasReservoirChlorine[reservoir.x, reservoir.y] / reservoirTotal);
            float naturalGasAmount = Mathf.Min(gasReservoirNaturalGas[reservoir.x, reservoir.y], amount * gasReservoirNaturalGas[reservoir.x, reservoir.y] / reservoirTotal);
            AddGasToPipe(pipe.x, pipe.y, oxygenAmount, carbonAmount, pollutedAmount, hydrogenAmount, chlorineAmount, naturalGasAmount, pollutedAmount > 0.001f ? gasReservoirGerms[reservoir.x, reservoir.y] : 0f);
            gasReservoirOxygen[reservoir.x, reservoir.y] = Mathf.Max(0f, gasReservoirOxygen[reservoir.x, reservoir.y] - oxygenAmount);
            gasReservoirCarbonDioxide[reservoir.x, reservoir.y] = Mathf.Max(0f, gasReservoirCarbonDioxide[reservoir.x, reservoir.y] - carbonAmount);
            gasReservoirPollutedOxygen[reservoir.x, reservoir.y] = Mathf.Max(0f, gasReservoirPollutedOxygen[reservoir.x, reservoir.y] - pollutedAmount);
            gasReservoirHydrogen[reservoir.x, reservoir.y] = Mathf.Max(0f, gasReservoirHydrogen[reservoir.x, reservoir.y] - hydrogenAmount);
            gasReservoirChlorine[reservoir.x, reservoir.y] = Mathf.Max(0f, gasReservoirChlorine[reservoir.x, reservoir.y] - chlorineAmount);
            gasReservoirNaturalGas[reservoir.x, reservoir.y] = Mathf.Max(0f, gasReservoirNaturalGas[reservoir.x, reservoir.y] - naturalGasAmount);
            if (gasReservoirPollutedOxygen[reservoir.x, reservoir.y] <= 0.001f)
            {
                gasReservoirGerms[reservoir.x, reservoir.y] = 0f;
            }
        }

        private void AddGasToPipe(int x, int y, float oxygenAmount, float carbonAmount, float pollutedAmount, float hydrogenAmount, float chlorineAmount, float naturalGasAmount, float germLevel)
        {
            float free = GasPipeCapacity - GasPipeTotal(x, y);
            float amount = oxygenAmount + carbonAmount + pollutedAmount + hydrogenAmount + chlorineAmount + naturalGasAmount;
            if (free <= 0f || amount <= 0f)
            {
                return;
            }

            float scale = Mathf.Min(1f, free / amount);
            float oldPolluted = gasPipePollutedOxygen[x, y];
            float addedPolluted = pollutedAmount * scale;
            gasPipeOxygen[x, y] += oxygenAmount * scale;
            gasPipeCarbonDioxide[x, y] += carbonAmount * scale;
            gasPipePollutedOxygen[x, y] += addedPolluted;
            gasPipeHydrogen[x, y] += hydrogenAmount * scale;
            gasPipeChlorine[x, y] += chlorineAmount * scale;
            gasPipeNaturalGas[x, y] += naturalGasAmount * scale;
            if (addedPolluted > 0.001f)
            {
                gasPipeGerms[x, y] = Mathf.Clamp01((gasPipeGerms[x, y] * oldPolluted + germLevel * addedPolluted) / Mathf.Max(0.001f, oldPolluted + addedPolluted));
            }
        }

        private void AddGasToReservoir(int x, int y, float oxygenAmount, float carbonAmount, float pollutedAmount, float hydrogenAmount, float chlorineAmount, float naturalGasAmount, float germLevel)
        {
            float free = GasReservoirCapacity - GasReservoirTotal(x, y);
            float amount = oxygenAmount + carbonAmount + pollutedAmount + hydrogenAmount + chlorineAmount + naturalGasAmount;
            if (free <= 0f || amount <= 0f)
            {
                return;
            }

            float scale = Mathf.Min(1f, free / amount);
            float oldPolluted = gasReservoirPollutedOxygen[x, y];
            float addedPolluted = pollutedAmount * scale;
            gasReservoirOxygen[x, y] += oxygenAmount * scale;
            gasReservoirCarbonDioxide[x, y] += carbonAmount * scale;
            gasReservoirPollutedOxygen[x, y] += addedPolluted;
            gasReservoirHydrogen[x, y] += hydrogenAmount * scale;
            gasReservoirChlorine[x, y] += chlorineAmount * scale;
            gasReservoirNaturalGas[x, y] += naturalGasAmount * scale;
            if (addedPolluted > 0.001f)
            {
                gasReservoirGerms[x, y] = Mathf.Clamp01((gasReservoirGerms[x, y] * oldPolluted + germLevel * addedPolluted) / Mathf.Max(0.001f, oldPolluted + addedPolluted));
            }
        }

        private void AddGasToTile(int x, int y, float oxygenAmount, float carbonAmount, float pollutedAmount, float hydrogenAmount, float chlorineAmount, float naturalGasAmount, float germLevel)
        {
            oxygen[x, y] = Mathf.Min(2.8f, oxygen[x, y] + oxygenAmount);
            carbonDioxide[x, y] = Mathf.Min(2.8f, carbonDioxide[x, y] + carbonAmount);
            pollutedOxygen[x, y] = Mathf.Min(2.8f, pollutedOxygen[x, y] + pollutedAmount);
            hydrogen[x, y] = Mathf.Min(2.8f, hydrogen[x, y] + hydrogenAmount);
            chlorine[x, y] = Mathf.Min(2.8f, chlorine[x, y] + chlorineAmount);
            naturalGas[x, y] = Mathf.Min(2.8f, naturalGas[x, y] + naturalGasAmount);
            if (pollutedAmount > 0.001f)
            {
                germs[x, y] = Mathf.Clamp01(Mathf.Max(germs[x, y], germLevel * 0.85f));
            }
        }

        private void ReleaseGasPipeContents(Vector2Int pipe)
        {
            if (!IsInside(pipe.x, pipe.y) || !IsPassable(pipe.x, pipe.y))
            {
                return;
            }

            AddGasToTile(
                pipe.x,
                pipe.y,
                gasPipeOxygen[pipe.x, pipe.y],
                gasPipeCarbonDioxide[pipe.x, pipe.y],
                gasPipePollutedOxygen[pipe.x, pipe.y],
                gasPipeHydrogen[pipe.x, pipe.y],
                gasPipeChlorine[pipe.x, pipe.y],
                gasPipeNaturalGas[pipe.x, pipe.y],
                gasPipeGerms[pipe.x, pipe.y]);
        }

        private float VentPollutedOxygen(Vector2Int center, float amount, float germAmount)
        {
            float emitted = 0f;
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    int x = center.x + dx;
                    int y = center.y + dy;
                    if (IsInside(x, y) && IsPassable(x, y))
                    {
                        float before = pollutedOxygen[x, y];
                        pollutedOxygen[x, y] = Mathf.Min(2.4f, pollutedOxygen[x, y] + amount);
                        emitted += Mathf.Max(0f, pollutedOxygen[x, y] - before);
                        germs[x, y] = Mathf.Min(1f, germs[x, y] + germAmount);
                    }
                }
            }

            return emitted;
        }

        private void RunAirDeodorizer(Vector2Int center, float deltaTime)
        {
            float powerUsed = Mathf.Min(power, 0.45f * deltaTime);
            float dirtUsed = Mathf.Min(dirt, 0.025f * deltaTime);
            float efficiency = Mathf.Min(powerUsed / Mathf.Max(0.001f, 0.45f * deltaTime), dirtUsed / Mathf.Max(0.001f, 0.025f * deltaTime));
            power -= powerUsed * efficiency;
            dirt -= dirtUsed * efficiency;
            WearEquipment(center, 0.00026f * deltaTime * efficiency);

            for (int dy = -2; dy <= 2; dy++)
            {
                for (int dx = -2; dx <= 2; dx++)
                {
                    int x = center.x + dx;
                    int y = center.y + dy;
                    if (!IsInside(x, y) || !IsPassable(x, y))
                    {
                        continue;
                    }

                    float cleaned = Mathf.Min(pollutedOxygen[x, y], 0.45f * deltaTime * efficiency);
                    pollutedOxygen[x, y] -= cleaned;
                    oxygen[x, y] = Mathf.Min(2.4f, oxygen[x, y] + cleaned * 0.72f);
                    germs[x, y] = Mathf.Max(0f, germs[x, y] - 0.65f * deltaTime * efficiency);
                }
            }
        }

        private void RunElectrolyzer(Vector2Int center, float deltaTime)
        {
            float targetWater = ElectrolyzerWaterRate * deltaTime;
            float waterUsed = ConsumeMachineWater(center, targetWater);
            if (waterUsed <= 0.001f)
            {
                return;
            }

            float targetPower = ElectrolyzerPowerRate * deltaTime * (waterUsed / Mathf.Max(0.001f, targetWater));
            float powerUsed = Mathf.Min(power, targetPower);
            float efficiency = powerUsed / Mathf.Max(0.001f, targetPower);
            power -= powerUsed;
            AddOxygen(center, ElectrolyzerOxygenRate * deltaTime * efficiency);
            AddHydrogen(center, ElectrolyzerHydrogenRate * deltaTime * efficiency);
            AddHeat(center, 0.42f * deltaTime * efficiency, 2);
            WearEquipment(center, 0.00042f * deltaTime * efficiency);
        }

        private void RunHydrogenFilter(Vector2Int center, float deltaTime)
        {
            if (!TryFindAdjacentMixedHydrogenPipe(center, out Vector2Int pipe) ||
                !TryFindHydrogenFilterOutput(center, out Vector2Int output))
            {
                return;
            }

            float nonHydrogen = PipeNonHydrogenTotal(pipe.x, pipe.y);
            float outputFree = Mathf.Max(0f, 2.8f - TileGasTotal(output.x, output.y));
            float target = Mathf.Min(nonHydrogen, Mathf.Min(outputFree, HydrogenFilterRate * deltaTime));
            if (target <= 0.001f)
            {
                return;
            }

            float targetPower = HydrogenFilterPowerRate * deltaTime * (target / Mathf.Max(0.001f, HydrogenFilterRate * deltaTime));
            float powerUsed = Mathf.Min(power, targetPower);
            float efficiency = powerUsed / Mathf.Max(0.001f, targetPower);
            float filtered = RemoveNonHydrogenFromPipe(pipe, output, target * efficiency);
            if (filtered <= 0.001f)
            {
                return;
            }

            power -= powerUsed;
            hydrogenFilteredGas += filtered;
            AddHeat(center, 0.06f * deltaTime * efficiency, 1);
            WearEquipment(center, 0.00030f * deltaTime * efficiency);
            gasDirty = true;
            overlayDirty = true;
            terrainDirty = true;
        }

        private float RemoveNonHydrogenFromPipe(Vector2Int pipe, Vector2Int output, float amount)
        {
            float nonHydrogen = PipeNonHydrogenTotal(pipe.x, pipe.y);
            if (amount <= 0.001f || nonHydrogen <= 0.001f)
            {
                return 0f;
            }

            float oxygenAmount = Mathf.Min(gasPipeOxygen[pipe.x, pipe.y], amount * gasPipeOxygen[pipe.x, pipe.y] / nonHydrogen);
            float carbonAmount = Mathf.Min(gasPipeCarbonDioxide[pipe.x, pipe.y], amount * gasPipeCarbonDioxide[pipe.x, pipe.y] / nonHydrogen);
            float pollutedAmount = Mathf.Min(gasPipePollutedOxygen[pipe.x, pipe.y], amount * gasPipePollutedOxygen[pipe.x, pipe.y] / nonHydrogen);
            float chlorineAmount = Mathf.Min(gasPipeChlorine[pipe.x, pipe.y], amount * gasPipeChlorine[pipe.x, pipe.y] / nonHydrogen);
            float naturalGasAmount = Mathf.Min(gasPipeNaturalGas[pipe.x, pipe.y], amount * gasPipeNaturalGas[pipe.x, pipe.y] / nonHydrogen);
            AddGasToTile(output.x, output.y, oxygenAmount, carbonAmount, pollutedAmount, 0f, chlorineAmount, naturalGasAmount, pollutedAmount > 0.001f ? gasPipeGerms[pipe.x, pipe.y] : 0f);
            gasPipeOxygen[pipe.x, pipe.y] = Mathf.Max(0f, gasPipeOxygen[pipe.x, pipe.y] - oxygenAmount);
            gasPipeCarbonDioxide[pipe.x, pipe.y] = Mathf.Max(0f, gasPipeCarbonDioxide[pipe.x, pipe.y] - carbonAmount);
            gasPipePollutedOxygen[pipe.x, pipe.y] = Mathf.Max(0f, gasPipePollutedOxygen[pipe.x, pipe.y] - pollutedAmount);
            gasPipeChlorine[pipe.x, pipe.y] = Mathf.Max(0f, gasPipeChlorine[pipe.x, pipe.y] - chlorineAmount);
            gasPipeNaturalGas[pipe.x, pipe.y] = Mathf.Max(0f, gasPipeNaturalGas[pipe.x, pipe.y] - naturalGasAmount);
            if (gasPipePollutedOxygen[pipe.x, pipe.y] <= 0.001f)
            {
                gasPipeGerms[pipe.x, pipe.y] = 0f;
            }

            return oxygenAmount + carbonAmount + pollutedAmount + chlorineAmount + naturalGasAmount;
        }

        private void RunCarbonSkimmer(Vector2Int center, float deltaTime)
        {
            float localCarbon = CarbonDioxideAround(center, 3);
            if (localCarbon <= 0.05f)
            {
                return;
            }

            float targetWater = CarbonSkimmerWaterRate * deltaTime;
            float waterUsed = ConsumeMachineWater(center, targetWater);
            if (waterUsed <= 0.001f)
            {
                return;
            }

            float targetPower = CarbonSkimmerPowerRate * deltaTime * (waterUsed / Mathf.Max(0.001f, targetWater));
            float powerUsed = Mathf.Min(power, targetPower);
            float efficiency = powerUsed / Mathf.Max(0.001f, targetPower);
            float cleaned = RemoveCarbonDioxide(center, CarbonSkimmerCarbonRate * deltaTime * efficiency, 3);
            power -= powerUsed;
            pollutedWater += waterUsed * Mathf.Clamp01(cleaned / Mathf.Max(0.001f, CarbonSkimmerCarbonRate * deltaTime));
            AddHeat(center, 0.12f * deltaTime * efficiency, 1);
            WearEquipment(center, 0.00038f * deltaTime * efficiency);
        }

        private void RunWaterSieve(Vector2Int center, float deltaTime)
        {
            if (pollutedWater <= 0.001f || dirt <= 0.001f)
            {
                return;
            }

            float targetPollutedWater = WaterSievePollutedWaterRate * deltaTime;
            float pollutedUsed = Mathf.Min(pollutedWater, targetPollutedWater);
            float dirtNeeded = WaterSieveDirtRate * deltaTime * (pollutedUsed / Mathf.Max(0.001f, targetPollutedWater));
            float dirtUsed = Mathf.Min(dirt, dirtNeeded);
            float filterEfficiency = dirtUsed / Mathf.Max(0.001f, dirtNeeded);
            float targetPower = WaterSievePowerRate * deltaTime * filterEfficiency;
            float powerUsed = Mathf.Min(power, targetPower);
            float powerEfficiency = powerUsed / Mathf.Max(0.001f, targetPower);
            float cleaned = pollutedUsed * filterEfficiency * powerEfficiency;
            if (cleaned <= 0.001f)
            {
                return;
            }

            pollutedWater = Mathf.Max(0f, pollutedWater - cleaned);
            dirt = Mathf.Max(0f, dirt - dirtUsed * powerEfficiency);
            power -= powerUsed;
            StoreCleanWater(center, cleaned * 0.92f);
            recycledWater += cleaned * 0.92f;
            VentPollutedOxygen(center, 0.004f * deltaTime, 0.015f * deltaTime);
            AddHeat(center, 0.10f * deltaTime * powerEfficiency, 1);
            WearEquipment(center, 0.00036f * deltaTime * powerEfficiency);
        }

        private float ConsumeMachineWater(Vector2Int center, float requested)
        {
            if (requested <= 0f)
            {
                return 0f;
            }

            float consumed = 0f;
            if (TryFindAdjacentLiquidPipeWithWater(center, out Vector2Int pipe))
            {
                float piped = Mathf.Min(requested, pipeWater[pipe.x, pipe.y]);
                pipeWater[pipe.x, pipe.y] -= piped;
                consumed += piped;
            }

            float remaining = requested - consumed;
            if (remaining > 0.001f && water > 0f)
            {
                float stored = Mathf.Min(remaining, water);
                water -= stored;
                consumed += stored;
            }

            return consumed;
        }

        private void StoreCleanWater(Vector2Int center, float amount)
        {
            if (amount <= 0f)
            {
                return;
            }

            if (TryFindAdjacentLiquidPipeWithSpace(center, out Vector2Int pipe))
            {
                float piped = Mathf.Min(amount, LiquidPipeCapacity - pipeWater[pipe.x, pipe.y]);
                pipeWater[pipe.x, pipe.y] += piped;
                amount -= piped;
            }

            if (amount > 0f)
            {
                water += amount;
            }
        }

        private float CarbonDioxideAround(Vector2Int center, int radius)
        {
            float total = 0f;
            for (int dy = -radius; dy <= radius; dy++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    int x = center.x + dx;
                    int y = center.y + dy;
                    if (IsInside(x, y) && IsPassable(x, y))
                    {
                        total += carbonDioxide[x, y];
                    }
                }
            }

            return total;
        }

        private float RemoveCarbonDioxide(Vector2Int center, float capacity, int radius)
        {
            float removed = 0f;
            for (int dy = -radius; dy <= radius && removed < capacity; dy++)
            {
                for (int dx = -radius; dx <= radius && removed < capacity; dx++)
                {
                    int x = center.x + dx;
                    int y = center.y + dy;
                    if (!IsInside(x, y) || !IsPassable(x, y))
                    {
                        continue;
                    }

                    float amount = Mathf.Min(carbonDioxide[x, y], capacity - removed);
                    carbonDioxide[x, y] -= amount;
                    removed += amount;
                }
            }

            return removed;
        }

        private void RunSpaceHeater(Vector2Int center, float deltaTime)
        {
            float localTemperature = AverageTemperatureAround(center, 3);
            if (localTemperature >= 24f)
            {
                return;
            }

            float powerUsed = Mathf.Min(power, 0.62f * deltaTime);
            float efficiency = powerUsed / Mathf.Max(0.001f, 0.62f * deltaTime);
            power -= powerUsed;
            AddHeat(center, 0.95f * deltaTime * efficiency, 3);
            WearEquipment(center, 0.00032f * deltaTime * efficiency);
        }

        private void RunThermoRegulator(Vector2Int center, float deltaTime)
        {
            float localTemperature = AverageTemperatureAround(center, 3);
            if (localTemperature <= 22f)
            {
                return;
            }

            float powerUsed = Mathf.Min(power, 0.85f * deltaTime);
            float efficiency = powerUsed / Mathf.Max(0.001f, 0.85f * deltaTime);
            power -= powerUsed;
            CoolArea(center, 0.8f * deltaTime * efficiency, 3);
            AddHeat(center, 0.18f * deltaTime * efficiency, 1);
            WearEquipment(center, 0.00038f * deltaTime * efficiency);
        }

        private void RunRefrigerator(Vector2Int center, float deltaTime)
        {
            float powerUsed = Mathf.Min(power, 0.32f * deltaTime);
            float efficiency = powerUsed / Mathf.Max(0.001f, 0.32f * deltaTime);
            power -= powerUsed;
            CoolArea(center, 0.12f * deltaTime * efficiency, 1);
            AddHeat(center, 0.06f * deltaTime * efficiency, 1);
            WearEquipment(center, 0.00018f * deltaTime * efficiency);
        }

        private void RunAutoSweeper(Vector2Int center, float deltaTime)
        {
            if (DryResourceFreeSpace() <= 0.01f || !TryFindAutoSweeperTarget(center, out Vector2Int target))
            {
                return;
            }

            float requestedTransfer = AutoSweeperTransferRate * deltaTime;
            float availableTransfer = Mathf.Min(requestedTransfer, Mathf.Min(looseResourceAmount[target.x, target.y], DryResourceFreeSpace()));
            if (availableTransfer <= 0.001f)
            {
                return;
            }

            float targetPower = AutoSweeperPowerRate * deltaTime * (availableTransfer / Mathf.Max(0.001f, requestedTransfer));
            float powerUsed = Mathf.Min(power, targetPower);
            float efficiency = powerUsed / Mathf.Max(0.001f, targetPower);
            float transfer = availableTransfer * efficiency;
            if (transfer <= 0.001f)
            {
                return;
            }

            LooseResourceKind resourceKind = looseResourceKind[target.x, target.y];
            float stored = StoreLooseResource(resourceKind, transfer);
            if (stored <= 0.001f)
            {
                return;
            }

            power -= powerUsed;
            looseResourceAmount[target.x, target.y] = Mathf.Max(0f, looseResourceAmount[target.x, target.y] - stored);
            if (looseResourceAmount[target.x, target.y] <= 0.05f)
            {
                looseResourceAmount[target.x, target.y] = 0f;
                looseResourceKind[target.x, target.y] = LooseResourceKind.None;
            }

            sweptResources += stored;
            autoSweptResources += stored;
            milestoneResourceLogistics |= sweptResources >= 12f;
            milestoneAutoSweeping |= techPowerRegulation && autoSweptResources >= 6f;
            AddHeat(center, 0.035f * deltaTime * efficiency, 1);
            WearEquipment(center, 0.00022f * deltaTime * efficiency);
            terrainDirty = true;
            overlayDirty = true;
        }

        private bool TryFindAutoSweeperTarget(Vector2Int center, out Vector2Int target)
        {
            target = new Vector2Int(-1, -1);
            float bestScore = -1f;
            for (int y = center.y - AutoSweeperRange; y <= center.y + AutoSweeperRange; y++)
            {
                for (int x = center.x - AutoSweeperRange; x <= center.x + AutoSweeperRange; x++)
                {
                    if (!IsInside(x, y) || !HasLooseResource(new Vector2Int(x, y)))
                    {
                        continue;
                    }

                    int distance = Mathf.Abs(center.x - x) + Mathf.Abs(center.y - y);
                    if (distance > AutoSweeperRange)
                    {
                        continue;
                    }

                    float score = AutoSweeperResourcePriority(looseResourceKind[x, y]) * 100f +
                        looseResourceAmount[x, y] +
                        (AutoSweeperRange - distance) * 0.25f;
                    if (score <= bestScore)
                    {
                        continue;
                    }

                    bestScore = score;
                    target = new Vector2Int(x, y);
                }
            }

            return target.x >= 0;
        }

        private int CountAutoSweeperTargets(Vector2Int center)
        {
            int count = 0;
            for (int y = center.y - AutoSweeperRange; y <= center.y + AutoSweeperRange; y++)
            {
                for (int x = center.x - AutoSweeperRange; x <= center.x + AutoSweeperRange; x++)
                {
                    if (!IsInside(x, y) || Mathf.Abs(center.x - x) + Mathf.Abs(center.y - y) > AutoSweeperRange)
                    {
                        continue;
                    }

                    if (HasLooseResource(new Vector2Int(x, y)))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private float AutoSweeperResourcePriority(LooseResourceKind kind)
        {
            switch (kind)
            {
                case LooseResourceKind.RefinedMetal:
                    return 6f;
                case LooseResourceKind.Metal:
                    return 5f;
                case LooseResourceKind.Coal:
                    return 4f;
                case LooseResourceKind.Algae:
                    return 3f;
                case LooseResourceKind.PollutedDirt:
                    return 2f;
                case LooseResourceKind.Dirt:
                    return 1f;
                default:
                    return 0f;
            }
        }

        private void RunCoalGenerator(Vector2Int center, float deltaTime)
        {
            if (!CanCoalGeneratorRun(center))
            {
                return;
            }

            float requestedPower = Mathf.Min(CoalGeneratorPowerRate * deltaTime, Mathf.Max(0f, maxPower - power));
            if (requestedPower <= 0.001f)
            {
                return;
            }

            float requestedCoal = CoalGeneratorCoalRate * deltaTime * (requestedPower / Mathf.Max(0.001f, CoalGeneratorPowerRate * deltaTime));
            float coalUsed = Mathf.Min(coal, requestedCoal);
            if (coalUsed <= 0.001f)
            {
                return;
            }

            float efficiency = coalUsed / Mathf.Max(0.001f, requestedCoal);
            float generated = requestedPower * efficiency;
            coal = Mathf.Max(0f, coal - coalUsed);
            power = Mathf.Min(maxPower, power + generated);
            coalPowerGenerated += generated;
            AddGasToTile(center.x, center.y, 0f, CoalGeneratorCarbonRate * deltaTime * efficiency, 0f, 0f, 0f, 0f, 0f);
            AddHeat(center, 0.42f * deltaTime * efficiency, 1);
            WearEquipment(center, 0.00052f * deltaTime * efficiency);
            gasDirty = true;
            terrainDirty = true;
        }

        private void RunHydrogenGenerator(Vector2Int center, float deltaTime)
        {
            if (!CanHydrogenGeneratorRun(center))
            {
                return;
            }

            float requestedPower = Mathf.Min(HydrogenGeneratorPowerRate * deltaTime, Mathf.Max(0f, maxPower - power));
            if (requestedPower <= 0.001f)
            {
                return;
            }

            float requestedHydrogen = HydrogenGeneratorHydrogenRate * deltaTime * (requestedPower / Mathf.Max(0.001f, HydrogenGeneratorPowerRate * deltaTime));
            float hydrogenUsed = RemoveHydrogenFuel(center, requestedHydrogen);
            if (hydrogenUsed <= 0.001f)
            {
                return;
            }

            float efficiency = hydrogenUsed / Mathf.Max(0.001f, requestedHydrogen);
            float generated = requestedPower * efficiency;
            power = Mathf.Min(maxPower, power + generated);
            hydrogenPowerGenerated += generated;
            AddHeat(center, 0.32f * deltaTime * efficiency, 1);
            WearEquipment(center, 0.00046f * deltaTime * efficiency);
            gasDirty = true;
            terrainDirty = true;
        }

        private bool CanHydrogenGeneratorRun(Vector2Int center)
        {
            if (!CanUseEquipment(center) || !HasWireAccess(center) || power >= maxPower - 1f || HydrogenFuelAvailable(center) <= 0.01f)
            {
                return false;
            }

            if (HasAutomationControl(center))
            {
                return HasAutomationSignal(center);
            }

            return power <= maxPower * 0.78f;
        }

        private void RunNaturalGasGenerator(Vector2Int center, float deltaTime)
        {
            if (!CanNaturalGasGeneratorRun(center))
            {
                return;
            }

            float requestedPower = Mathf.Min(NaturalGasGeneratorPowerRate * deltaTime, Mathf.Max(0f, maxPower - power));
            if (requestedPower <= 0.001f)
            {
                return;
            }

            float requestedGas = NaturalGasGeneratorGasRate * deltaTime * (requestedPower / Mathf.Max(0.001f, NaturalGasGeneratorPowerRate * deltaTime));
            float gasUsed = RemoveNaturalGasFuel(center, requestedGas);
            if (gasUsed <= 0.001f)
            {
                return;
            }

            float efficiency = gasUsed / Mathf.Max(0.001f, requestedGas);
            float generated = requestedPower * efficiency;
            power = Mathf.Min(maxPower, power + generated);
            naturalGasPowerGenerated += generated;
            pollutedWater += NaturalGasGeneratorPollutedWaterRate * deltaTime * efficiency;
            AddGasToTile(center.x, center.y, 0f, NaturalGasGeneratorCarbonRate * deltaTime * efficiency, 0f, 0f, 0f, 0f, 0f);
            AddHeat(center, 0.38f * deltaTime * efficiency, 1);
            WearEquipment(center, 0.00050f * deltaTime * efficiency);
            gasDirty = true;
            terrainDirty = true;
        }

        private void RunSteamTurbine(Vector2Int center, float deltaTime)
        {
            if (!CanSteamTurbineRun(center))
            {
                return;
            }

            float requestedPower = Mathf.Min(SteamTurbinePowerRate * deltaTime, Mathf.Max(0f, maxPower - power));
            if (requestedPower <= 0.001f)
            {
                return;
            }

            float requestedSteam = SteamTurbineSteamRate * deltaTime * (requestedPower / Mathf.Max(0.001f, SteamTurbinePowerRate * deltaTime));
            float steamUsed = RemoveHotSteam(center, requestedSteam, SteamTurbineRadius);
            if (steamUsed <= 0.001f)
            {
                return;
            }

            float efficiency = steamUsed / Mathf.Max(0.001f, requestedSteam);
            float generated = requestedPower * efficiency;
            float recoveredWater = steamUsed * SteamTurbineWaterYield;
            power = Mathf.Min(maxPower, power + generated);
            steamTurbinePowerGenerated += generated;
            steamTurbineWaterRecovered += recoveredWater;
            StoreCleanWater(center, recoveredWater);
            CoolArea(center, 0.28f * deltaTime * efficiency, 2);
            AddHeat(center, 0.12f * deltaTime * efficiency, 1);
            WearEquipment(center, 0.00058f * deltaTime * efficiency);
            gasDirty = true;
            terrainDirty = true;
            overlayDirty = true;
        }

        private bool CanSteamTurbineRun(Vector2Int center)
        {
            if (!CanUseEquipment(center) || !HasWireAccess(center) || power >= maxPower - 1f || HotSteamAvailable(center, SteamTurbineRadius) <= 0.03f)
            {
                return false;
            }

            if (HasAutomationControl(center))
            {
                return HasAutomationSignal(center);
            }

            return power <= maxPower * 0.78f;
        }

        private bool CanNaturalGasGeneratorRun(Vector2Int center)
        {
            if (!CanUseEquipment(center) || !HasWireAccess(center) || power >= maxPower - 1f || NaturalGasFuelAvailable(center) <= 0.01f)
            {
                return false;
            }

            if (HasAutomationControl(center))
            {
                return HasAutomationSignal(center);
            }

            return power <= maxPower * 0.78f;
        }

        private void RunSolarPanel(Vector2Int center, float deltaTime)
        {
            float irradiance = SolarIrradiance();
            if (irradiance <= 0.001f)
            {
                return;
            }

            if (!IsSolarPanelSkyExposed(center))
            {
                solarBlockedSeconds += deltaTime;
                return;
            }

            if (!CanSolarPanelRun(center))
            {
                return;
            }

            float generated = Mathf.Min(SolarPanelPowerRate * deltaTime * irradiance, Mathf.Max(0f, maxPower - power));
            if (generated <= 0.001f)
            {
                return;
            }

            power = Mathf.Min(maxPower, power + generated);
            solarPowerGenerated += generated;
            AddHeat(center, 0.018f * deltaTime * irradiance, 1);
            WearEquipment(center, 0.00018f * deltaTime * irradiance);
            terrainDirty = true;
            overlayDirty = true;
        }

        private bool CanSolarPanelRun(Vector2Int center)
        {
            if (!CanUseEquipment(center) || !HasWireAccess(center) || power >= maxPower - 1f || SolarIrradiance() <= 0.01f || !IsSolarPanelSkyExposed(center))
            {
                return false;
            }

            if (HasAutomationControl(center))
            {
                return HasAutomationSignal(center);
            }

            return true;
        }

        private float SolarIrradiance()
        {
            float normalized = Mathf.Repeat(cycleTimer / CycleLengthSeconds, 1f);
            if (normalized <= SolarDayStart || normalized >= SolarDayEnd)
            {
                return 0f;
            }

            float dayProgress = Mathf.InverseLerp(SolarDayStart, SolarDayEnd, normalized);
            return Mathf.Clamp01(Mathf.Sin(dayProgress * Mathf.PI));
        }

        private bool IsSolarPanelSkyExposed(Vector2Int cell)
        {
            if (!IsInside(cell.x, cell.y))
            {
                return false;
            }

            for (int y = cell.y + 1; y < WorldHeight; y++)
            {
                if (IsSolarBlockingCell(cell.x, y))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsSolarBlockingCell(int x, int y)
        {
            if (!IsInside(x, y))
            {
                return false;
            }

            if (cells[x, y] == CellKind.BunkerDoor && !IsBunkerDoorClosed(new Vector2Int(x, y)))
            {
                return waterMass[x, y] > 0.5f;
            }

            return cells[x, y] != CellKind.Empty || waterMass[x, y] > 0.5f;
        }

        private int CountSkyExposedSolarPanels()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] == CellKind.SolarPanel && IsSolarPanelSkyExposed(new Vector2Int(x, y)))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private void SimulateMeteorShowers(float deltaTime)
        {
            if (deltaTime <= 0f)
            {
                return;
            }

            if (meteorShowerSeconds > 0f)
            {
                meteorStrikeTimer -= deltaTime;
                while (meteorStrikeTimer <= 0f && meteorShowerSeconds > 0f)
                {
                    TriggerMeteorWave();
                    meteorStrikeTimer += MeteorStrikeIntervalSeconds;
                }

                meteorShowerSeconds = Mathf.Max(0f, meteorShowerSeconds - deltaTime);
                terrainDirty = true;
                overlayDirty = true;
                if (meteorShowerSeconds <= 0f)
                {
                    meteorCooldownSeconds = MeteorCooldownSeconds;
                    Log("Meteor shower passed. Clear regolith and repair exposed equipment.");
                }

                return;
            }

            meteorCooldownSeconds = Mathf.Max(0f, meteorCooldownSeconds - deltaTime);
            if (meteorCooldownSeconds <= 0f)
            {
                StartMeteorShower();
            }
        }

        private void StartMeteorShower()
        {
            meteorShowerSeconds = MeteorShowerDurationSeconds;
            meteorStrikeTimer = 0f;
            terrainDirty = true;
            overlayDirty = true;
            Log("Meteor shower incoming. Bunker Doors will close and exposed surface equipment may take damage.");
        }

        private void TriggerMeteorWave()
        {
            int seed = Mathf.Abs(cycle * 97 + Mathf.RoundToInt(cycleTimer * 31f) + meteorStrikes * 43);
            for (int i = 0; i < MeteorStrikesPerWave; i++)
            {
                int column = (seed + i * 29 + meteorStrikes * 7) % WorldWidth;
                ImpactMeteorColumn(column);
            }
        }

        private void ImpactMeteorColumn(int x)
        {
            meteorStrikes++;
            for (int y = WorldHeight - 1; y >= 0; y--)
            {
                if (!IsMeteorBlockingCell(x, y))
                {
                    continue;
                }

                Vector2Int impact = new Vector2Int(x, y);
                CellKind hitKind = cells[x, y];
                if (hitKind == CellKind.BunkerDoor && !IsBrokenEquipment(impact))
                {
                    meteorImpactsBlocked++;
                    DamageEquipment(impact, MeteorBunkerDoorDamage);
                    AddHeat(impact, MeteorBunkerImpactHeat, 1);
                    DepositRegolithAboveImpact(impact);
                    if (IsBrokenEquipment(impact))
                    {
                        Log("A Bunker Door was breached by meteor impacts.");
                    }

                    terrainDirty = true;
                    overlayDirty = true;
                    return;
                }

                if (IsRepairableEquipment(hitKind))
                {
                    meteorDamageEvents++;
                    DamageEquipment(impact, MeteorEquipmentDamage);
                    AddHeat(impact, MeteorImpactHeat, 1);
                    DepositRegolithAboveImpact(impact);
                    Log("Meteor damaged " + CellLabel(hitKind) + " at " + x + ", " + y + ".");
                    terrainDirty = true;
                    overlayDirty = true;
                    return;
                }

                if (hitKind == CellKind.Water)
                {
                    waterMass[x, y] = Mathf.Max(0f, waterMass[x, y] - 10f);
                    steam[x, y] += 0.18f;
                    AddHeat(impact, MeteorImpactHeat, 1);
                    DepositRegolithAboveImpact(impact);
                    gasDirty = true;
                    terrainDirty = true;
                    overlayDirty = true;
                    return;
                }

                AddHeat(impact, MeteorImpactHeat * 0.55f, 1);
                DepositRegolithAboveImpact(impact);
                terrainDirty = true;
                overlayDirty = true;
                return;
            }
        }

        private bool IsMeteorBlockingCell(int x, int y)
        {
            if (!IsInside(x, y))
            {
                return false;
            }

            if (cells[x, y] == CellKind.Empty && waterMass[x, y] <= 0.5f)
            {
                return false;
            }

            return cells[x, y] != CellKind.BunkerDoor || !IsBrokenEquipment(new Vector2Int(x, y));
        }

        private void DepositRegolithAboveImpact(Vector2Int impact)
        {
            if (TryPlaceMeteorRegolith(impact.x, impact.y + 1))
            {
                return;
            }

            TryPlaceMeteorRegolith(impact.x + 1, impact.y + 1);
            TryPlaceMeteorRegolith(impact.x - 1, impact.y + 1);
        }

        private bool TryPlaceMeteorRegolith(int x, int y)
        {
            if (!CanPlaceMeteorRegolith(x, y))
            {
                return false;
            }

            Vector2Int cell = new Vector2Int(x, y);
            Worker worker = WorkerAt(cell);
            if (worker != null && worker.Health > 0f)
            {
                worker.Health = Mathf.Max(0f, worker.Health - (worker.SuitEquipped ? 7f : 14f));
                worker.Stress = Mathf.Min(100f, worker.Stress + 16f);
                ClearAssignment(worker);
                if (TryFindOpenSpawnNear(cell, out Vector2Int safeCell))
                {
                    worker.Cell = safeCell;
                    if (worker.Transform != null)
                    {
                        worker.Transform.position = CellCenter(safeCell);
                    }
                }

                worker.Activity = "Dodging Meteor Debris";
                Log(worker.Name + " was struck by meteor debris.");
            }

            cells[x, y] = CellKind.Regolith;
            equipmentCondition[x, y] = 0f;
            waterMass[x, y] = 0f;
            oxygen[x, y] = 0f;
            carbonDioxide[x, y] = 0f;
            pollutedOxygen[x, y] = 0f;
            hydrogen[x, y] = 0f;
            chlorine[x, y] = 0f;
            naturalGas[x, y] = 0f;
            steam[x, y] = 0f;
            germs[x, y] = 0f;
            temperature[x, y] = Mathf.Max(temperature[x, y], MeteorRegolithTemperature);
            meteorRegolithDeposited += 6f;
            CancelJobsAt(cell, false);
            InvalidateRooms();
            gasDirty = true;
            terrainDirty = true;
            overlayDirty = true;
            return true;
        }

        private bool CanPlaceMeteorRegolith(int x, int y)
        {
            return IsInside(x, y) &&
                cells[x, y] == CellKind.Empty &&
                waterMass[x, y] <= 0.05f &&
                !powerWire[x, y] &&
                !automationWire[x, y] &&
                !liquidPipe[x, y] &&
                !gasPipe[x, y] &&
                !shippingRail[x, y] &&
                (looseResourceKind[x, y] == LooseResourceKind.None || looseResourceAmount[x, y] <= 0.01f);
        }

        private bool IsMeteorShowerActive()
        {
            return meteorShowerSeconds > 0f;
        }

        private bool IsBunkerDoorClosed(Vector2Int cell)
        {
            if (!IsInside(cell.x, cell.y) || cells[cell.x, cell.y] != CellKind.BunkerDoor || IsBrokenEquipment(cell))
            {
                return false;
            }

            if (IsMeteorShowerActive())
            {
                return true;
            }

            UpdateAutomationWires();
            return HasCachedAutomationControl(cell) && HasCachedAutomationSignal(cell);
        }

        private int CountIntactBunkerDoors()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] == CellKind.BunkerDoor && !IsBrokenEquipment(new Vector2Int(x, y)))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private void RunSpaceScanner(Vector2Int center, float deltaTime)
        {
            if (!IsSpaceScannerSkyExposed(center))
            {
                spaceScannerBlockedSeconds += deltaTime;
                return;
            }

            if (!CanPoweredMachineRun(center))
            {
                return;
            }

            if (SpaceScannerSignalActive(center.x, center.y))
            {
                spaceScannerSignalSeconds += deltaTime;
            }

            float powerUsed = Mathf.Min(power, SpaceScannerPowerRate * deltaTime);
            float efficiency = powerUsed / Mathf.Max(0.001f, SpaceScannerPowerRate * deltaTime);
            power -= powerUsed;
            AddHeat(center, 0.012f * deltaTime, 1);
            WearEquipment(center, 0.00015f * deltaTime * efficiency);
            overlayDirty = true;
        }

        private bool SpaceScannerSignalActive(int x, int y)
        {
            if (!IsInside(x, y) || cells[x, y] != CellKind.SpaceScanner)
            {
                return false;
            }

            Vector2Int cell = new Vector2Int(x, y);
            if (!CanUseEquipment(cell) || !HasPoweredCircuit(cell) || !IsSpaceScannerSkyExposed(cell))
            {
                return false;
            }

            return IsMeteorShowerActive() || meteorCooldownSeconds <= SpaceScannerWarningSeconds;
        }

        private bool AnySpaceScannerSignalActive()
        {
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (SpaceScannerSignalActive(x, y))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsSpaceScannerSkyExposed(Vector2Int cell)
        {
            if (!IsInside(cell.x, cell.y))
            {
                return false;
            }

            for (int y = cell.y + 1; y < WorldHeight; y++)
            {
                CellKind kind = cells[cell.x, y];
                if (kind != CellKind.Empty || waterMass[cell.x, y] > 0.5f)
                {
                    return false;
                }
            }

            return true;
        }

        private int CountSkyExposedSpaceScanners()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] == CellKind.SpaceScanner && IsSpaceScannerSkyExposed(new Vector2Int(x, y)))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private float HotSteamAvailable(Vector2Int center, int radius)
        {
            float total = 0f;
            for (int dy = -radius; dy <= radius; dy++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    int x = center.x + dx;
                    int y = center.y + dy;
                    if (IsInside(x, y) && IsPassable(x, y) && temperature[x, y] >= SteamTurbineMinimumTemperature)
                    {
                        total += steam[x, y];
                    }
                }
            }

            return total;
        }

        private float RemoveHotSteam(Vector2Int center, float capacity, int radius)
        {
            float removed = 0f;
            for (int dy = radius; dy >= -radius && removed < capacity; dy--)
            {
                for (int dx = -radius; dx <= radius && removed < capacity; dx++)
                {
                    int x = center.x + dx;
                    int y = center.y + dy;
                    if (!IsInside(x, y) || !IsPassable(x, y) || temperature[x, y] < SteamTurbineMinimumTemperature)
                    {
                        continue;
                    }

                    float amount = Mathf.Min(steam[x, y], capacity - removed);
                    if (amount <= 0.001f)
                    {
                        continue;
                    }

                    steam[x, y] = Mathf.Max(0f, steam[x, y] - amount);
                    temperature[x, y] = Mathf.Clamp(temperature[x, y] - amount * 8f, -30f, 120f);
                    removed += amount;
                }
            }

            return removed;
        }

        private float NaturalGasFuelAvailable(Vector2Int center)
        {
            return PipedNaturalGasAround(center) + NaturalGasAround(center, 3);
        }

        private float HydrogenFuelAvailable(Vector2Int center)
        {
            return PipedHydrogenAround(center) + HydrogenAround(center, 3);
        }

        private float PipedNaturalGasAround(Vector2Int center)
        {
            float total = 0f;
            Vector2Int[] candidates =
            {
                center,
                new Vector2Int(center.x + 1, center.y),
                new Vector2Int(center.x - 1, center.y),
                new Vector2Int(center.x, center.y + 1),
                new Vector2Int(center.x, center.y - 1)
            };

            foreach (Vector2Int candidate in candidates)
            {
                if (IsInside(candidate.x, candidate.y) && gasPipe[candidate.x, candidate.y])
                {
                    total += Mathf.Max(0f, gasPipeNaturalGas[candidate.x, candidate.y]);
                }
            }

            return total;
        }

        private float PipedHydrogenAround(Vector2Int center)
        {
            float total = 0f;
            Vector2Int[] candidates =
            {
                center,
                new Vector2Int(center.x + 1, center.y),
                new Vector2Int(center.x - 1, center.y),
                new Vector2Int(center.x, center.y + 1),
                new Vector2Int(center.x, center.y - 1)
            };

            foreach (Vector2Int candidate in candidates)
            {
                if (IsInside(candidate.x, candidate.y) && gasPipe[candidate.x, candidate.y])
                {
                    total += Mathf.Max(0f, gasPipeHydrogen[candidate.x, candidate.y]);
                }
            }

            return total;
        }

        private float RemoveNaturalGasFuel(Vector2Int center, float capacity)
        {
            float removed = RemovePipedNaturalGas(center, capacity);
            if (removed < capacity)
            {
                removed += RemoveNaturalGas(center, capacity - removed, 3);
            }

            return removed;
        }

        private float RemoveHydrogenFuel(Vector2Int center, float capacity)
        {
            float removed = RemovePipedHydrogen(center, capacity);
            if (removed < capacity)
            {
                removed += RemoveHydrogen(center, capacity - removed, 3);
            }

            return removed;
        }

        private float RemovePipedNaturalGas(Vector2Int center, float capacity)
        {
            float removed = 0f;
            for (int i = 0; i < 5 && removed < capacity; i++)
            {
                Vector2Int pipe;
                if (!TryFindAdjacentGasPipeWithNaturalGas(center, out pipe))
                {
                    break;
                }

                float amount = Mathf.Min(gasPipeNaturalGas[pipe.x, pipe.y], capacity - removed);
                if (amount <= 0.001f)
                {
                    break;
                }

                gasPipeNaturalGas[pipe.x, pipe.y] = Mathf.Max(0f, gasPipeNaturalGas[pipe.x, pipe.y] - amount);
                removed += amount;
            }

            return removed;
        }

        private float RemovePipedHydrogen(Vector2Int center, float capacity)
        {
            float removed = 0f;
            for (int i = 0; i < 5 && removed < capacity; i++)
            {
                if (!TryFindAdjacentGasPipeWithHydrogen(center, out Vector2Int pipe))
                {
                    break;
                }

                float amount = Mathf.Min(gasPipeHydrogen[pipe.x, pipe.y], capacity - removed);
                if (amount <= 0.001f)
                {
                    break;
                }

                gasPipeHydrogen[pipe.x, pipe.y] = Mathf.Max(0f, gasPipeHydrogen[pipe.x, pipe.y] - amount);
                removed += amount;
            }

            return removed;
        }

        private bool CanCoalGeneratorRun(Vector2Int center)
        {
            if (!CanUseEquipment(center) || coal <= 0.001f || !HasWireAccess(center) || power >= maxPower - 1f)
            {
                return false;
            }

            if (HasAutomationControl(center))
            {
                return HasAutomationSignal(center);
            }

            return power <= maxPower * 0.72f;
        }

        private float HydrogenAround(Vector2Int center, int radius)
        {
            float total = 0f;
            for (int dy = -radius; dy <= radius; dy++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    int x = center.x + dx;
                    int y = center.y + dy;
                    if (IsInside(x, y) && IsPassable(x, y))
                    {
                        total += hydrogen[x, y];
                    }
                }
            }

            return total;
        }

        private float NaturalGasAround(Vector2Int center, int radius)
        {
            float total = 0f;
            for (int dy = -radius; dy <= radius; dy++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    int x = center.x + dx;
                    int y = center.y + dy;
                    if (IsInside(x, y) && IsPassable(x, y))
                    {
                        total += naturalGas[x, y];
                    }
                }
            }

            return total;
        }

        private float RemoveHydrogen(Vector2Int center, float capacity, int radius)
        {
            float removed = 0f;
            for (int dy = radius; dy >= -radius && removed < capacity; dy--)
            {
                for (int dx = -radius; dx <= radius && removed < capacity; dx++)
                {
                    int x = center.x + dx;
                    int y = center.y + dy;
                    if (!IsInside(x, y) || !IsPassable(x, y))
                    {
                        continue;
                    }

                    float amount = Mathf.Min(hydrogen[x, y], capacity - removed);
                    hydrogen[x, y] -= amount;
                    removed += amount;
                }
            }

            return removed;
        }

        private float RemoveNaturalGas(Vector2Int center, float capacity, int radius)
        {
            float removed = 0f;
            for (int dy = radius; dy >= -radius && removed < capacity; dy--)
            {
                for (int dx = -radius; dx <= radius && removed < capacity; dx++)
                {
                    int x = center.x + dx;
                    int y = center.y + dy;
                    if (!IsInside(x, y) || !IsPassable(x, y))
                    {
                        continue;
                    }

                    float amount = Mathf.Min(naturalGas[x, y], capacity - removed);
                    naturalGas[x, y] -= amount;
                    removed += amount;
                }
            }

            return removed;
        }

        private void RunAtmoSuitDock(Vector2Int center, float deltaTime)
        {
            float capacity = SuitOxygenCapacityTotal();
            if (capacity <= 0f || suitOxygen >= capacity - 0.01f || !CanPoweredMachineRun(center))
            {
                return;
            }

            if (!TryFindSuitDockOxygenSource(center, out Vector2Int source))
            {
                return;
            }

            float powerUsed = Mathf.Min(power, SuitDockPowerRate * deltaTime);
            float powerEfficiency = powerUsed / Mathf.Max(0.001f, SuitDockPowerRate * deltaTime);
            float amount = Mathf.Min(
                oxygen[source.x, source.y],
                Mathf.Min(SuitDockChargeRate * deltaTime * powerEfficiency, capacity - suitOxygen));
            if (amount <= 0.001f)
            {
                return;
            }

            power -= powerUsed;
            oxygen[source.x, source.y] = Mathf.Max(0f, oxygen[source.x, source.y] - amount);
            suitOxygen = Mathf.Min(capacity, suitOxygen + amount);
            AddHeat(center, 0.025f * deltaTime * powerEfficiency, 1);
            WearEquipment(center, 0.00022f * deltaTime * powerEfficiency);
            gasDirty = true;
            overlayDirty = true;
        }

        private bool TryFindSuitDockOxygenSource(Vector2Int center, out Vector2Int source)
        {
            source = center;
            float best = IsInside(center.x, center.y) && IsPassable(center.x, center.y) ? oxygen[center.x, center.y] : 0f;
            Vector2Int[] offsets =
            {
                new Vector2Int(1, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, -1)
            };

            foreach (Vector2Int offset in offsets)
            {
                int x = center.x + offset.x;
                int y = center.y + offset.y;
                if (IsInside(x, y) && IsPassable(x, y) && oxygen[x, y] > best)
                {
                    best = oxygen[x, y];
                    source = new Vector2Int(x, y);
                }
            }

            return best > 0.03f;
        }

        private float SuitOxygenCapacityTotal()
        {
            return CountCells(CellKind.AtmoSuitDock) * SuitDockOxygenCapacity;
        }

        private bool NeedsSuitProtection(Vector2Int cell)
        {
            if (!IsInside(cell.x, cell.y) || !IsPassable(cell.x, cell.y))
            {
                return false;
            }

            return oxygen[cell.x, cell.y] < 0.14f ||
                carbonDioxide[cell.x, cell.y] > 1.1f ||
                pollutedOxygen[cell.x, cell.y] > 0.16f ||
                hydrogen[cell.x, cell.y] > 0.42f ||
                TileGasTotal(cell.x, cell.y) > OverpressureStressThreshold ||
                temperature[cell.x, cell.y] < 0f ||
                temperature[cell.x, cell.y] > 44f;
        }

        private bool UseSuitProtection(Worker worker, float deltaTime)
        {
            if (worker == null || !worker.SuitEquipped || !NeedsSuitProtection(worker.Cell) || suitOxygen <= 0.001f)
            {
                return false;
            }

            float requested = SuitBreathRate * deltaTime;
            float used = Mathf.Min(suitOxygen, requested);
            if (used <= 0.001f)
            {
                return false;
            }

            suitOxygen = Mathf.Max(0f, suitOxygen - used);
            suitOxygenUsed += used;
            overlayDirty = true;
            return used >= requested * 0.5f;
        }

        private bool HasChargedSuitOxygen()
        {
            return suitOxygen > SuitCheckpointMinimumCharge;
        }

        private bool CanEquipSuitAtCheckpoint(Vector2Int checkpoint)
        {
            return IsSuitCheckpointCell(checkpoint) &&
                HasAdjacentSuitDock(checkpoint) &&
                SuitOxygenCapacityTotal() > 0f &&
                HasChargedSuitOxygen();
        }

        private bool CanTraversePathStep(Vector2Int from, Vector2Int to)
        {
            if (!CanCharacterTraversePathStep(from, to))
            {
                return false;
            }

            if (IsSuitCheckpointCell(from) && NeedsSuitProtection(to) && !CanEquipSuitAtCheckpoint(from))
            {
                return false;
            }

            return true;
        }

        private bool CanWorkerTraversePathStep(Worker worker, Vector2Int from, Vector2Int to)
        {
            if (!CanCharacterTraversePathStep(from, to))
            {
                return false;
            }

            if (IsSuitCheckpointCell(from) && NeedsSuitProtection(to))
            {
                if (worker != null && worker.SuitEquipped)
                {
                    return HasChargedSuitOxygen();
                }

                return CanEquipSuitAtCheckpoint(from);
            }

            return true;
        }

        private bool CanCharacterTraversePathStep(Vector2Int from, Vector2Int to)
        {
            if (!IsCharacterPathCell(to, from))
            {
                return false;
            }

            int dx = Mathf.Abs(to.x - from.x);
            int dy = Mathf.Abs(to.y - from.y);
            if (dx + dy != 1)
            {
                return false;
            }

            if (dy > 0 && !CanCharacterMoveVertically(from, to))
            {
                return false;
            }

            return true;
        }

        private bool IsCharacterStandableCell(Vector2Int cell)
        {
            return IsCharacterPathCell(cell, new Vector2Int(-1, -1));
        }

        private bool IsCharacterPathCell(Vector2Int cell, Vector2Int from)
        {
            if (!IsInside(cell.x, cell.y) || !IsPassable(cell.x, cell.y))
            {
                return false;
            }

            CellKind kind = cells[cell.x, cell.y];
            if (kind == CellKind.Ladder || kind == CellKind.Floor)
            {
                return true;
            }

            if (kind == CellKind.ManualAirlock)
            {
                return airlockOpen[cell.x, cell.y];
            }

            if (kind == CellKind.BunkerDoor)
            {
                return !IsBunkerDoorClosed(cell);
            }

            if (kind == CellKind.Empty)
            {
                return HasCharacterFloorSupport(cell) || IsCharacterLadderCell(from);
            }

            return HasCharacterFloorSupport(cell) || IsCharacterSpecialStructure(cell);
        }

        private bool CanCharacterMoveVertically(Vector2Int from, Vector2Int to)
        {
            return IsCharacterLadderCell(from) ||
                IsCharacterLadderCell(to) ||
                IsCharacterSpecialStructure(from) ||
                IsCharacterSpecialStructure(to);
        }

        private bool IsCharacterLadderCell(Vector2Int cell)
        {
            return IsInside(cell.x, cell.y) && cells[cell.x, cell.y] == CellKind.Ladder;
        }

        private bool IsCharacterSpecialStructure(Vector2Int cell)
        {
            if (!IsInside(cell.x, cell.y))
            {
                return false;
            }

            CellKind kind = cells[cell.x, cell.y];
            if (kind == CellKind.ManualAirlock)
            {
                return airlockOpen[cell.x, cell.y];
            }

            if (kind == CellKind.BunkerDoor)
            {
                return !IsBunkerDoorClosed(cell);
            }

            return false;
        }

        private bool HasCharacterFloorSupport(Vector2Int cell)
        {
            if (!IsInside(cell.x, cell.y))
            {
                return false;
            }

            if (cell.y <= 0)
            {
                return true;
            }

            Vector2Int belowCell = new Vector2Int(cell.x, cell.y - 1);
            CellKind below = cells[belowCell.x, belowCell.y];
            if (below == CellKind.Floor || IsSolidTile(below))
            {
                return true;
            }

            return IsCharacterSpecialStructure(belowCell);
        }

        private void DenySuitCheckpointEntry(Worker worker, Vector2Int checkpoint)
        {
            suitEntryDenials++;
            if (worker != null)
            {
                ClearAssignment(worker);
                worker.Activity = "Waiting for Atmo Suit";
                worker.Stress = Mathf.Min(100f, worker.Stress + 0.8f);
            }

            WearEquipment(checkpoint, 0.00015f);
            overlayDirty = true;
            Log((worker != null ? worker.Name + " " : string.Empty) + "cannot cross Atmo Suit Checkpoint without charged suit oxygen.");
        }

        private bool UpdateSuitCheckpointCrossing(Worker worker, Vector2Int from, Vector2Int to)
        {
            if (worker == null || from == to)
            {
                return true;
            }

            bool fromCheckpoint = IsSuitCheckpointCell(from);
            bool toCheckpoint = IsSuitCheckpointCell(to);
            if (!fromCheckpoint && !toCheckpoint)
            {
                return true;
            }

            if (fromCheckpoint)
            {
                if (!IsInside(to.x, to.y) || !IsPassable(to.x, to.y))
                {
                    return true;
                }

                if (NeedsSuitProtection(to))
                {
                    if (worker.SuitEquipped)
                    {
                        return HasChargedSuitOxygen();
                    }

                    return EquipSuitAtCheckpoint(worker, from);
                }

                if (worker.SuitEquipped)
                {
                    ReturnSuitAtCheckpoint(worker, from);
                }
            }

            return true;
        }

        private bool EquipSuitAtCheckpoint(Worker worker, Vector2Int checkpoint)
        {
            if (worker == null || worker.SuitEquipped)
            {
                return worker != null && worker.SuitEquipped;
            }

            if (!IsSuitCheckpointCell(checkpoint) || !HasAdjacentSuitDock(checkpoint) || SuitOxygenCapacityTotal() <= 0f)
            {
                return false;
            }

            if (suitOxygen <= SuitCheckpointMinimumCharge)
            {
                Log("Atmo Suit Checkpoint needs charged suit oxygen before entry.");
                return false;
            }

            worker.SuitEquipped = true;
            suitCheckpointUses++;
            WearEquipment(checkpoint, 0.0007f);
            overlayDirty = true;
            Log(worker.Name + " equipped an atmo suit.");
            return true;
        }

        private void ReturnSuitAtCheckpoint(Worker worker, Vector2Int checkpoint)
        {
            if (worker == null || !worker.SuitEquipped || !IsSuitCheckpointCell(checkpoint))
            {
                return;
            }

            worker.SuitEquipped = false;
            suitCheckpointUses++;
            WearEquipment(checkpoint, 0.00035f);
            overlayDirty = true;
            Log(worker.Name + " returned an atmo suit.");
        }

        private bool IsSuitCheckpointCell(Vector2Int cell)
        {
            return IsInside(cell.x, cell.y) &&
                cells[cell.x, cell.y] == CellKind.AtmoSuitCheckpoint &&
                !IsBrokenEquipment(cell);
        }

        private bool HasAdjacentSuitDock(Vector2Int checkpoint)
        {
            return IsUsableSuitDock(checkpoint.x + 1, checkpoint.y) ||
                IsUsableSuitDock(checkpoint.x - 1, checkpoint.y) ||
                IsUsableSuitDock(checkpoint.x, checkpoint.y + 1) ||
                IsUsableSuitDock(checkpoint.x, checkpoint.y - 1);
        }

        private bool HasAdjacentSuitCheckpoint(Vector2Int dock)
        {
            return IsSuitCheckpointCell(new Vector2Int(dock.x + 1, dock.y)) ||
                IsSuitCheckpointCell(new Vector2Int(dock.x - 1, dock.y)) ||
                IsSuitCheckpointCell(new Vector2Int(dock.x, dock.y + 1)) ||
                IsSuitCheckpointCell(new Vector2Int(dock.x, dock.y - 1));
        }

        private bool IsUsableSuitDock(int x, int y)
        {
            return IsInside(x, y) &&
                cells[x, y] == CellKind.AtmoSuitDock &&
                !IsBrokenEquipment(new Vector2Int(x, y));
        }

        private int CountSuitedWorkers()
        {
            int count = 0;
            foreach (Worker worker in workers)
            {
                if (worker.SuitEquipped && worker.Health > 0f)
                {
                    count++;
                }
            }

            return count;
        }

        private void SimulateFoodSpoilage(float deltaTime)
        {
            if (food <= 0f)
            {
                food = 0f;
                foodFreshness = 1f;
                return;
            }

            int refrigerators = CountCells(CellKind.Refrigerator);
            int poweredRefrigerators = CountPoweredRefrigerators();
            float storageTemperature = AverageFoodStorageTemperature();
            float temperatureFactor = storageTemperature > 38f ? 2.3f :
                storageTemperature > 28f ? 1.55f :
                storageTemperature < 8f ? 0.55f : 1f;
            float storageFactor = poweredRefrigerators > 0 ? 0.18f : refrigerators > 0 ? 0.78f : 1f;
            float decayRate = 0.00022f * temperatureFactor * storageFactor;
            float spoiled = Mathf.Min(food, food * decayRate * deltaTime);

            food -= spoiled;
            if (spoiled > 0.001f)
            {
                AddPollutedDirt(spoiled * 0.16f, FoodWasteCell());
            }

            foodFreshness = Mathf.Clamp01(foodFreshness - decayRate * deltaTime * 4.5f);
            if (food <= 1f)
            {
                food = 0f;
                foodFreshness = 1f;
            }
        }

        private void SimulatePollutedDirtOffgas(float deltaTime)
        {
            if (pollutedDirt <= 0.01f)
            {
                return;
            }

            Vector2Int source = WasteStorageCell();
            float pressure = Mathf.Clamp01(pollutedDirt / 80f);
            VentPollutedOxygen(source, (0.006f + pressure * 0.026f) * deltaTime, (0.018f + pressure * 0.055f) * deltaTime);
            gasDirty = true;
            overlayDirty = true;
        }

        private void SimulatePollutedWaterOffgas(float deltaTime)
        {
            if (deltaTime <= 0f || pollutedWater <= PollutedWaterOffgasMinimum)
            {
                return;
            }

            CollectPollutedWaterOffgasSources(pollutedWaterOffgasSources);
            if (pollutedWaterOffgasSources.Count == 0)
            {
                return;
            }

            float pressure = Mathf.Clamp01(pollutedWater / 120f);
            float sourceScale = 1f / Mathf.Sqrt(Mathf.Max(1, pollutedWaterOffgasSources.Count));
            float emitted = 0f;
            for (int i = 0; i < pollutedWaterOffgasSources.Count; i++)
            {
                float amount = (0.0035f + pressure * 0.014f) * deltaTime * sourceScale;
                float germsAdded = (0.010f + pressure * 0.040f) * deltaTime * sourceScale;
                emitted += VentPollutedOxygen(pollutedWaterOffgasSources[i], amount, germsAdded);
            }

            if (emitted > 0.001f)
            {
                pollutedWaterOffgassedMass += emitted;
                pollutedWaterOffgasEvents++;
                gasDirty = true;
                overlayDirty = true;
            }
        }

        private void CollectPollutedWaterOffgasSources(List<Vector2Int> sources)
        {
            sources.Clear();
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (IsPollutedWaterOffgasSource(cells[x, y]))
                    {
                        sources.Add(new Vector2Int(x, y));
                    }
                }
            }

            if (sources.Count == 0)
            {
                sources.Add(WasteStorageCell());
            }
        }

        private int CountPollutedWaterOffgasSources()
        {
            if (pollutedWater <= PollutedWaterOffgasMinimum)
            {
                return 0;
            }

            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (IsPollutedWaterOffgasSource(cells[x, y]))
                    {
                        count++;
                    }
                }
            }

            return Mathf.Max(1, count);
        }

        private bool IsPollutedWaterOffgasSource(CellKind kind)
        {
            return kind == CellKind.BottleEmptier ||
                kind == CellKind.CarbonSkimmer ||
                kind == CellKind.WaterSieve ||
                kind == CellKind.WashBasin;
        }

        private void SimulateFallingSand()
        {
            bool changed = false;
            int moved = 0;
            for (int y = 1; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (!IsFallingSolid(cells[x, y]) || !CanSandFallInto(x, y - 1))
                    {
                        continue;
                    }

                    MoveSandDown(new Vector2Int(x, y), new Vector2Int(x, y - 1));
                    changed = true;
                    moved++;
                }
            }

            if (!changed)
            {
                return;
            }

            sandFalls += moved;
            terrainDirty = true;
            gasDirty = true;
            overlayDirty = true;
            if (moved >= 3)
            {
                Log("Unstable loose terrain collapsed " + moved + " tiles.");
            }
        }

        private bool IsFallingSolid(CellKind kind)
        {
            return kind == CellKind.Sand || kind == CellKind.Regolith;
        }

        private bool CanSandFallInto(int x, int y)
        {
            return IsInside(x, y) &&
                cells[x, y] == CellKind.Empty &&
                waterMass[x, y] <= 0.05f &&
                !powerWire[x, y] &&
                !automationWire[x, y] &&
                !liquidPipe[x, y] &&
                !gasPipe[x, y] &&
                !shippingRail[x, y] &&
                (looseResourceKind[x, y] == LooseResourceKind.None || looseResourceAmount[x, y] <= 0.01f);
        }

        private void MoveSandDown(Vector2Int from, Vector2Int to)
        {
            float displacedOxygen = oxygen[to.x, to.y];
            float displacedCarbon = carbonDioxide[to.x, to.y];
            float displacedPolluted = pollutedOxygen[to.x, to.y];
            float displacedHydrogen = hydrogen[to.x, to.y];
            float displacedChlorine = chlorine[to.x, to.y];
            float displacedNaturalGas = naturalGas[to.x, to.y];
            float displacedGerms = germs[to.x, to.y];
            float sourceTemperature = temperature[from.x, from.y];
            float displacedTemperature = temperature[to.x, to.y];

            CellKind fallingKind = cells[from.x, from.y];
            cells[to.x, to.y] = fallingKind;
            cells[from.x, from.y] = CellKind.Empty;
            equipmentCondition[to.x, to.y] = 0f;
            equipmentCondition[from.x, from.y] = 0f;
            plantGrowth[to.x, to.y] = 0f;
            cropTendedSeconds[to.x, to.y] = 0f;
            cropStress[to.x, to.y] = 0f;

            oxygen[from.x, from.y] = displacedOxygen;
            carbonDioxide[from.x, from.y] = displacedCarbon;
            pollutedOxygen[from.x, from.y] = displacedPolluted;
            hydrogen[from.x, from.y] = displacedHydrogen;
            chlorine[from.x, from.y] = displacedChlorine;
            naturalGas[from.x, from.y] = displacedNaturalGas;
            germs[from.x, from.y] = displacedGerms;

            oxygen[to.x, to.y] = 0f;
            carbonDioxide[to.x, to.y] = 0f;
            pollutedOxygen[to.x, to.y] = 0f;
            hydrogen[to.x, to.y] = 0f;
            chlorine[to.x, to.y] = 0f;
            naturalGas[to.x, to.y] = 0f;
            germs[to.x, to.y] = 0f;
            temperature[to.x, to.y] = Mathf.Lerp(displacedTemperature, sourceTemperature, 0.75f);
            temperature[from.x, from.y] = Mathf.Lerp(sourceTemperature, displacedTemperature, 0.35f);

            CancelJobsAt(to, false);
            DisplaceWorkersFromFallingSand(from, to);
        }

        private void DisplaceWorkersFromFallingSand(Vector2Int from, Vector2Int to)
        {
            for (int i = 0; i < workers.Count; i++)
            {
                Worker worker = workers[i];
                if (worker == null || worker.Health <= 0f)
                {
                    continue;
                }

                Vector2Int workerCell = worker.Cell;
                if (worker.Transform != null)
                {
                    workerCell = WorldToCell(worker.Transform.position);
                }

                if (workerCell != to)
                {
                    continue;
                }

                sandStrikeInjuries++;
                worker.Health = Mathf.Max(0f, worker.Health - (worker.SuitEquipped ? 4f : 9f));
                worker.Stress = Mathf.Min(100f, worker.Stress + 12f);
                ClearAssignment(worker);
                worker.Cell = from;
                if (worker.Transform != null)
                {
                    worker.Transform.position = CellCenter(from);
                }

                worker.Activity = "Dodging Falling Sand";
                Log(worker.Name + " was struck by falling sand.");
            }
        }

        private int CountUnstableSandTiles()
        {
            int count = 0;
            for (int y = 1; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (IsFallingSolid(cells[x, y]) && CanSandFallInto(x, y - 1))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private void AddPollutedDirt(float amount, Vector2Int source)
        {
            if (amount <= 0f)
            {
                return;
            }

            pollutedDirt += amount;
            VentPollutedOxygen(source, 0.01f * Mathf.Clamp(amount, 0.1f, 6f), 0.018f * Mathf.Clamp(amount, 0.1f, 6f));
            gasDirty = true;
            overlayDirty = true;
        }

        private Vector2Int FoodWasteCell()
        {
            Vector2Int refrigerator = FirstCellOfKind(CellKind.Refrigerator);
            if (refrigerator.x >= 0)
            {
                return refrigerator;
            }

            Vector2Int storage = FirstCellOfKind(CellKind.StorageBin);
            if (storage.x >= 0)
            {
                return storage;
            }

            return WasteStorageCell();
        }

        private Vector2Int WasteStorageCell()
        {
            Vector2Int compost = FirstCellOfKind(CellKind.Compost);
            if (compost.x >= 0)
            {
                return compost;
            }

            Vector2Int outhouse = FirstCellOfKind(CellKind.Outhouse);
            if (outhouse.x >= 0)
            {
                return outhouse;
            }

            if (workers.Count > 0)
            {
                return workers[0].Cell;
            }

            return new Vector2Int(WorldWidth / 2, WorldHeight / 2);
        }

        private Vector2Int FirstCellOfKind(CellKind kind)
        {
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] == kind)
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }

            return new Vector2Int(-1, -1);
        }

        private float AverageFoodStorageTemperature()
        {
            int refrigerators = CountCells(CellKind.Refrigerator);
            if (refrigerators == 0)
            {
                return AverageTemperature();
            }

            float total = 0f;
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] == CellKind.Refrigerator)
                    {
                        total += IsPoweredRefrigeratorAt(x, y) ? 3f : temperature[x, y];
                        count++;
                    }
                }
            }

            return count == 0 ? AverageTemperature() : total / count;
        }

        private int CountPoweredRefrigerators()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (IsPoweredRefrigeratorAt(x, y))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private bool IsPoweredRefrigeratorAt(int x, int y)
        {
            return IsInside(x, y) &&
                cells[x, y] == CellKind.Refrigerator &&
                CanPoweredMachineRun(new Vector2Int(x, y));
        }

        private void AddHeat(Vector2Int center, float amount, int radius)
        {
            AdjustTemperatureArea(center, Mathf.Abs(amount), radius);
        }

        private void CoolArea(Vector2Int center, float amount, int radius)
        {
            AdjustTemperatureArea(center, -Mathf.Abs(amount), radius);
        }

        private void AdjustTemperatureArea(Vector2Int center, float amount, int radius)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    int x = center.x + dx;
                    int y = center.y + dy;
                    if (!IsInside(x, y) || (!IsPassable(x, y) && cells[x, y] != CellKind.Water))
                    {
                        continue;
                    }

                    float distance = Mathf.Abs(dx) + Mathf.Abs(dy);
                    float falloff = 1f / (1f + distance);
                    temperature[x, y] = Mathf.Clamp(temperature[x, y] + amount * falloff, -30f, 120f);
                }
            }
        }

        private float AverageTemperatureAround(Vector2Int center, int radius)
        {
            float total = 0f;
            int count = 0;
            for (int dy = -radius; dy <= radius; dy++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    int x = center.x + dx;
                    int y = center.y + dy;
                    if (IsInside(x, y) && (IsPassable(x, y) || cells[x, y] == CellKind.Water))
                    {
                        total += temperature[x, y];
                        count++;
                    }
                }
            }

            return count == 0 ? temperature[center.x, center.y] : total / count;
        }

        private void AddOxygen(Vector2Int center, float amount)
        {
            float share = amount / 9f;
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    int x = center.x + dx;
                    int y = center.y + dy;
                    if (IsInside(x, y) && IsPassable(x, y))
                    {
                        oxygen[x, y] = Mathf.Min(2.2f, oxygen[x, y] + share);
                    }
                }
            }
        }

        private void AddHydrogen(Vector2Int center, float amount)
        {
            float share = amount / 6f;
            Vector2Int[] offsets =
            {
                new Vector2Int(0, 1),
                new Vector2Int(1, 1),
                new Vector2Int(-1, 1),
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(-1, 0)
            };

            foreach (Vector2Int offset in offsets)
            {
                int x = center.x + offset.x;
                int y = center.y + offset.y;
                if (IsInside(x, y) && IsPassable(x, y))
                {
                    hydrogen[x, y] = Mathf.Min(2.8f, hydrogen[x, y] + share);
                }
            }
        }

        private void StepGas(float deltaTime)
        {
            Array.Copy(oxygen, nextOxygen, oxygen.Length);
            Array.Copy(carbonDioxide, nextCarbonDioxide, carbonDioxide.Length);
            Array.Copy(pollutedOxygen, nextPollutedOxygen, pollutedOxygen.Length);
            Array.Copy(hydrogen, nextHydrogen, hydrogen.Length);
            Array.Copy(steam, nextSteam, steam.Length);
            Array.Copy(chlorine, nextChlorine, chlorine.Length);
            Array.Copy(naturalGas, nextNaturalGas, naturalGas.Length);
            Array.Copy(germs, nextGerms, germs.Length);

            float flowRate = Mathf.Clamp01(deltaTime * 0.75f);
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (!IsPassable(x, y))
                    {
                        nextOxygen[x, y] = 0f;
                        nextCarbonDioxide[x, y] = 0f;
                        nextPollutedOxygen[x, y] = 0f;
                        nextHydrogen[x, y] = 0f;
                        nextSteam[x, y] = 0f;
                        nextChlorine[x, y] = 0f;
                        nextNaturalGas[x, y] = 0f;
                        continue;
                    }

                    EqualizeGas(x, y, x + 1, y, flowRate);
                    EqualizeGas(x, y, x, y + 1, flowRate);
                }
            }

            for (int y = 1; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (!IsPassable(x, y) || !IsPassable(x, y - 1))
                    {
                        continue;
                    }

                    float settlingFactor = Mathf.Min(TileGasPermeability(x, y), TileGasPermeability(x, y - 1));
                    float settle = Mathf.Min(nextCarbonDioxide[x, y] * 0.045f * deltaTime * settlingFactor, 0.05f);
                    nextCarbonDioxide[x, y] -= settle;
                    nextCarbonDioxide[x, y - 1] += settle;
                    float chlorineSettle = Mathf.Min(nextChlorine[x, y] * 0.032f * deltaTime * settlingFactor, 0.04f);
                    nextChlorine[x, y] -= chlorineSettle;
                    nextChlorine[x, y - 1] += chlorineSettle;
                }
            }

            for (int y = 0; y < WorldHeight - 1; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (!IsPassable(x, y) || !IsPassable(x, y + 1))
                    {
                        continue;
                    }

                    float liftFactor = Mathf.Min(TileGasPermeability(x, y), TileGasPermeability(x, y + 1));
                    float lift = Mathf.Min(nextHydrogen[x, y] * 0.060f * deltaTime * liftFactor, 0.06f);
                    nextHydrogen[x, y] -= lift;
                    nextHydrogen[x, y + 1] += lift;

                    float steamLift = Mathf.Min(nextSteam[x, y] * 0.035f * deltaTime * liftFactor, 0.045f);
                    nextSteam[x, y] -= steamLift;
                    nextSteam[x, y + 1] += steamLift;

                    float naturalGasLift = Mathf.Min(nextNaturalGas[x, y] * 0.032f * deltaTime * liftFactor, 0.04f);
                    nextNaturalGas[x, y] -= naturalGasLift;
                    nextNaturalGas[x, y + 1] += naturalGasLift;
                }
            }

            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    oxygen[x, y] = Mathf.Clamp(nextOxygen[x, y], 0f, 2.8f);
                    carbonDioxide[x, y] = Mathf.Clamp(nextCarbonDioxide[x, y], 0f, 2.8f);
                    pollutedOxygen[x, y] = Mathf.Clamp(nextPollutedOxygen[x, y], 0f, 2.8f);
                    hydrogen[x, y] = Mathf.Clamp(nextHydrogen[x, y], 0f, 2.8f);
                    steam[x, y] = Mathf.Clamp(nextSteam[x, y], 0f, 2.8f);
                    chlorine[x, y] = Mathf.Clamp(nextChlorine[x, y], 0f, 2.8f);
                    naturalGas[x, y] = Mathf.Clamp(nextNaturalGas[x, y], 0f, 2.8f);
                    float sterilized = Mathf.Min(Mathf.Clamp01(nextGerms[x, y]), chlorine[x, y] * ChlorineSterilizeRate * deltaTime);
                    if (sterilized > 0.0001f)
                    {
                        chlorineSterilizedGerms += sterilized;
                    }

                    nextGerms[x, y] = Mathf.Max(0f, nextGerms[x, y] - sterilized);
                    germs[x, y] = IsPassable(x, y) ? Mathf.Clamp01(nextGerms[x, y] - 0.012f * deltaTime) : germs[x, y];
                }
            }
        }

        private void StepTemperature(float deltaTime)
        {
            Array.Copy(temperature, nextTemperature, temperature.Length);

            float rate = Mathf.Clamp01(deltaTime * 0.38f);
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    EqualizeTemperature(x, y, x + 1, y, rate);
                    EqualizeTemperature(x, y, x, y + 1, rate);
                }
            }

            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    float value = nextTemperature[x, y];
                    if (y <= 2 && cells[x, y] == CellKind.Rock)
                    {
                        value = Mathf.MoveTowards(value, 68f, 0.18f * deltaTime);
                    }
                    else if (y >= 40 && IsPassable(x, y))
                    {
                        value = Mathf.MoveTowards(value, -8f, 0.1f * deltaTime);
                    }

                    if (cells[x, y] == CellKind.Ice && value > 1f)
                    {
                        MeltIceCell(x, y, value);
                        value = temperature[x, y];
                    }
                    else if (cells[x, y] == CellKind.Water && value < -1f)
                    {
                        FreezeWaterCell(x, y, value);
                        value = temperature[x, y];
                    }

                    if (HasWorldLiquidAt(x, y) && waterMass[x, y] > LiquidMinimumRetainedMass && value > WaterEvaporationTemperature)
                    {
                        EvaporateWaterCell(x, y, value, deltaTime);
                        value = temperature[x, y];
                    }
                    else if (steam[x, y] > 0.02f && value < SteamCondensationTemperature)
                    {
                        CondenseSteamCell(x, y, value, deltaTime);
                        value = temperature[x, y];
                    }

                    temperature[x, y] = Mathf.Clamp(value, -30f, 120f);
                }
            }
        }

        private void MeltIceCell(int x, int y, float temperatureValue)
        {
            cells[x, y] = CellKind.Water;
            waterMass[x, y] = 35f;
            equipmentCondition[x, y] = 0f;
            oxygen[x, y] = 0f;
            carbonDioxide[x, y] = 0f;
            pollutedOxygen[x, y] = 0f;
            hydrogen[x, y] = 0f;
            steam[x, y] = 0f;
            chlorine[x, y] = 0f;
            naturalGas[x, y] = 0f;
            germs[x, y] = 0f;
            powerWire[x, y] = false;
            poweredWire[x, y] = false;
            iceMeltedTiles++;
            temperature[x, y] = Mathf.Clamp(Mathf.Max(1.2f, temperatureValue), -30f, 120f);
            terrainDirty = true;
            gasDirty = true;
            overlayDirty = true;
        }

        private void FreezeWaterCell(int x, int y, float temperatureValue)
        {
            Vector2Int cell = new Vector2Int(x, y);
            cells[x, y] = CellKind.Ice;
            waterMass[x, y] = 0f;
            equipmentCondition[x, y] = 0f;
            plantGrowth[x, y] = 0f;
            cropTendedSeconds[x, y] = 0f;
            cropStress[x, y] = 0f;
            oxygen[x, y] = 0f;
            carbonDioxide[x, y] = 0f;
            pollutedOxygen[x, y] = 0f;
            hydrogen[x, y] = 0f;
            steam[x, y] = 0f;
            chlorine[x, y] = 0f;
            naturalGas[x, y] = 0f;
            germs[x, y] = 0f;
            looseResourceKind[x, y] = LooseResourceKind.None;
            looseResourceAmount[x, y] = 0f;
            powerWire[x, y] = false;
            poweredWire[x, y] = false;
            wireLoad[x, y] = 0f;
            overloadedWire[x, y] = false;
            wireOverloadStress[x, y] = 0f;
            automationWire[x, y] = false;
            automationControlledWire[x, y] = false;
            automationSignalWire[x, y] = false;
            liquidPipe[x, y] = false;
            pipeWater[x, y] = 0f;
            gasPipe[x, y] = false;
            gasPipeOxygen[x, y] = 0f;
            gasPipeCarbonDioxide[x, y] = 0f;
            gasPipePollutedOxygen[x, y] = 0f;
            gasPipeHydrogen[x, y] = 0f;
            gasPipeChlorine[x, y] = 0f;
            gasPipeNaturalGas[x, y] = 0f;
            gasPipeGerms[x, y] = 0f;
            shippingRail[x, y] = false;
            shippingRailKind[x, y] = LooseResourceKind.None;
            shippingRailAmount[x, y] = 0f;
            waterFrozenTiles++;
            temperature[x, y] = Mathf.Clamp(Mathf.Min(-1.2f, temperatureValue), -30f, 120f);
            CancelJobsAt(cell, false);
            terrainDirty = true;
            gasDirty = true;
            overlayDirty = true;
        }

        private void EvaporateWaterCell(int x, int y, float temperatureValue, float deltaTime)
        {
            if (!HasWorldLiquidAt(x, y))
            {
                return;
            }

            Vector2Int source = new Vector2Int(x, y);
            Vector2Int output;
            if (!TryFindSteamOutput(source, out output))
            {
                return;
            }

            float heatDrive = Mathf.Max(0f, temperatureValue - WaterEvaporationTemperature);
            float requested = heatDrive * WaterEvaporationRate * deltaTime;
            float outputFree = Mathf.Max(0f, 2.8f - TileGasTotal(output.x, output.y));
            float amount = Mathf.Min(Mathf.Min(waterMass[x, y], requested), outputFree);
            if (amount <= 0.001f)
            {
                return;
            }

            waterMass[x, y] = Mathf.Max(0f, waterMass[x, y] - amount);
            if (waterMass[x, y] <= LiquidMinimumRetainedMass)
            {
                if (cells[x, y] == CellKind.Water)
                {
                    DryWaterCell(source, 0.04f, 0.02f, 0f, 0f, 0f, 0f, 0f, Mathf.Min(temperatureValue, WaterEvaporationTemperature));
                }
                else
                {
                    waterMass[x, y] = 0f;
                }
            }

            float added = AddSteamToTile(output, amount);
            if (added <= 0.001f)
            {
                return;
            }

            steamEvaporatedMass += added;
            temperature[x, y] = Mathf.Clamp(temperatureValue - added * 0.8f, -30f, 120f);
            temperature[output.x, output.y] = Mathf.Clamp(Mathf.Max(temperature[output.x, output.y], WaterEvaporationTemperature + added * 0.25f), -30f, 120f);
            terrainDirty = true;
            gasDirty = true;
            overlayDirty = true;
        }

        private void CondenseSteamCell(int x, int y, float temperatureValue, float deltaTime)
        {
            if (!IsPassable(x, y) || steam[x, y] <= 0.001f)
            {
                return;
            }

            Vector2Int source = new Vector2Int(x, y);
            Vector2Int output;
            if (!TryFindCondensateOutput(source, out output))
            {
                return;
            }

            float chillDrive = Mathf.Max(0f, SteamCondensationTemperature - temperatureValue);
            float requested = chillDrive * SteamCondensationRate * deltaTime;
            float amount = Mathf.Min(Mathf.Min(steam[x, y], requested), LiquidFreeCapacity(output.x, output.y));
            if (amount <= 0.001f)
            {
                return;
            }

            steam[x, y] = Mathf.Max(0f, steam[x, y] - amount);
            ReleaseWaterToCell(output, amount);
            temperature[output.x, output.y] = Mathf.Clamp(Mathf.Max(temperature[output.x, output.y], temperatureValue + amount * 1.4f), -30f, 120f);
            temperature[x, y] = Mathf.Clamp(temperatureValue + amount * 0.8f, -30f, 120f);
            steamCondensedMass += amount;
            terrainDirty = true;
            gasDirty = true;
            overlayDirty = true;
        }

        private bool TryFindSteamOutput(Vector2Int source, out Vector2Int output)
        {
            output = new Vector2Int(-1, -1);
            Vector2Int[] candidates =
            {
                source,
                new Vector2Int(source.x, source.y + 1),
                new Vector2Int(source.x + 1, source.y + 1),
                new Vector2Int(source.x - 1, source.y + 1),
                new Vector2Int(source.x + 1, source.y),
                new Vector2Int(source.x - 1, source.y),
                new Vector2Int(source.x, source.y + 2),
                new Vector2Int(source.x, source.y - 1)
            };

            float bestPressure = float.MaxValue;
            for (int i = 0; i < candidates.Length; i++)
            {
                Vector2Int candidate = candidates[i];
                if (!IsInside(candidate.x, candidate.y) || !IsPassable(candidate.x, candidate.y))
                {
                    continue;
                }

                float pressure = TileGasTotal(candidate.x, candidate.y);
                if (pressure >= 2.78f || pressure >= bestPressure)
                {
                    continue;
                }

                bestPressure = pressure;
                output = candidate;
            }

            return output.x >= 0;
        }

        private bool TryFindCondensateOutput(Vector2Int source, out Vector2Int output)
        {
            output = new Vector2Int(-1, -1);
            Vector2Int[] candidates =
            {
                source,
                new Vector2Int(source.x, source.y - 1),
                new Vector2Int(source.x + 1, source.y),
                new Vector2Int(source.x - 1, source.y),
                new Vector2Int(source.x, source.y + 1)
            };

            float bestMass = float.MaxValue;
            for (int i = 0; i < candidates.Length; i++)
            {
                Vector2Int candidate = candidates[i];
                if (!IsInside(candidate.x, candidate.y) || !CanLiquidOccupy(candidate.x, candidate.y))
                {
                    continue;
                }

                float free = LiquidFreeCapacity(candidate.x, candidate.y);
                float mass = LiquidMassAt(candidate.x, candidate.y);
                if (free <= LiquidMinimumRetainedMass || mass >= bestMass)
                {
                    continue;
                }

                bestMass = mass;
                output = candidate;
            }

            return output.x >= 0;
        }

        private float AddSteamToTile(Vector2Int cell, float amount)
        {
            if (!IsInside(cell.x, cell.y) || !IsPassable(cell.x, cell.y) || amount <= 0f)
            {
                return 0f;
            }

            float free = Mathf.Max(0f, 2.8f - TileGasTotal(cell.x, cell.y));
            float added = Mathf.Min(amount, free);
            if (added <= 0.001f)
            {
                return 0f;
            }

            steam[cell.x, cell.y] = Mathf.Min(2.8f, steam[cell.x, cell.y] + added);
            return added;
        }

        private int CountMeltingIceTiles()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] == CellKind.Ice && temperature[x, y] > 1f)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountFreezingWaterTiles()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] == CellKind.Water && temperature[x, y] < -1f)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private void EqualizeTemperature(int ax, int ay, int bx, int by, float rate)
        {
            if (!IsInside(bx, by))
            {
                return;
            }

            float conductivity = Mathf.Min(TileThermalConductivity(ax, ay), TileThermalConductivity(bx, by));
            if (conductivity <= 0f)
            {
                return;
            }

            float flow = (temperature[ax, ay] - temperature[bx, by]) * conductivity * rate;
            nextTemperature[ax, ay] -= flow / ThermalCapacity(cells[ax, ay]);
            nextTemperature[bx, by] += flow / ThermalCapacity(cells[bx, by]);
        }

        private float ThermalConductivity(CellKind kind)
        {
            switch (kind)
            {
                case CellKind.Empty:
                    return 0.08f;
                case CellKind.Water:
                    return 0.18f;
                case CellKind.Ice:
                    return 0.16f;
                case CellKind.InsulatedTile:
                    return 0.018f;
                case CellKind.Regolith:
                    return 0.10f;
                case CellKind.MetalOre:
                case CellKind.Battery:
                case CellKind.ManualGenerator:
                case CellKind.CoalGenerator:
                case CellKind.HydrogenGenerator:
                case CellKind.NaturalGasGenerator:
                case CellKind.SteamTurbine:
                case CellKind.SolarPanel:
                case CellKind.BunkerDoor:
                case CellKind.SpaceScanner:
                case CellKind.HydrogenFilter:
                case CellKind.RockCrusher:
                case CellKind.AtmoSuitDock:
                case CellKind.AtmoSuitCheckpoint:
                case CellKind.LiquidReservoir:
                case CellKind.GasReservoir:
                case CellKind.LiquidPipeSensor:
                case CellKind.LiquidShutoff:
                case CellKind.GasPipeSensor:
                case CellKind.GasShutoff:
                case CellKind.SteamVent:
                case CellKind.HydrogenVent:
                case CellKind.NaturalGasVent:
                case CellKind.SpaceHeater:
                case CellKind.ThermoRegulator:
                    return 0.32f;
                case CellKind.ManualAirlock:
                    return 0.045f;
                case CellKind.Floor:
                case CellKind.Ladder:
                    return 0.24f;
                case CellKind.Rock:
                    return 0.12f;
                default:
                    return 0.18f;
            }
        }

        private float TileThermalConductivity(int x, int y)
        {
            CellKind kind = cells[x, y];
            if (kind == CellKind.ManualAirlock)
            {
                return airlockOpen[x, y] ? 0.20f : 0.026f;
            }

            return ThermalConductivity(kind);
        }

        private float ThermalCapacity(CellKind kind)
        {
            switch (kind)
            {
                case CellKind.Empty:
                    return 1f;
                case CellKind.Water:
                    return 4f;
                case CellKind.Rock:
                case CellKind.Regolith:
                case CellKind.Coal:
                case CellKind.Ice:
                    return 3f;
                case CellKind.MetalOre:
                    return 2.4f;
                case CellKind.SteamVent:
                case CellKind.HydrogenVent:
                case CellKind.NaturalGasVent:
                    return 2.8f;
                case CellKind.SteamTurbine:
                    return 2.4f;
                case CellKind.SolarPanel:
                    return 1.9f;
                case CellKind.SpaceScanner:
                    return 1.7f;
                case CellKind.InsulatedTile:
                    return 3.6f;
                case CellKind.ManualAirlock:
                case CellKind.BunkerDoor:
                    return 2.1f;
                default:
                    return 1.6f;
            }
        }

        private void EqualizeGas(int ax, int ay, int bx, int by, float flowRate)
        {
            if (!IsInside(bx, by) || !IsPassable(ax, ay) || !IsPassable(bx, by))
            {
                return;
            }

            flowRate *= Mathf.Min(TileGasPermeability(ax, ay), TileGasPermeability(bx, by));
            float oxygenFlow = (oxygen[ax, ay] - oxygen[bx, by]) * flowRate * 0.5f;
            float co2Flow = (carbonDioxide[ax, ay] - carbonDioxide[bx, by]) * flowRate * 0.5f;
            float pollutedFlow = (pollutedOxygen[ax, ay] - pollutedOxygen[bx, by]) * flowRate * 0.5f;
            float hydrogenFlow = (hydrogen[ax, ay] - hydrogen[bx, by]) * flowRate * 0.5f;
            float steamFlow = (steam[ax, ay] - steam[bx, by]) * flowRate * 0.5f;
            float chlorineFlow = (chlorine[ax, ay] - chlorine[bx, by]) * flowRate * 0.5f;
            float naturalGasFlow = (naturalGas[ax, ay] - naturalGas[bx, by]) * flowRate * 0.5f;
            float germFlow = (germs[ax, ay] - germs[bx, by]) * flowRate * 0.5f;

            nextOxygen[ax, ay] -= oxygenFlow;
            nextOxygen[bx, by] += oxygenFlow;
            nextCarbonDioxide[ax, ay] -= co2Flow;
            nextCarbonDioxide[bx, by] += co2Flow;
            nextPollutedOxygen[ax, ay] -= pollutedFlow;
            nextPollutedOxygen[bx, by] += pollutedFlow;
            nextHydrogen[ax, ay] -= hydrogenFlow;
            nextHydrogen[bx, by] += hydrogenFlow;
            nextSteam[ax, ay] -= steamFlow;
            nextSteam[bx, by] += steamFlow;
            nextChlorine[ax, ay] -= chlorineFlow;
            nextChlorine[bx, by] += chlorineFlow;
            nextNaturalGas[ax, ay] -= naturalGasFlow;
            nextNaturalGas[bx, by] += naturalGasFlow;
            nextGerms[ax, ay] -= germFlow;
            nextGerms[bx, by] += germFlow;
        }

        private float GasPermeability(CellKind kind)
        {
            return kind == CellKind.InsulatedTile ? 0.02f :
                kind == CellKind.BunkerDoor ? 0.12f :
                kind == CellKind.ManualAirlock ? 0.16f :
                1f;
        }

        private float TileGasPermeability(int x, int y)
        {
            CellKind kind = cells[x, y];
            if (kind == CellKind.ManualAirlock)
            {
                return airlockOpen[x, y] ? 0.92f : 0.02f;
            }

            if (kind == CellKind.BunkerDoor)
            {
                return IsBunkerDoorClosed(new Vector2Int(x, y)) ? 0.015f : 0.88f;
            }

            return GasPermeability(kind);
        }

        private bool TryCreateRepairJob()
        {
            if (metal < RepairMetalCost)
            {
                return false;
            }

            Vector2Int bestCell = new Vector2Int(-1, -1);
            float bestScore = float.MinValue;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    Vector2Int cell = new Vector2Int(x, y);
                    if (!NeedsAutoRepair(cell) || FindAnyJobAt(cell) != null)
                    {
                        continue;
                    }

                    Job testJob = new Job(JobType.Repair, cell, RepairWorkRequired(cell))
                    {
                        BuildKind = cells[x, y]
                    };

                    if (!CanAnyActiveWorkerReachJob(testJob))
                    {
                        continue;
                    }

                    float score = 1f - Mathf.Clamp01(equipmentCondition[x, y]);
                    if (IsBrokenEquipment(cell))
                    {
                        score += 2f;
                    }

                    if (RequiresPower(cells[x, y]) || cells[x, y] == CellKind.ManualGenerator || cells[x, y] == CellKind.CoalGenerator || cells[x, y] == CellKind.HydrogenGenerator || cells[x, y] == CellKind.NaturalGasGenerator || cells[x, y] == CellKind.SteamTurbine || cells[x, y] == CellKind.SolarPanel)
                    {
                        score += 0.35f;
                    }

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestCell = cell;
                    }
                }
            }

            if (bestCell.x < 0)
            {
                return false;
            }

            Job job = new Job(JobType.Repair, bestCell, RepairWorkRequired(bestCell))
            {
                AutoGenerated = true,
                BuildKind = cells[bestCell.x, bestCell.y],
                Priority = IsBrokenEquipment(bestCell) ? 8 : 5
            };
            jobs.Add(job);
            overlayDirty = true;
            return true;
        }

        private bool TryCreateSweepJob()
        {
            if (DryResourceFreeSpace() <= 0.01f)
            {
                return false;
            }

            Vector2Int bestCell = new Vector2Int(-1, -1);
            float bestScore = float.MinValue;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    Vector2Int cell = new Vector2Int(x, y);
                    if (!HasLooseResource(cell) || FindAnyJobAt(cell) != null)
                    {
                        continue;
                    }

                    Job testJob = new Job(JobType.Sweep, cell, SweepWorkRequired(cell));
                    if (!CanAnyActiveWorkerReachJob(testJob))
                    {
                        continue;
                    }

                    float score = looseResourceAmount[x, y];
                    if (looseResourceKind[x, y] == LooseResourceKind.Metal || looseResourceKind[x, y] == LooseResourceKind.Coal)
                    {
                        score += 8f;
                    }

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestCell = cell;
                    }
                }
            }

            if (bestCell.x < 0)
            {
                return false;
            }

            Job job = new Job(JobType.Sweep, bestCell, SweepWorkRequired(bestCell))
            {
                AutoGenerated = true,
                Priority = looseResourceKind[bestCell.x, bestCell.y] == LooseResourceKind.Metal ? 5 : 4
            };
            jobs.Add(job);
            overlayDirty = true;
            return true;
        }

        private bool TryCreateEmptyBottleJob()
        {
            if (!HasStoredLiquidForBottleEmptier())
            {
                return false;
            }

            Vector2Int bestCell = new Vector2Int(-1, -1);
            float bestScore = float.MinValue;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    Vector2Int cell = new Vector2Int(x, y);
                    if (cells[x, y] != CellKind.BottleEmptier ||
                        !CanUseEquipment(cell) ||
                        FindAnyJobAt(cell) != null ||
                        !CanEmptyBottleAt(cell))
                    {
                        continue;
                    }

                    Job testJob = new Job(JobType.EmptyBottle, cell, BottleEmptierWorkRequired);
                    if (!CanAnyActiveWorkerReachJob(testJob))
                    {
                        continue;
                    }

                    float score = pollutedWater > 0.5f ? 20f + pollutedWater : CleanWaterAvailableForBottleEmptier();
                    if (TryFindBottleEmptierOutput(cell, out Vector2Int output) && output.y < cell.y)
                    {
                        score += 2f;
                    }

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestCell = cell;
                    }
                }
            }

            if (bestCell.x < 0)
            {
                return false;
            }

            Job job = new Job(JobType.EmptyBottle, bestCell, BottleEmptierWorkRequired)
            {
                AutoGenerated = true,
                Priority = pollutedWater > 0.5f ? 6 : 4
            };
            jobs.Add(job);
            overlayDirty = true;
            return true;
        }

        private bool CanAnyActiveWorkerReachJob(Job job)
        {
            foreach (Worker worker in workers)
            {
                if (worker.Health > 0f && TryFindPathToJob(worker.Cell, job, out _))
                {
                    return true;
                }
            }

            return false;
        }

        private void EnsureMaintenanceJobs()
        {
            for (int i = jobs.Count - 1; i >= 0; i--)
            {
                Job job = jobs[i];
                if (job.AutoGenerated && job.AssignedWorker == null && !IsJobValid(job))
                {
                    jobs.RemoveAt(i);
                    overlayDirty = true;
                }
            }

            foreach (Worker worker in workers)
            {
                if (NeedsRescue(worker) && !HasRescueJobFor(worker.Name) && TryCreateRescueJob(worker))
                {
                    return;
                }
            }

            foreach (Worker worker in workers)
            {
                if (NeedsTreatment(worker) && !HasTreatmentJobFor(worker.Name) && TryCreateTreatmentJob(worker))
                {
                    return;
                }
            }

            foreach (Worker worker in workers)
            {
                if (NeedsToilet(worker) && !HasToiletJobFor(worker.Name) && TryCreateToiletJob(worker))
                {
                    return;
                }
            }

            foreach (Worker worker in workers)
            {
                if (NeedsHandWash(worker) && !HasWashHandsJobFor(worker.Name) && TryCreateWashHandsJob(worker))
                {
                    return;
                }
            }

            foreach (Worker worker in workers)
            {
                if (NeedsFood(worker) && !HasEatJobFor(worker.Name) && TryCreateEatJob(worker))
                {
                    return;
                }
            }

            foreach (Worker worker in workers)
            {
                if (NeedsRelaxation(worker) && !HasRelaxJobFor(worker.Name) && TryCreateRelaxJob(worker))
                {
                    return;
                }
            }

            if (TryCreateSweepJob())
            {
                return;
            }

            if (TryCreateRepairJob())
            {
                return;
            }

            if (TryCreateGroomHatchJob())
            {
                return;
            }

            if (power < maxPower * 0.55f)
            {
                UpdateAutomationWires();
                bool controlledGenerators = CountAutomationControlledGenerators() > 0;
                float requestThreshold = controlledGenerators ? maxPower * SmartBatteryLowThreshold : maxPower * 0.55f;
                if (power < requestThreshold)
                {
                    for (int y = 0; y < WorldHeight; y++)
                    {
                        for (int x = 0; x < WorldWidth; x++)
                        {
                            Vector2Int cell = new Vector2Int(x, y);
                            if (cells[x, y] == CellKind.ManualGenerator && CanGeneratorOperate(cell) && FindJobAt(cell, JobType.OperateGenerator) == null)
                            {
                                Job job = new Job(JobType.OperateGenerator, cell, 3.2f)
                                {
                                    AutoGenerated = true,
                                    Priority = power < maxPower * 0.18f ? 9 : 6
                                };
                                jobs.Add(job);
                                overlayDirty = true;
                                return;
                            }
                        }
                    }
                }
            }

            if (water < 160f)
            {
                for (int y = 0; y < WorldHeight; y++)
                {
                    for (int x = 0; x < WorldWidth; x++)
                    {
                        Vector2Int cell = new Vector2Int(x, y);
                        if (cells[x, y] == CellKind.WaterPump &&
                            CanUseEquipment(cell) &&
                            TryFindAdjacentWater(cell, out _) &&
                            FindJobAt(cell, JobType.PumpWater) == null)
                        {
                            Job job = new Job(JobType.PumpWater, cell, 2.5f)
                            {
                                AutoGenerated = true,
                                Priority = water < workers.Count * 12f ? 8 : 5
                            };
                            jobs.Add(job);
                            overlayDirty = true;
                            return;
                        }
                    }
                }
            }

            if (TryCreateEmptyBottleJob())
            {
                return;
            }

            if (researchPoints < 32f && power > 4f)
            {
                for (int y = 0; y < WorldHeight; y++)
                {
                    for (int x = 0; x < WorldWidth; x++)
                    {
                        Vector2Int cell = new Vector2Int(x, y);
                        if (cells[x, y] == CellKind.ResearchStation && CanPoweredMachineRun(cell) && FindJobAt(cell, JobType.Research) == null)
                        {
                            Job job = new Job(JobType.Research, cell, 4.8f)
                            {
                                AutoGenerated = true,
                                Priority = !techAirSystems ? 7 : 4
                            };
                            jobs.Add(job);
                            overlayDirty = true;
                            return;
                        }
                    }
                }
            }

            if (techFoodPreparation && food < workers.Count * 2100f && water >= 4f && dirt >= 1f && power > 2f)
            {
                for (int y = 0; y < WorldHeight; y++)
                {
                    for (int x = 0; x < WorldWidth; x++)
                    {
                        Vector2Int cell = new Vector2Int(x, y);
                        if (cells[x, y] == CellKind.MicrobeMusher && CanPoweredMachineRun(cell) && FindJobAt(cell, JobType.Cook) == null)
                        {
                            Job job = new Job(JobType.Cook, cell, 3f)
                            {
                                AutoGenerated = true,
                                Priority = food < workers.Count * 1200f ? 8 : 5
                            };
                            jobs.Add(job);
                            overlayDirty = true;
                            return;
                        }
                    }
                }
            }

            if (techFoodPreparation && (pollutedDirt >= CropTendPollutedDirtCost || dirt >= CropTendDirtFallbackCost) && TryCreateTendCropJob())
            {
                return;
            }

            if (techPowerRegulation && refinedMetal < 40f && metal >= RockCrusherOrePerJob && power > RockCrusherPowerCost)
            {
                for (int y = 0; y < WorldHeight; y++)
                {
                    for (int x = 0; x < WorldWidth; x++)
                    {
                        Vector2Int cell = new Vector2Int(x, y);
                        if (cells[x, y] == CellKind.RockCrusher && CanPoweredMachineRun(cell) && FindJobAt(cell, JobType.RefineMetal) == null)
                        {
                            Job job = new Job(JobType.RefineMetal, cell, 3.6f)
                            {
                                AutoGenerated = true,
                                Priority = refinedMetal < 8f ? 7 : 4
                            };
                            jobs.Add(job);
                            overlayDirty = true;
                            return;
                        }
                    }
                }
            }

            if (pollutedDirt >= 4f)
            {
                for (int y = 0; y < WorldHeight; y++)
                {
                    for (int x = 0; x < WorldWidth; x++)
                    {
                        Vector2Int cell = new Vector2Int(x, y);
                        if (cells[x, y] == CellKind.Compost && CanUseEquipment(cell) && FindJobAt(cell, JobType.Compost) == null)
                        {
                            Job job = new Job(JobType.Compost, cell, 3.1f)
                            {
                                AutoGenerated = true,
                                Priority = pollutedDirt > 28f ? 7 : 5
                            };
                            jobs.Add(job);
                            overlayDirty = true;
                            return;
                        }
                    }
                }
            }

            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] == CellKind.Planter && plantGrowth[x, y] >= 1f && FindJobAt(new Vector2Int(x, y), JobType.Harvest) == null)
                    {
                        Job job = new Job(JobType.Harvest, new Vector2Int(x, y), 2f)
                        {
                            AutoGenerated = true,
                            Priority = food < workers.Count * 1500f ? 7 : 4
                        };
                        jobs.Add(job);
                        overlayDirty = true;
                    }
                }
            }
        }

        private bool TryCreateTendCropJob()
        {
            if (CountCells(CellKind.FarmStation) == 0 || (pollutedDirt < CropTendPollutedDirtCost && dirt < CropTendDirtFallbackCost))
            {
                return false;
            }

            Vector2Int bestCrop = new Vector2Int(-1, -1);
            float bestScore = -1f;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    Vector2Int crop = new Vector2Int(x, y);
                    if (!IsCropTendingTarget(crop) || FindJobAt(crop, JobType.TendCrop) != null || !TryFindFarmStationForCrop(crop, out _))
                    {
                        continue;
                    }

                    float score = (1f - plantGrowth[x, y]) * 100f + Mathf.Clamp(12f - cropTendedSeconds[x, y], 0f, 12f);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestCrop = crop;
                    }
                }
            }

            if (bestCrop.x < 0)
            {
                return false;
            }

            Job job = new Job(JobType.TendCrop, bestCrop, 2.4f)
            {
                AutoGenerated = true,
                Priority = food < workers.Count * 1800f ? 6 : 4
            };
            jobs.Add(job);
            overlayDirty = true;
            return true;
        }

        private bool TryCreateGroomHatchJob()
        {
            if (hatches.Count == 0 || CountCells(CellKind.RanchingStation) == 0)
            {
                return false;
            }

            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    Vector2Int station = new Vector2Int(x, y);
                    if (cells[x, y] != CellKind.RanchingStation ||
                        FindJobAt(station, JobType.GroomHatch) != null ||
                        FindGroomableHatch(station) == null)
                    {
                        continue;
                    }

                    Job job = new Job(JobType.GroomHatch, station, 2.6f)
                    {
                        AutoGenerated = true,
                        Priority = CountUngroomedHatches() > 1 ? 6 : 4
                    };
                    if (!CanAnyActiveWorkerReachJob(job))
                    {
                        continue;
                    }

                    jobs.Add(job);
                    overlayDirty = true;
                    return true;
                }
            }

            return false;
        }

        private HatchCritter FindGroomableHatch(Vector2Int station)
        {
            HatchCritter best = null;
            float bestScore = float.MaxValue;
            for (int i = 0; i < hatches.Count; i++)
            {
                HatchCritter hatch = hatches[i];
                if (hatch == null || hatch.GroomedSeconds > 8f)
                {
                    continue;
                }

                int distance = Mathf.Abs(hatch.Cell.x - station.x) + Mathf.Abs(hatch.Cell.y - station.y);
                if (distance > HatchGroomRange)
                {
                    continue;
                }

                float score = distance + hatch.Happiness * 0.04f;
                if (score < bestScore)
                {
                    bestScore = score;
                    best = hatch;
                }
            }

            return best;
        }

        private int CountUngroomedHatches()
        {
            int count = 0;
            for (int i = 0; i < hatches.Count; i++)
            {
                if (hatches[i].GroomedSeconds <= 8f)
                {
                    count++;
                }
            }

            return count;
        }

        private int CountGroomedHatches()
        {
            int count = 0;
            for (int i = 0; i < hatches.Count; i++)
            {
                if (hatches[i].GroomedSeconds > 0f)
                {
                    count++;
                }
            }

            return count;
        }

        private bool HasHatchEdibleDebris()
        {
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (IsHatchEdible(looseResourceKind[x, y]) && looseResourceAmount[x, y] > 0.05f)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void UpdateWorkers(float deltaTime)
        {
            int activeWorkers = 0;
            foreach (Worker worker in workers)
            {
                UpdateWorkerVitals(worker, deltaTime);
                if (worker.Health <= 0f)
                {
                    ClearAssignment(worker);
                    worker.Activity = "Incapacitated";
                    continue;
                }

                activeWorkers++;

                if (worker.StressBreakSeconds > 0f)
                {
                    UpdateStressBreak(worker, deltaTime);
                    continue;
                }

                if (ShouldTriggerStressBreak(worker))
                {
                    StartStressBreak(worker);
                    UpdateStressBreak(worker, deltaTime);
                    continue;
                }

                if (worker.AssignedJob == null || !IsJobValid(worker.AssignedJob))
                {
                    ClearAssignment(worker);
                    AssignBestJob(worker);
                }

                if (worker.AssignedJob == null)
                {
                    worker.Activity = "Idle";
                    continue;
                }

                MoveOrWork(worker, deltaTime);
            }

            if (activeWorkers == 0 && workers.Count > 0)
            {
                TriggerColonyFailure("All duplicants are incapacitated.");
            }
        }

        private void UpdateWorkerVitals(Worker worker, float deltaTime)
        {
            worker.Cell = WorldToCell(worker.Transform.position);
            if (worker.Health <= 0f)
            {
                worker.Health = 0f;
                worker.IncapacitatedSeconds += deltaTime;
                worker.Activity = "Incapacitated";
                return;
            }

            worker.IncapacitatedSeconds = 0f;

            bool sleeping = IsSleeping(worker);
            worker.Bladder = Mathf.Clamp(worker.Bladder + (sleeping ? 0.18f : 0.34f) * deltaTime, 0f, 100f);
            if (worker.Bladder >= 100f)
            {
                TriggerBladderAccident(worker);
            }
            else if (worker.Bladder >= 86f)
            {
                worker.Stress = Mathf.Min(100f, worker.Stress + 1.2f * deltaTime);
            }

            if (!sleeping)
            {
                float fatigueRate = IsRestTime() ? 0.72f : 0.34f;
                worker.Fatigue = Mathf.Min(100f, worker.Fatigue + fatigueRate * deltaTime);
            }

            if (IsInside(worker.Cell.x, worker.Cell.y) && IsPassable(worker.Cell.x, worker.Cell.y))
            {
                bool suitProtected = UseSuitProtection(worker, deltaTime);
                if (!suitProtected)
                {
                    float breath = 0.018f * deltaTime;
                    oxygen[worker.Cell.x, worker.Cell.y] = Mathf.Max(0f, oxygen[worker.Cell.x, worker.Cell.y] - breath);
                    carbonDioxide[worker.Cell.x, worker.Cell.y] = Mathf.Min(2.5f, carbonDioxide[worker.Cell.x, worker.Cell.y] + breath * 0.82f);
                    if (pollutedOxygen[worker.Cell.x, worker.Cell.y] > 0.12f)
                    {
                        float exposure = pollutedOxygen[worker.Cell.x, worker.Cell.y] * (0.7f + germs[worker.Cell.x, worker.Cell.y] * 1.8f) * deltaTime;
                        worker.GermExposure = Mathf.Min(100f, worker.GermExposure + exposure);
                        worker.Stress = Mathf.Min(100f, worker.Stress + 0.7f * deltaTime);
                    }
                    else
                    {
                        worker.GermExposure = Mathf.Max(0f, worker.GermExposure - 0.18f * deltaTime);
                    }
                }
                else
                {
                    worker.GermExposure = Mathf.Max(0f, worker.GermExposure - 0.12f * deltaTime);
                }

                if (!suitProtected && chlorine[worker.Cell.x, worker.Cell.y] > ChlorineExposureThreshold)
                {
                    float chlorineSeverity = Mathf.Clamp01(chlorine[worker.Cell.x, worker.Cell.y] / 1.4f);
                    chlorineExposureSeconds += deltaTime;
                    worker.Stress = Mathf.Min(100f, worker.Stress + 1.7f * chlorineSeverity * deltaTime);
                    float beforeHealth = worker.Health;
                    worker.Health = Mathf.Max(0f, worker.Health - ChlorineDamageRate * chlorineSeverity * deltaTime);
                    chlorineHealthDamage += Mathf.Max(0f, beforeHealth - worker.Health);
                }

                if (worker.GermExposure >= 40f)
                {
                    worker.Sickness = Mathf.Min(100f, worker.Sickness + 0.42f * deltaTime);
                }
                else
                {
                    worker.Sickness = Mathf.Max(0f, worker.Sickness - 0.08f * deltaTime);
                }

                float fatigueWorkFactor = worker.Fatigue > 88f ? 0.55f : worker.Fatigue > 68f ? 0.75f : 1f;
                float sicknessWorkFactor = worker.Sickness > 65f ? 0.7f : worker.Sickness > 35f ? 0.85f : 1f;
                float localTemperature = temperature[worker.Cell.x, worker.Cell.y];
                float temperatureWorkFactor = localTemperature < 0f || localTemperature > 52f ? 0.58f :
                    localTemperature < 8f || localTemperature > 40f ? 0.82f : 1f;
                float effectiveWorkSpeed = WorkerSkillSpeedMultiplier(worker) * fatigueWorkFactor * sicknessWorkFactor * temperatureWorkFactor;
                float localDecor = DecorScoreAt(worker.Cell.x, worker.Cell.y);
                ApplyWorkerThermalExposure(worker, localTemperature, steam[worker.Cell.x, worker.Cell.y], suitProtected, deltaTime);

                float localPressure = TileGasTotal(worker.Cell.x, worker.Cell.y);
                if (!suitProtected && localPressure > OverpressureStressThreshold)
                {
                    float pressureSeverity = Mathf.Clamp01((localPressure - OverpressureStressThreshold) / Mathf.Max(0.001f, 2.8f - OverpressureStressThreshold));
                    worker.Stress = Mathf.Min(100f, worker.Stress + OverpressureStressRate * pressureSeverity * deltaTime);
                    overpressureExposureSeconds += deltaTime;
                    if (localPressure > OverpressureDamageThreshold)
                    {
                        float beforeHealth = worker.Health;
                        worker.Health = Mathf.Max(0f, worker.Health - OverpressureDamageRate * pressureSeverity * deltaTime);
                        overpressureHealthDamage += Mathf.Max(0f, beforeHealth - worker.Health);
                    }
                }

                if (!suitProtected && oxygen[worker.Cell.x, worker.Cell.y] < 0.12f)
                {
                    worker.Stress = Mathf.Min(100f, worker.Stress + 4f * deltaTime);
                    worker.Health = Mathf.Max(0f, worker.Health - 3.5f * deltaTime);
                    worker.WorkSpeed = Mathf.Min(0.65f, effectiveWorkSpeed);
                }
                else
                {
                    worker.Stress = Mathf.Max(0f, worker.Stress - 0.6f * deltaTime);
                    if (worker.Calories > 500f)
                    {
                        worker.Health = Mathf.Min(100f, worker.Health + 0.35f * deltaTime);
                    }

                    worker.WorkSpeed = effectiveWorkSpeed;
                }

                if (!sleeping && localDecor > 0.05f)
                {
                    worker.Stress = Mathf.Max(0f, worker.Stress - DecorStressReliefRate(localDecor) * deltaTime);
                }

                float roomRelief = RoomPassiveStressReliefRate(worker.Cell.x, worker.Cell.y);
                if (!sleeping && roomRelief > 0f)
                {
                    worker.Stress = Mathf.Max(0f, worker.Stress - roomRelief * deltaTime);
                }

                UpdateWorkerMorale(worker, deltaTime);
            }

            if (worker.Sickness >= 70f)
            {
                worker.Health = Mathf.Max(0f, worker.Health - 0.9f * deltaTime);
                worker.Stress = Mathf.Min(100f, worker.Stress + 1.1f * deltaTime);
            }

            if (worker.Fatigue >= 92f && !sleeping)
            {
                worker.Stress = Mathf.Min(100f, worker.Stress + 2.4f * deltaTime);
                worker.Health = Mathf.Max(0f, worker.Health - 0.8f * deltaTime);
            }

            worker.Calories = Mathf.Max(0f, worker.Calories - (sleeping ? 0.05f : 0.22f) * deltaTime);
            if (worker.Calories < 900f && food >= 700f && (CountCells(CellKind.MessTable) == 0 || worker.Calories < 250f))
            {
                EatStoredFood(worker, false);
            }

            if (worker.Calories <= 0f)
            {
                worker.Stress = Mathf.Min(100f, worker.Stress + 5f * deltaTime);
                worker.Health = Mathf.Max(0f, worker.Health - 4.5f * deltaTime);
            }
        }

        private void ApplyWorkerThermalExposure(Worker worker, float localTemperature, float localSteam, bool suitProtected, float deltaTime)
        {
            if (worker == null)
            {
                return;
            }

            if (suitProtected)
            {
                worker.HeatExposure = Mathf.Max(0f, worker.HeatExposure - ThermalExposureRecoveryRate * deltaTime);
                worker.ChillExposure = Mathf.Max(0f, worker.ChillExposure - ThermalExposureRecoveryRate * deltaTime);
                return;
            }

            float heatSeverity = localTemperature <= ThermalHeatStressTemperature ? 0f :
                Mathf.Clamp01((localTemperature - ThermalHeatStressTemperature) / Mathf.Max(0.001f, ThermalHeatDamageTemperature - ThermalHeatStressTemperature));
            float coldSeverity = localTemperature >= ThermalColdStressTemperature ? 0f :
                Mathf.Clamp01((ThermalColdStressTemperature - localTemperature) / Mathf.Max(0.001f, ThermalColdStressTemperature - ThermalColdDamageTemperature));
            float scaldSeverity = localSteam <= SteamScaldMassThreshold || localTemperature <= 52f ? 0f :
                Mathf.Clamp01(localSteam / 2.8f) * Mathf.Clamp01((localTemperature - 52f) / 48f);
            heatSeverity = Mathf.Clamp01(heatSeverity + scaldSeverity * 1.35f);

            if (heatSeverity > 0.001f)
            {
                float previous = worker.HeatExposure;
                worker.HeatExposure = Mathf.Min(100f, worker.HeatExposure + heatSeverity * ThermalExposureBuildRate * deltaTime);
                worker.ChillExposure = Mathf.Max(0f, worker.ChillExposure - ThermalExposureRecoveryRate * 0.65f * deltaTime);
                worker.Stress = Mathf.Min(100f, worker.Stress + Mathf.Lerp(1.2f, 3.2f, heatSeverity) * deltaTime);
                thermalExposureSeconds += deltaTime;
                if (previous < ThermalInjuryExposureThreshold && worker.HeatExposure >= ThermalInjuryExposureThreshold)
                {
                    heatStrokeCases++;
                    Log(worker.Name + " is suffering heat injury. Cool the area or use atmo suits.");
                }

                if (worker.HeatExposure >= ThermalInjuryExposureThreshold || localTemperature > ThermalHeatDamageTemperature || scaldSeverity > 0.08f)
                {
                    float damageSeverity = Mathf.Max(heatSeverity, Mathf.InverseLerp(ThermalInjuryExposureThreshold, 100f, worker.HeatExposure));
                    float beforeHealth = worker.Health;
                    worker.Health = Mathf.Max(0f, worker.Health - ThermalExposureDamageRate * damageSeverity * deltaTime);
                    thermalHealthDamage += Mathf.Max(0f, beforeHealth - worker.Health);
                }
            }
            else
            {
                worker.HeatExposure = Mathf.Max(0f, worker.HeatExposure - ThermalExposureRecoveryRate * deltaTime);
            }

            if (coldSeverity > 0.001f)
            {
                float previous = worker.ChillExposure;
                worker.ChillExposure = Mathf.Min(100f, worker.ChillExposure + coldSeverity * ThermalExposureBuildRate * deltaTime);
                worker.HeatExposure = Mathf.Max(0f, worker.HeatExposure - ThermalExposureRecoveryRate * 0.65f * deltaTime);
                worker.Stress = Mathf.Min(100f, worker.Stress + Mathf.Lerp(1.0f, 2.8f, coldSeverity) * deltaTime);
                thermalExposureSeconds += deltaTime;
                if (previous < ThermalInjuryExposureThreshold && worker.ChillExposure >= ThermalInjuryExposureThreshold)
                {
                    hypothermiaCases++;
                    Log(worker.Name + " is suffering hypothermia. Warm the area or use atmo suits.");
                }

                if (worker.ChillExposure >= ThermalInjuryExposureThreshold || localTemperature < ThermalColdDamageTemperature)
                {
                    float damageSeverity = Mathf.Max(coldSeverity, Mathf.InverseLerp(ThermalInjuryExposureThreshold, 100f, worker.ChillExposure));
                    float beforeHealth = worker.Health;
                    worker.Health = Mathf.Max(0f, worker.Health - ThermalExposureDamageRate * damageSeverity * deltaTime);
                    thermalHealthDamage += Mathf.Max(0f, beforeHealth - worker.Health);
                }
            }
            else
            {
                worker.ChillExposure = Mathf.Max(0f, worker.ChillExposure - ThermalExposureRecoveryRate * deltaTime);
            }
        }

        private void TriggerBladderAccident(Worker worker)
        {
            worker.Bladder = 18f;
            worker.Stress = Mathf.Min(100f, worker.Stress + 18f);
            worker.GermExposure = Mathf.Min(100f, worker.GermExposure + 16f);

            if (IsInside(worker.Cell.x, worker.Cell.y) && IsPassable(worker.Cell.x, worker.Cell.y))
            {
                pollutedOxygen[worker.Cell.x, worker.Cell.y] = Mathf.Min(2.5f, pollutedOxygen[worker.Cell.x, worker.Cell.y] + 0.75f);
                germs[worker.Cell.x, worker.Cell.y] = Mathf.Clamp01(germs[worker.Cell.x, worker.Cell.y] + 0.55f);
                gasDirty = true;
                overlayDirty = true;
            }

            Log(worker.Name + " had an accident. Build more outhouses.");
        }

        private bool ShouldTriggerStressBreak(Worker worker)
        {
            return worker != null && worker.Health > 0f && worker.StressBreakSeconds <= 0f && worker.Stress >= 98f;
        }

        private void StartStressBreak(Worker worker)
        {
            ClearAssignment(worker);
            worker.StressBreakSeconds = 9f;
            worker.StressBreakPulseTimer = 0f;
            worker.Activity = "Stress Break";
            AddStressBreakPollution(worker, 0.32f, 0.14f);
            Log(worker.Name + " hit critical stress and is having a stress break.");
        }

        private void UpdateStressBreak(Worker worker, float deltaTime)
        {
            if (worker.AssignedJob != null)
            {
                ClearAssignment(worker);
            }

            worker.Activity = "Stress Break";
            worker.Path.Clear();
            worker.PathIndex = 0;
            worker.StressBreakSeconds = Mathf.Max(0f, worker.StressBreakSeconds - deltaTime);
            worker.StressBreakPulseTimer -= deltaTime;
            worker.Stress = Mathf.Max(54f, worker.Stress - 4.4f * deltaTime);
            worker.Fatigue = Mathf.Min(100f, worker.Fatigue + 0.16f * deltaTime);
            worker.Calories = Mathf.Max(0f, worker.Calories - 0.08f * deltaTime);

            if (worker.StressBreakPulseTimer <= 0f)
            {
                worker.StressBreakPulseTimer = 1.5f;
                AddStressBreakPollution(worker, 0.12f, 0.06f);
            }

            if (worker.StressBreakSeconds <= 0f)
            {
                worker.Stress = Mathf.Min(worker.Stress, 60f);
                worker.Activity = "Idle";
                Log(worker.Name + " recovered from a stress break.");
            }
        }

        private void AddStressBreakPollution(Worker worker, float pollutedAmount, float germAmount)
        {
            if (worker == null || !IsInside(worker.Cell.x, worker.Cell.y) || !IsPassable(worker.Cell.x, worker.Cell.y))
            {
                return;
            }

            pollutedOxygen[worker.Cell.x, worker.Cell.y] = Mathf.Min(2.5f, pollutedOxygen[worker.Cell.x, worker.Cell.y] + pollutedAmount);
            germs[worker.Cell.x, worker.Cell.y] = Mathf.Clamp01(germs[worker.Cell.x, worker.Cell.y] + germAmount);
            gasDirty = true;
            overlayDirty = true;
        }

        private void AssignBestJob(Worker worker)
        {
            if (NeedsTreatment(worker) && (TryAssignTreatmentJob(worker) || (TryCreateTreatmentJob(worker) && TryAssignTreatmentJob(worker))))
            {
                return;
            }

            if (NeedsToilet(worker) && (TryAssignToiletJob(worker) || (TryCreateToiletJob(worker) && TryAssignToiletJob(worker))))
            {
                return;
            }

            if (NeedsHandWash(worker) && (TryAssignWashHandsJob(worker) || (TryCreateWashHandsJob(worker) && TryAssignWashHandsJob(worker))))
            {
                return;
            }

            if (NeedsFood(worker) && (TryAssignEatJob(worker) || (TryCreateEatJob(worker) && TryAssignEatJob(worker))))
            {
                return;
            }

            if (NeedsRelaxation(worker) && (TryAssignRelaxJob(worker) || (TryCreateRelaxJob(worker) && TryAssignRelaxJob(worker))))
            {
                return;
            }

            if (ShouldSleep(worker) && TryAssignSleepJob(worker))
            {
                return;
            }

            Job bestJob = null;
            List<Vector2Int> bestPath = null;
            int bestScore = int.MinValue;

            foreach (Job job in jobs)
            {
                if (!CanWorkerTakeJob(worker, job))
                {
                    continue;
                }

                if (TryFindPathToJob(worker.Cell, job, out List<Vector2Int> path))
                {
                    int score = JobAssignmentScore(job, path.Count);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestPath = path;
                        bestJob = job;
                    }
                }
            }

            if (bestJob == null)
            {
                return;
            }

            AssignJob(worker, bestJob, bestPath);
        }

        private void AssignJob(Worker worker, Job job, List<Vector2Int> path)
        {
            job.AssignedWorker = worker;
            worker.AssignedJob = job;
            worker.Path = path ?? new List<Vector2Int>();
            worker.PathIndex = 0;
            worker.Activity = "Going to " + JobLabel(job);
            overlayDirty = true;
        }

        private bool CanWorkerTakeJob(Worker worker, Job job)
        {
            if (job.AssignedWorker != null || !IsJobValid(job))
            {
                return false;
            }

            if (job.Type == JobType.Rescue)
            {
                Worker patient = FindWorkerByName(job.TargetWorkerName);
                return patient != null && patient != worker && worker.Health > 0f;
            }

            return string.IsNullOrEmpty(job.TargetWorkerName) || job.TargetWorkerName == worker.Name;
        }

        private void UpdateJobAges(float deltaTime)
        {
            if (deltaTime <= 0f || jobs.Count == 0)
            {
                return;
            }

            foreach (Job job in jobs)
            {
                if (job.AssignedWorker == null && !job.Cancelled)
                {
                    job.AgeSeconds = Mathf.Min(JobAgingMaxSeconds, job.AgeSeconds + deltaTime);
                }
                else if (job.AssignedWorker != null)
                {
                    job.AgeSeconds = Mathf.Max(0f, job.AgeSeconds - deltaTime * 0.5f);
                }
            }
        }

        private bool TryCreateRescueJob(Worker patient)
        {
            if (!NeedsRescue(patient) || CountCells(CellKind.MedicalCot) == 0)
            {
                return false;
            }

            Job job = new Job(JobType.Rescue, patient.Cell, RescueWorkRequired(patient))
            {
                AutoGenerated = true,
                Priority = RescuePriority(patient),
                TargetWorkerName = patient.Name
            };

            if (!CanAnyOtherActiveWorkerReachJob(patient, job))
            {
                return false;
            }

            jobs.Add(job);
            overlayDirty = true;
            return true;
        }

        private bool CanAnyOtherActiveWorkerReachJob(Worker patient, Job job)
        {
            foreach (Worker worker in workers)
            {
                if (worker == patient || worker.Health <= 0f)
                {
                    continue;
                }

                if (TryFindPathToJob(worker.Cell, job, out _))
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryAssignTreatmentJob(Worker worker)
        {
            Job bestJob = null;
            List<Vector2Int> bestPath = null;
            int bestDistance = int.MaxValue;

            foreach (Job job in jobs)
            {
                if (job.Type != JobType.Treat || !CanWorkerTakeJob(worker, job))
                {
                    continue;
                }

                if (TryFindPathToJob(worker.Cell, job, out List<Vector2Int> path) && path.Count < bestDistance)
                {
                    bestDistance = path.Count;
                    bestPath = path;
                    bestJob = job;
                }
            }

            if (bestJob == null)
            {
                return false;
            }

            bestJob.Priority = TreatmentPriority(worker);
            AssignJob(worker, bestJob, bestPath);
            return true;
        }

        private bool TryCreateTreatmentJob(Worker worker)
        {
            Vector2Int bestCell = new Vector2Int(-1, -1);
            int bestDistance = int.MaxValue;

            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    Vector2Int cotCell = new Vector2Int(x, y);
                    if (cells[x, y] != CellKind.MedicalCot || IsMedicalCotReserved(cotCell))
                    {
                        continue;
                    }

                    Job testJob = new Job(JobType.Treat, cotCell, TreatmentWorkRequired(worker))
                    {
                        TargetWorkerName = worker.Name
                    };

                    if (TryFindPathToJob(worker.Cell, testJob, out List<Vector2Int> path) && path.Count < bestDistance)
                    {
                        bestDistance = path.Count;
                        bestCell = cotCell;
                    }
                }
            }

            if (bestCell.x < 0)
            {
                return false;
            }

            Job job = new Job(JobType.Treat, bestCell, TreatmentWorkRequired(worker))
            {
                AutoGenerated = true,
                Priority = TreatmentPriority(worker),
                TargetWorkerName = worker.Name
            };
            jobs.Add(job);
            overlayDirty = true;
            return true;
        }

        private bool TryAssignToiletJob(Worker worker)
        {
            Job bestJob = null;
            List<Vector2Int> bestPath = null;
            int bestDistance = int.MaxValue;

            foreach (Job job in jobs)
            {
                if (job.Type != JobType.UseToilet || !CanWorkerTakeJob(worker, job))
                {
                    continue;
                }

                if (TryFindPathToJob(worker.Cell, job, out List<Vector2Int> path) && path.Count < bestDistance)
                {
                    bestDistance = path.Count;
                    bestPath = path;
                    bestJob = job;
                }
            }

            if (bestJob == null)
            {
                return false;
            }

            bestJob.Priority = ToiletPriority(worker);
            AssignJob(worker, bestJob, bestPath);
            return true;
        }

        private bool TryCreateToiletJob(Worker worker)
        {
            Vector2Int bestCell = new Vector2Int(-1, -1);
            int bestDistance = int.MaxValue;

            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    Vector2Int toiletCell = new Vector2Int(x, y);
                    if (cells[x, y] != CellKind.Outhouse || IsOuthouseReserved(toiletCell))
                    {
                        continue;
                    }

                    Job testJob = new Job(JobType.UseToilet, toiletCell, ToiletWorkRequired(worker))
                    {
                        TargetWorkerName = worker.Name
                    };

                    if (TryFindPathToJob(worker.Cell, testJob, out List<Vector2Int> path) && path.Count < bestDistance)
                    {
                        bestDistance = path.Count;
                        bestCell = toiletCell;
                    }
                }
            }

            if (bestCell.x < 0)
            {
                return false;
            }

            Job job = new Job(JobType.UseToilet, bestCell, ToiletWorkRequired(worker))
            {
                AutoGenerated = true,
                Priority = ToiletPriority(worker),
                TargetWorkerName = worker.Name
            };
            jobs.Add(job);
            overlayDirty = true;
            return true;
        }

        private bool TryAssignWashHandsJob(Worker worker)
        {
            Job bestJob = null;
            List<Vector2Int> bestPath = null;
            int bestDistance = int.MaxValue;

            foreach (Job job in jobs)
            {
                if (job.Type != JobType.WashHands || !CanWorkerTakeJob(worker, job))
                {
                    continue;
                }

                if (TryFindPathToJob(worker.Cell, job, out List<Vector2Int> path) && path.Count < bestDistance)
                {
                    bestDistance = path.Count;
                    bestPath = path;
                    bestJob = job;
                }
            }

            if (bestJob == null)
            {
                return false;
            }

            bestJob.Priority = WashHandsPriority(worker);
            AssignJob(worker, bestJob, bestPath);
            return true;
        }

        private bool TryCreateWashHandsJob(Worker worker)
        {
            if (water <= 0.05f)
            {
                return false;
            }

            Vector2Int bestCell = new Vector2Int(-1, -1);
            int bestDistance = int.MaxValue;

            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    Vector2Int basinCell = new Vector2Int(x, y);
                    if (cells[x, y] != CellKind.WashBasin || !CanUseEquipment(basinCell) || IsWashBasinReserved(basinCell))
                    {
                        continue;
                    }

                    Job testJob = new Job(JobType.WashHands, basinCell, WashHandsWorkRequired(worker))
                    {
                        TargetWorkerName = worker.Name
                    };

                    if (TryFindPathToJob(worker.Cell, testJob, out List<Vector2Int> path) && path.Count < bestDistance)
                    {
                        bestDistance = path.Count;
                        bestCell = basinCell;
                    }
                }
            }

            if (bestCell.x < 0)
            {
                return false;
            }

            Job job = new Job(JobType.WashHands, bestCell, WashHandsWorkRequired(worker))
            {
                AutoGenerated = true,
                Priority = WashHandsPriority(worker),
                TargetWorkerName = worker.Name
            };
            jobs.Add(job);
            overlayDirty = true;
            return true;
        }

        private bool TryAssignEatJob(Worker worker)
        {
            Job bestJob = null;
            List<Vector2Int> bestPath = null;
            int bestDistance = int.MaxValue;

            foreach (Job job in jobs)
            {
                if (job.Type != JobType.Eat || !CanWorkerTakeJob(worker, job))
                {
                    continue;
                }

                if (TryFindPathToJob(worker.Cell, job, out List<Vector2Int> path) && path.Count < bestDistance)
                {
                    bestDistance = path.Count;
                    bestPath = path;
                    bestJob = job;
                }
            }

            if (bestJob == null)
            {
                return false;
            }

            bestJob.Priority = EatPriority(worker);
            AssignJob(worker, bestJob, bestPath);
            return true;
        }

        private bool TryCreateEatJob(Worker worker)
        {
            Vector2Int bestCell = new Vector2Int(-1, -1);
            int bestDistance = int.MaxValue;

            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    Vector2Int tableCell = new Vector2Int(x, y);
                    if (cells[x, y] != CellKind.MessTable || IsMessTableReserved(tableCell))
                    {
                        continue;
                    }

                    Job testJob = new Job(JobType.Eat, tableCell, EatWorkRequired(worker))
                    {
                        TargetWorkerName = worker.Name
                    };

                    if (TryFindPathToJob(worker.Cell, testJob, out List<Vector2Int> path) && path.Count < bestDistance)
                    {
                        bestDistance = path.Count;
                        bestCell = tableCell;
                    }
                }
            }

            if (bestCell.x < 0)
            {
                return false;
            }

            Job job = new Job(JobType.Eat, bestCell, EatWorkRequired(worker))
            {
                AutoGenerated = true,
                Priority = EatPriority(worker),
                TargetWorkerName = worker.Name
            };
            jobs.Add(job);
            overlayDirty = true;
            return true;
        }

        private bool TryAssignRelaxJob(Worker worker)
        {
            Job bestJob = null;
            List<Vector2Int> bestPath = null;
            int bestDistance = int.MaxValue;

            foreach (Job job in jobs)
            {
                if (job.Type != JobType.Relax || !CanWorkerTakeJob(worker, job))
                {
                    continue;
                }

                if (TryFindPathToJob(worker.Cell, job, out List<Vector2Int> path) && path.Count < bestDistance)
                {
                    bestDistance = path.Count;
                    bestPath = path;
                    bestJob = job;
                }
            }

            if (bestJob == null)
            {
                return false;
            }

            bestJob.Priority = RelaxPriority(worker);
            AssignJob(worker, bestJob, bestPath);
            return true;
        }

        private bool TryCreateRelaxJob(Worker worker)
        {
            Vector2Int bestCell = new Vector2Int(-1, -1);
            int bestDistance = int.MaxValue;

            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    Vector2Int tableCell = new Vector2Int(x, y);
                    if (cells[x, y] != CellKind.MassageTable || IsMassageTableReserved(tableCell))
                    {
                        continue;
                    }

                    Job testJob = new Job(JobType.Relax, tableCell, RelaxWorkRequired(worker))
                    {
                        TargetWorkerName = worker.Name
                    };

                    if (TryFindPathToJob(worker.Cell, testJob, out List<Vector2Int> path) && path.Count < bestDistance)
                    {
                        bestDistance = path.Count;
                        bestCell = tableCell;
                    }
                }
            }

            if (bestCell.x < 0)
            {
                return false;
            }

            Job job = new Job(JobType.Relax, bestCell, RelaxWorkRequired(worker))
            {
                AutoGenerated = true,
                Priority = RelaxPriority(worker),
                TargetWorkerName = worker.Name
            };
            jobs.Add(job);
            overlayDirty = true;
            return true;
        }

        private bool TryAssignSleepJob(Worker worker)
        {
            Job bestJob = null;
            List<Vector2Int> bestPath = null;
            int bestDistance = int.MaxValue;

            foreach (Job job in jobs)
            {
                if (job.Type != JobType.Sleep || job.AssignedWorker != null || !IsJobValid(job))
                {
                    continue;
                }

                if (TryFindPathToJob(worker.Cell, job, out List<Vector2Int> path) && path.Count < bestDistance)
                {
                    bestDistance = path.Count;
                    bestPath = path;
                    bestJob = job;
                }
            }

            if (bestJob == null)
            {
                for (int y = 0; y < WorldHeight; y++)
                {
                    for (int x = 0; x < WorldWidth; x++)
                    {
                        Vector2Int bedCell = new Vector2Int(x, y);
                        if (cells[x, y] != CellKind.Bed || IsBedReserved(bedCell))
                        {
                            continue;
                        }

                        Job sleepJob = new Job(JobType.Sleep, bedCell, SleepWorkRequired(worker))
                        {
                            AutoGenerated = true,
                            Priority = SleepPriority(worker)
                        };

                        if (TryFindPathToJob(worker.Cell, sleepJob, out List<Vector2Int> path) && path.Count < bestDistance)
                        {
                            bestDistance = path.Count;
                            bestPath = path;
                            bestJob = sleepJob;
                        }
                    }
                }

                if (bestJob != null)
                {
                    jobs.Add(bestJob);
                }
            }

            if (bestJob == null)
            {
                return false;
            }

            bestJob.Priority = SleepPriority(worker);
            AssignJob(worker, bestJob, bestPath);
            return true;
        }

        private void MoveOrWork(Worker worker, float deltaTime)
        {
            if (worker.PathIndex < worker.Path.Count)
            {
                Vector2Int targetCell = worker.Path[worker.PathIndex];
                if (!CanWorkerTraversePathStep(worker, worker.Cell, targetCell))
                {
                    if (IsSuitCheckpointCell(worker.Cell) && NeedsSuitProtection(targetCell))
                    {
                        DenySuitCheckpointEntry(worker, worker.Cell);
                    }
                    else
                    {
                        ClearAssignment(worker);
                    }

                    return;
                }

                Vector3 target = CellCenter(targetCell);
                worker.Transform.position = Vector3.MoveTowards(worker.Transform.position, target, worker.MoveSpeed * deltaTime);
                worker.Renderer.flipX = target.x < worker.Transform.position.x;
                worker.Activity = "Moving";

                if ((worker.Transform.position - target).sqrMagnitude < 0.0025f)
                {
                    Vector2Int previousCell = worker.Cell;
                    worker.Cell = targetCell;
                    worker.PathIndex++;
                    if (!UpdateSuitCheckpointCrossing(worker, previousCell, worker.Cell))
                    {
                        worker.Cell = previousCell;
                        worker.Transform.position = CellCenter(previousCell);
                        DenySuitCheckpointEntry(worker, previousCell);
                    }
                }

                return;
            }

            Job job = worker.AssignedJob;
            worker.Activity = JobLabel(job);
            if (job.Type == JobType.Sleep)
            {
                worker.Activity = "Sleeping";
                bool inBarracks = RoomKindAt(job.Cell.x, job.Cell.y) == RoomKind.Barracks;
                worker.Fatigue = Mathf.Max(0f, worker.Fatigue - (inBarracks ? 7.6f : 6.5f) * deltaTime);
                worker.Stress = Mathf.Max(0f, worker.Stress - (inBarracks ? 2.2f : 1.4f) * deltaTime);
                worker.Morale = Mathf.Min(10f, worker.Morale + (inBarracks ? 0.16f : 0.08f) * deltaTime);
                if (worker.Calories > 400f)
                {
                    worker.Health = Mathf.Min(100f, worker.Health + 0.7f * deltaTime);
                }

                job.Progress += deltaTime;
                if (job.Progress >= job.WorkRequired || (!IsRestTime() && worker.Fatigue < 28f) || worker.Fatigue < 8f)
                {
                    CompleteJob(job, worker);
                }

                return;
            }

            if (job.Type == JobType.Treat)
            {
                worker.Activity = "Receiving Treatment";
                float clinicFactor = RoomKindAt(job.Cell.x, job.Cell.y) == RoomKind.Clinic ? 1.25f : 1f;
                float careFactor = (water >= 1f ? 1f : 0.65f) * clinicFactor;
                worker.GermExposure = Mathf.Max(0f, worker.GermExposure - 4.5f * careFactor * deltaTime);
                worker.Sickness = Mathf.Max(0f, worker.Sickness - 2.8f * careFactor * deltaTime);
                worker.Stress = Mathf.Max(0f, worker.Stress - 1.8f * deltaTime);
                if (worker.Calories > 350f)
                {
                    worker.Health = Mathf.Min(100f, worker.Health + 1.4f * careFactor * deltaTime);
                }

                job.Progress += deltaTime * careFactor;
                if (job.Progress >= job.WorkRequired || !NeedsTreatment(worker))
                {
                    CompleteJob(job, worker);
                }

                return;
            }

            if (job.Type == JobType.UseToilet)
            {
                worker.Activity = "Using Outhouse";
                worker.Bladder = Mathf.Max(0f, worker.Bladder - 38f * deltaTime);
                worker.Stress = Mathf.Max(0f, worker.Stress - 0.8f * deltaTime);
                job.Progress += deltaTime;
                if (job.Progress >= job.WorkRequired || worker.Bladder <= 5f)
                {
                    CompleteJob(job, worker);
                }

                return;
            }

            if (job.Type == JobType.WashHands)
            {
                worker.Activity = "Washing Hands";
                float washFactor = water > 0.05f ? 1f : 0.35f;
                worker.GermExposure = Mathf.Max(0f, worker.GermExposure - 6.5f * washFactor * deltaTime);
                job.Progress += deltaTime * washFactor;
                if (job.Progress >= job.WorkRequired || worker.GermExposure <= 2f)
                {
                    CompleteJob(job, worker);
                }

                return;
            }

            if (job.Type == JobType.Eat)
            {
                worker.Activity = "Eating";
                float mealRoomFactor = RoomKindAt(job.Cell.x, job.Cell.y) == RoomKind.MessHall ? 1.45f : 1f;
                worker.Stress = Mathf.Max(0f, worker.Stress - 1.4f * mealRoomFactor * deltaTime);
                job.Progress += deltaTime;
                if (job.Progress >= job.WorkRequired || worker.Calories >= 2100f)
                {
                    CompleteJob(job, worker);
                }

                return;
            }

            if (job.Type == JobType.Relax)
            {
                worker.Activity = "Relaxing";
                float recreationFactor = RoomKindAt(job.Cell.x, job.Cell.y) == RoomKind.RecreationRoom ? 1.25f : 1f;
                worker.Stress = Mathf.Max(0f, worker.Stress - 7.5f * recreationFactor * deltaTime);
                worker.Morale = Mathf.Min(10f, worker.Morale + 0.42f * recreationFactor * deltaTime);
                worker.Fatigue = Mathf.Min(100f, worker.Fatigue + 0.08f * deltaTime);
                worker.Calories = Mathf.Max(0f, worker.Calories - 0.04f * deltaTime);
                job.Progress += deltaTime;
                if (job.Progress >= job.WorkRequired || worker.Stress <= 32f)
                {
                    CompleteJob(job, worker);
                }

                return;
            }

            float oxygenFactor = IsInside(worker.Cell.x, worker.Cell.y) && oxygen[worker.Cell.x, worker.Cell.y] < 0.12f ? 0.45f : 1f;
            job.Progress += deltaTime * worker.WorkSpeed * oxygenFactor;

            if (job.Type == JobType.OperateGenerator)
            {
                worker.Calories -= 0.65f * deltaTime;
            }
            else if (job.Type == JobType.Research)
            {
                worker.Calories -= 0.18f * deltaTime;
            }
            else if (job.Type == JobType.PumpWater)
            {
                worker.Calories -= 0.28f * deltaTime;
            }
            else if (job.Type == JobType.EmptyBottle)
            {
                worker.Calories -= 0.16f * deltaTime;
            }
            else if (job.Type == JobType.GroomHatch)
            {
                worker.Calories -= 0.12f * deltaTime;
                worker.Stress = Mathf.Max(0f, worker.Stress - 0.35f * deltaTime);
            }

            if (job.Progress >= job.WorkRequired)
            {
                CompleteJob(job, worker);
            }
        }

        private void CompleteJob(Job job, Worker worker)
        {
            switch (job.Type)
            {
                case JobType.Dig:
                    CompleteDig(job);
                    break;
                case JobType.Build:
                    CompleteBuild(job);
                    break;
                case JobType.BuildWire:
                    CompletePowerWire(job);
                    break;
                case JobType.BuildAutomationWire:
                    CompleteAutomationWire(job);
                    break;
                case JobType.BuildPipe:
                    CompleteLiquidPipe(job);
                    break;
                case JobType.BuildGasPipe:
                    CompleteGasPipe(job);
                    break;
                case JobType.BuildShippingRail:
                    CompleteShippingRail(job);
                    break;
                case JobType.Deconstruct:
                    CompleteDeconstruct(job, worker);
                    break;
                case JobType.Mop:
                    CompleteMop(job, worker);
                    break;
                case JobType.Repair:
                    CompleteRepair(job, worker);
                    break;
                case JobType.Rescue:
                    CompleteRescue(job, worker);
                    break;
                case JobType.Sweep:
                    CompleteSweep(job, worker);
                    break;
                case JobType.OperateGenerator:
                    power = Mathf.Min(maxPower, power + 38f);
                    AddHeat(job.Cell, 4.5f, 1);
                    WearEquipment(job.Cell, 0.018f);
                    Log(worker.Name + " generated power.");
                    break;
                case JobType.Harvest:
                    AddFreshFood(650f, 0.78f);
                    plantGrowth[job.Cell.x, job.Cell.y] = 0f;
                    cropTendedSeconds[job.Cell.x, job.Cell.y] = 0f;
                    cropStress[job.Cell.x, job.Cell.y] = 0f;
                    Log(worker.Name + " harvested mealwood.");
                    break;
                case JobType.PumpWater:
                    CompletePumpWater(job, worker);
                    break;
                case JobType.EmptyBottle:
                    CompleteEmptyBottle(job, worker);
                    break;
                case JobType.TendCrop:
                    CompleteTendCrop(job, worker);
                    break;
                case JobType.Research:
                    bool hadAir = techAirSystems;
                    bool hadFood = techFoodPreparation;
                    bool hadPower = techPowerRegulation;
                    researchPoints += 6f;
                    power = Mathf.Max(0f, power - 4f);
                    AddHeat(job.Cell, 0.7f, 1);
                    WearEquipment(job.Cell, 0.014f);
                    ApplyResearchUnlocks(false);
                    if (!hadAir && techAirSystems)
                    {
                        Log("Research unlocked: Air Systems.");
                    }
                    else if (!hadFood && techFoodPreparation)
                    {
                        Log("Research unlocked: Food Preparation.");
                    }
                    else if (!hadPower && techPowerRegulation)
                    {
                        Log("Research unlocked: Power Regulation.");
                    }
                    else
                    {
                        Log(worker.Name + " completed research.");
                    }
                    break;
                case JobType.Cook:
                    water = Mathf.Max(0f, water - 4f);
                    dirt = Mathf.Max(0f, dirt - 1f);
                    power = Mathf.Max(0f, power - 2f);
                    AddFreshFood(900f, 0.96f);
                    AddHeat(job.Cell, 1.1f, 1);
                    WearEquipment(job.Cell, 0.014f);
                    Log(worker.Name + " cooked mush bars.");
                    break;
                case JobType.RefineMetal:
                    CompleteRefineMetal(job, worker);
                    break;
                case JobType.Sleep:
                    worker.Fatigue = Mathf.Min(worker.Fatigue, 18f);
                    worker.Stress = Mathf.Max(0f, worker.Stress - 8f);
                    Log(worker.Name + " finished resting.");
                    break;
                case JobType.Treat:
                    CompleteTreatment(job, worker);
                    break;
                case JobType.UseToilet:
                    CompleteToiletUse(job, worker);
                    break;
                case JobType.WashHands:
                    CompleteWashHands(job, worker);
                    break;
                case JobType.Eat:
                    CompleteEat(job, worker);
                    break;
                case JobType.Relax:
                    CompleteRelaxation(job, worker);
                    break;
                case JobType.Compost:
                    CompleteCompost(job, worker);
                    break;
                case JobType.GroomHatch:
                    CompleteGroomHatch(job, worker);
                    break;
            }

            GrantWorkerExperience(worker, job);
            jobs.Remove(job);
            ClearAssignment(worker);
            overlayDirty = true;
        }

        private void CompletePowerWire(Job job)
        {
            powerWire[job.Cell.x, job.Cell.y] = true;
            poweredWire[job.Cell.x, job.Cell.y] = false;
            wireLoad[job.Cell.x, job.Cell.y] = 0f;
            overloadedWire[job.Cell.x, job.Cell.y] = false;
            wireOverloadStress[job.Cell.x, job.Cell.y] = 0f;
            gasDirty = true;
            overlayDirty = true;
            Log("Built Power Wire.");
        }

        private void CompleteAutomationWire(Job job)
        {
            automationWire[job.Cell.x, job.Cell.y] = true;
            overlayDirty = true;
            Log("Built Automation Wire.");
        }

        private void CompleteLiquidPipe(Job job)
        {
            liquidPipe[job.Cell.x, job.Cell.y] = true;
            pipeWater[job.Cell.x, job.Cell.y] = Mathf.Clamp(pipeWater[job.Cell.x, job.Cell.y], 0f, LiquidPipeCapacity);
            overlayDirty = true;
            Log("Built Liquid Pipe.");
        }

        private void CompleteGasPipe(Job job)
        {
            gasPipe[job.Cell.x, job.Cell.y] = true;
            gasPipeOxygen[job.Cell.x, job.Cell.y] = Mathf.Clamp(gasPipeOxygen[job.Cell.x, job.Cell.y], 0f, GasPipeCapacity);
            gasPipeCarbonDioxide[job.Cell.x, job.Cell.y] = Mathf.Clamp(gasPipeCarbonDioxide[job.Cell.x, job.Cell.y], 0f, GasPipeCapacity);
            gasPipePollutedOxygen[job.Cell.x, job.Cell.y] = Mathf.Clamp(gasPipePollutedOxygen[job.Cell.x, job.Cell.y], 0f, GasPipeCapacity);
            gasPipeHydrogen[job.Cell.x, job.Cell.y] = Mathf.Clamp(gasPipeHydrogen[job.Cell.x, job.Cell.y], 0f, GasPipeCapacity);
            gasPipeChlorine[job.Cell.x, job.Cell.y] = Mathf.Clamp(gasPipeChlorine[job.Cell.x, job.Cell.y], 0f, GasPipeCapacity);
            gasPipeNaturalGas[job.Cell.x, job.Cell.y] = Mathf.Clamp(gasPipeNaturalGas[job.Cell.x, job.Cell.y], 0f, GasPipeCapacity);
            overlayDirty = true;
            Log("Built Gas Pipe.");
        }

        private void CompleteShippingRail(Job job)
        {
            shippingRail[job.Cell.x, job.Cell.y] = true;
            shippingRailKind[job.Cell.x, job.Cell.y] = shippingRailAmount[job.Cell.x, job.Cell.y] > 0.001f ? shippingRailKind[job.Cell.x, job.Cell.y] : LooseResourceKind.None;
            shippingRailAmount[job.Cell.x, job.Cell.y] = Mathf.Clamp(shippingRailAmount[job.Cell.x, job.Cell.y], 0f, ShippingRailCapacity);
            overlayDirty = true;
            Log("Built Shipping Rail.");
        }

        private void CompleteDeconstruct(Job job, Worker worker)
        {
            if (!IsDeconstructJobValid(job))
            {
                Log("Deconstruction target no longer exists.");
                return;
            }

            string label = DeconstructTargetLabel(job);
            if (job.BuildKind != CellKind.Empty)
            {
                RemoveBuiltCell(job.Cell, job.BuildKind);
            }
            else if (job.RemovePowerWire)
            {
                powerWire[job.Cell.x, job.Cell.y] = false;
                poweredWire[job.Cell.x, job.Cell.y] = false;
                wireLoad[job.Cell.x, job.Cell.y] = 0f;
                overloadedWire[job.Cell.x, job.Cell.y] = false;
                wireOverloadStress[job.Cell.x, job.Cell.y] = 0f;
                StoreDryResource(ref metal, 0.5f);
                gasDirty = true;
                overlayDirty = true;
            }
            else if (job.RemoveAutomationWire)
            {
                automationWire[job.Cell.x, job.Cell.y] = false;
                automationControlledWire[job.Cell.x, job.Cell.y] = false;
                automationSignalWire[job.Cell.x, job.Cell.y] = false;
                StoreDryResource(ref metal, 0.5f);
                overlayDirty = true;
            }
            else if (job.RemoveLiquidPipe)
            {
                float drained = pipeWater[job.Cell.x, job.Cell.y];
                liquidPipe[job.Cell.x, job.Cell.y] = false;
                pipeWater[job.Cell.x, job.Cell.y] = 0f;
                water += drained;
                StoreDryResource(ref metal, 0.5f);
                overlayDirty = true;
            }
            else if (job.RemoveGasPipe)
            {
                ReleaseGasPipeContents(job.Cell);
                gasPipe[job.Cell.x, job.Cell.y] = false;
                gasPipeOxygen[job.Cell.x, job.Cell.y] = 0f;
                gasPipeCarbonDioxide[job.Cell.x, job.Cell.y] = 0f;
                gasPipePollutedOxygen[job.Cell.x, job.Cell.y] = 0f;
                gasPipeHydrogen[job.Cell.x, job.Cell.y] = 0f;
                gasPipeChlorine[job.Cell.x, job.Cell.y] = 0f;
                gasPipeNaturalGas[job.Cell.x, job.Cell.y] = 0f;
                gasPipeGerms[job.Cell.x, job.Cell.y] = 0f;
                StoreDryResource(ref metal, 0.5f);
                gasDirty = true;
                overlayDirty = true;
            }
            else if (job.RemoveShippingRail)
            {
                ReleaseShippingRailContents(job.Cell);
                shippingRail[job.Cell.x, job.Cell.y] = false;
                shippingRailKind[job.Cell.x, job.Cell.y] = LooseResourceKind.None;
                shippingRailAmount[job.Cell.x, job.Cell.y] = 0f;
                StoreDryResource(ref metal, 0.5f);
                overlayDirty = true;
            }

            deconstructionsCompleted++;
            Log(worker.Name + " deconstructed " + label + ".");
        }

        private void RemoveBuiltCell(Vector2Int cell, CellKind removedKind)
        {
            float releasedReservoirOxygen = removedKind == CellKind.GasReservoir ? gasReservoirOxygen[cell.x, cell.y] : 0f;
            float releasedReservoirCarbon = removedKind == CellKind.GasReservoir ? gasReservoirCarbonDioxide[cell.x, cell.y] : 0f;
            float releasedReservoirPolluted = removedKind == CellKind.GasReservoir ? gasReservoirPollutedOxygen[cell.x, cell.y] : 0f;
            float releasedReservoirHydrogen = removedKind == CellKind.GasReservoir ? gasReservoirHydrogen[cell.x, cell.y] : 0f;
            float releasedReservoirChlorine = removedKind == CellKind.GasReservoir ? gasReservoirChlorine[cell.x, cell.y] : 0f;
            float releasedReservoirNaturalGas = removedKind == CellKind.GasReservoir ? gasReservoirNaturalGas[cell.x, cell.y] : 0f;
            float releasedReservoirGerms = removedKind == CellKind.GasReservoir ? gasReservoirGerms[cell.x, cell.y] : 0f;
            cells[cell.x, cell.y] = CellKind.Empty;
            if (removedKind == CellKind.Battery)
            {
                maxPower = Mathf.Max(100f, maxPower - (techPowerRegulation ? 100f : 60f));
                power = Mathf.Min(power, maxPower);
            }
            else if (removedKind == CellKind.SmartBattery)
            {
                maxPower = Mathf.Max(100f, maxPower - 180f);
                power = Mathf.Min(power, maxPower);
            }
            else if (removedKind == CellKind.Planter)
            {
                plantGrowth[cell.x, cell.y] = 0f;
                cropTendedSeconds[cell.x, cell.y] = 0f;
                cropStress[cell.x, cell.y] = 0f;
            }
            else if (removedKind == CellKind.AtmoSuitDock)
            {
                suitOxygen = Mathf.Min(suitOxygen, SuitOxygenCapacityTotal());
            }
            else if (removedKind == CellKind.PrintingPod && CountCells(CellKind.PrintingPod) == 0)
            {
                printingPodProgress = 0f;
            }
            else if (removedKind == CellKind.LiquidReservoir)
            {
                water += liquidReservoirWater[cell.x, cell.y];
            }

            waterMass[cell.x, cell.y] = 0f;
            liquidReservoirWater[cell.x, cell.y] = 0f;
            automationSwitchState[cell.x, cell.y] = false;
            airlockOpen[cell.x, cell.y] = false;
            gasReservoirOxygen[cell.x, cell.y] = 0f;
            gasReservoirCarbonDioxide[cell.x, cell.y] = 0f;
            gasReservoirPollutedOxygen[cell.x, cell.y] = 0f;
            gasReservoirHydrogen[cell.x, cell.y] = 0f;
            gasReservoirChlorine[cell.x, cell.y] = 0f;
            gasReservoirNaturalGas[cell.x, cell.y] = 0f;
            gasReservoirGerms[cell.x, cell.y] = 0f;
            equipmentCondition[cell.x, cell.y] = 0f;
            oxygen[cell.x, cell.y] = NeighborAverage(oxygen, cell.x, cell.y, 0.18f);
            carbonDioxide[cell.x, cell.y] = NeighborAverage(carbonDioxide, cell.x, cell.y, 0.04f);
            pollutedOxygen[cell.x, cell.y] = NeighborAverage(pollutedOxygen, cell.x, cell.y, 0f);
            hydrogen[cell.x, cell.y] = NeighborAverage(hydrogen, cell.x, cell.y, 0f);
            chlorine[cell.x, cell.y] = NeighborAverage(chlorine, cell.x, cell.y, 0f);
            naturalGas[cell.x, cell.y] = NeighborAverage(naturalGas, cell.x, cell.y, 0f);
            germs[cell.x, cell.y] = Mathf.Clamp01(NeighborAverage(germs, cell.x, cell.y, 0f));
            if (releasedReservoirOxygen + releasedReservoirCarbon + releasedReservoirPolluted + releasedReservoirHydrogen + releasedReservoirChlorine + releasedReservoirNaturalGas > 0.001f)
            {
                AddGasToTile(cell.x, cell.y, releasedReservoirOxygen, releasedReservoirCarbon, releasedReservoirPolluted, releasedReservoirHydrogen, releasedReservoirChlorine, releasedReservoirNaturalGas, releasedReservoirGerms);
            }

            RefundDeconstructedBuildCost(removedKind);
            terrainDirty = true;
            gasDirty = true;
            InvalidateRooms();
        }

        private void RefundDeconstructedBuildCost(CellKind kind)
        {
            BuildSpec spec = BuildSpecForKind(kind);
            StoreDryResource(ref dirt, spec.Dirt * 0.5f);
            StoreDryResource(ref metal, spec.Metal * 0.5f);
            StoreDryResource(ref algae, spec.Algae * 0.5f);
            StoreDryResource(ref refinedMetal, spec.RefinedMetal * 0.5f);
        }

        private void CompleteDig(Job job)
        {
            int x = job.Cell.x;
            int y = job.Cell.y;
            CellKind mined = cells[x, y];
            LooseResourceKind resourceKind = LooseResourceKind.Dirt;
            float minedAmount = 0f;
            bool createsLooseResource = true;
            switch (mined)
            {
                case CellKind.Algae:
                    resourceKind = LooseResourceKind.Algae;
                    minedAmount = 5f;
                    break;
                case CellKind.Slime:
                    resourceKind = LooseResourceKind.Algae;
                    minedAmount = 3f;
                    break;
                case CellKind.Coal:
                    resourceKind = LooseResourceKind.Coal;
                    minedAmount = 9f;
                    break;
                case CellKind.Ice:
                    water += 12f;
                    createsLooseResource = false;
                    break;
                case CellKind.MetalOre:
                    resourceKind = LooseResourceKind.Metal;
                    minedAmount = 7f;
                    break;
                case CellKind.Sand:
                case CellKind.Regolith:
                    resourceKind = LooseResourceKind.Dirt;
                    minedAmount = 3f;
                    break;
                case CellKind.Rock:
                    resourceKind = LooseResourceKind.Dirt;
                    minedAmount = 1f;
                    break;
                default:
                    resourceKind = LooseResourceKind.Dirt;
                    minedAmount = 4f;
                    break;
            }

            cells[x, y] = CellKind.Empty;
            equipmentCondition[x, y] = 0f;
            oxygen[x, y] = NeighborAverage(oxygen, x, y, 0.18f);
            carbonDioxide[x, y] = NeighborAverage(carbonDioxide, x, y, 0.08f);
            hydrogen[x, y] = NeighborAverage(hydrogen, x, y, 0f);
            chlorine[x, y] = NeighborAverage(chlorine, x, y, 0f);
            naturalGas[x, y] = NeighborAverage(naturalGas, x, y, 0f);
            if (mined == CellKind.Slime)
            {
                pollutedOxygen[x, y] = Mathf.Max(0.8f, NeighborAverage(pollutedOxygen, x, y, 0.35f));
                germs[x, y] = 1f;
            }
            terrainDirty = true;
            gasDirty = true;
            InvalidateRooms();
            if (createsLooseResource)
            {
                float dropped = AddLooseResource(new Vector2Int(x, y), resourceKind, minedAmount);
                LogMiningResult(mined, LooseResourceLabel(resourceKind), minedAmount, dropped);
            }
            else
            {
                Log("Mined ice. Stored 12 water.");
            }
        }

        private float AddLooseResource(Vector2Int origin, LooseResourceKind kind, float amount)
        {
            if (kind == LooseResourceKind.None || amount <= 0f)
            {
                return 0f;
            }

            if (TryFindLooseResourceDropCell(origin, kind, out Vector2Int dropCell))
            {
                looseResourceKind[dropCell.x, dropCell.y] = kind;
                looseResourceAmount[dropCell.x, dropCell.y] = Mathf.Min(80f, looseResourceAmount[dropCell.x, dropCell.y] + amount);
                terrainDirty = true;
                overlayDirty = true;
                return amount;
            }

            return StoreLooseResource(kind, amount);
        }

        private bool TryFindLooseResourceDropCell(Vector2Int origin, LooseResourceKind kind, out Vector2Int dropCell)
        {
            if (CanDropLooseResource(origin, kind))
            {
                dropCell = origin;
                return true;
            }

            Vector2Int[] offsets =
            {
                new Vector2Int(1, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, -1),
                new Vector2Int(1, 1),
                new Vector2Int(-1, 1),
                new Vector2Int(1, -1),
                new Vector2Int(-1, -1)
            };

            foreach (Vector2Int offset in offsets)
            {
                Vector2Int candidate = origin + offset;
                if (CanDropLooseResource(candidate, kind))
                {
                    dropCell = candidate;
                    return true;
                }
            }

            dropCell = origin;
            return false;
        }

        private bool CanDropLooseResource(Vector2Int cell, LooseResourceKind kind)
        {
            return IsInside(cell.x, cell.y) &&
                IsPassable(cell.x, cell.y) &&
                (looseResourceKind[cell.x, cell.y] == LooseResourceKind.None || looseResourceKind[cell.x, cell.y] == kind) &&
                looseResourceAmount[cell.x, cell.y] < 72f;
        }

        private float StoreDryResource(ref float resource, float amount)
        {
            if (amount <= 0f)
            {
                return 0f;
            }

            float stored = Mathf.Min(amount, DryResourceFreeSpace());
            resource += stored;
            return stored;
        }

        private float StoreLooseResource(LooseResourceKind kind, float amount)
        {
            switch (kind)
            {
                case LooseResourceKind.Dirt:
                    return StoreDryResource(ref dirt, amount);
                case LooseResourceKind.Metal:
                    return StoreDryResource(ref metal, amount);
                case LooseResourceKind.Algae:
                    return StoreDryResource(ref algae, amount);
                case LooseResourceKind.Coal:
                    return StoreDryResource(ref coal, amount);
                case LooseResourceKind.RefinedMetal:
                    return StoreDryResource(ref refinedMetal, amount);
                case LooseResourceKind.PollutedDirt:
                    return StoreDryResource(ref pollutedDirt, amount);
                default:
                    return 0f;
            }
        }

        private void CompleteSweep(Job job, Worker worker)
        {
            Vector2Int cell = job.Cell;
            if (!HasLooseResource(cell))
            {
                Log(worker.Name + " found no debris to sweep.");
                return;
            }

            LooseResourceKind kind = looseResourceKind[cell.x, cell.y];
            float amount = looseResourceAmount[cell.x, cell.y];
            float stored = StoreLooseResource(kind, amount);
            looseResourceAmount[cell.x, cell.y] = Mathf.Max(0f, amount - stored);
            if (looseResourceAmount[cell.x, cell.y] <= 0.05f)
            {
                looseResourceAmount[cell.x, cell.y] = 0f;
                looseResourceKind[cell.x, cell.y] = LooseResourceKind.None;
            }

            sweptResources += stored;
            milestoneResourceLogistics |= sweptResources >= 12f;
            terrainDirty = true;
            overlayDirty = true;
            Log(worker.Name + " swept " + stored.ToString("0.#") + " kg " + LooseResourceLabel(kind) + ".");
        }

        private void CompleteTendCrop(Job job, Worker worker)
        {
            Vector2Int crop = job.Cell;
            if (!IsCropTendingTarget(crop) || !TryFindFarmStationForCrop(crop, out Vector2Int station))
            {
                Log(worker.Name + " found no crop to tend.");
                return;
            }

            bool usedPollutedDirt = pollutedDirt >= CropTendPollutedDirtCost;
            if (usedPollutedDirt)
            {
                pollutedDirt = Mathf.Max(0f, pollutedDirt - CropTendPollutedDirtCost);
            }
            else if (dirt >= CropTendDirtFallbackCost)
            {
                dirt = Mathf.Max(0f, dirt - CropTendDirtFallbackCost);
            }
            else
            {
                Log("Farm Station needs polluted dirt or dirt for crop tending.");
                return;
            }

            cropTendedSeconds[crop.x, crop.y] = CropTendedSeconds;
            cropsTended++;
            WearEquipment(station, 0.004f);
            terrainDirty = true;
            overlayDirty = true;
            Log(worker.Name + " tended mealwood with " + (usedPollutedDirt ? "polluted dirt." : "dirt."));
        }

        private bool EatStoredFood(Worker worker, bool seated)
        {
            if (food < 700f)
            {
                return false;
            }

            food -= 700f;
            worker.Calories = Mathf.Min(3200f, worker.Calories + (seated ? 1450f : 1200f));
            if (seated)
            {
                worker.Stress = Mathf.Max(0f, worker.Stress - DiningStressRelief(worker.Cell));
                worker.Morale = Mathf.Min(10f, worker.Morale + (RoomKindAt(worker.Cell.x, worker.Cell.y) == RoomKind.MessHall ? 1.1f : 0.7f));
                mealsEatenAtTable++;
                Log(worker.Name + " ate at a Mess Table.");
            }
            else
            {
                worker.Stress = Mathf.Min(100f, worker.Stress + 1.5f);
                worker.Morale = Mathf.Max(0f, worker.Morale - 0.25f);
                Log(worker.Name + " ate a ration.");
            }

            ApplyFoodQualityEffects(worker, seated);
            return true;
        }

        private void ApplyFoodQualityEffects(Worker worker, bool seated)
        {
            if (worker == null || foodFreshness >= StaleFoodFreshnessThreshold)
            {
                return;
            }

            staleMealsEaten++;
            float staleSeverity = Mathf.Clamp01((StaleFoodFreshnessThreshold - foodFreshness) / StaleFoodFreshnessThreshold);
            worker.Stress = Mathf.Min(100f, worker.Stress + (seated ? 1.2f : 2.1f) * staleSeverity);
            worker.Morale = Mathf.Max(0f, worker.Morale - 0.35f * staleSeverity);
            worker.GermExposure = Mathf.Min(100f, worker.GermExposure + 8f * staleSeverity);

            if (foodFreshness < FoodPoisoningFreshnessThreshold)
            {
                float poisonSeverity = Mathf.Clamp01((FoodPoisoningFreshnessThreshold - foodFreshness) / FoodPoisoningFreshnessThreshold);
                worker.Sickness = Mathf.Min(100f, worker.Sickness + 18f * poisonSeverity);
                worker.Health = Mathf.Max(0f, worker.Health - 5f * poisonSeverity);
                worker.Stress = Mathf.Min(100f, worker.Stress + 4f * poisonSeverity);
                foodPoisoningCases++;
                if (IsInside(worker.Cell.x, worker.Cell.y))
                {
                    germs[worker.Cell.x, worker.Cell.y] = Mathf.Clamp01(germs[worker.Cell.x, worker.Cell.y] + 0.12f * poisonSeverity);
                    pollutedOxygen[worker.Cell.x, worker.Cell.y] = Mathf.Min(2.5f, pollutedOxygen[worker.Cell.x, worker.Cell.y] + 0.04f * poisonSeverity);
                    gasDirty = true;
                    overlayDirty = true;
                }

                Log(worker.Name + " got sick from spoiled food. Refrigerate or compost bad meals.");
            }
            else
            {
                Log(worker.Name + " ate stale food. Fresh storage is needed.");
            }
        }

        private void CompleteRefineMetal(Job job, Worker worker)
        {
            float oreUsed = Mathf.Min(metal, RockCrusherOrePerJob);
            if (oreUsed <= 0.5f || power < RockCrusherPowerCost || !CanPoweredMachineRun(job.Cell))
            {
                Log("Rock Crusher needs metal ore and power.");
                return;
            }

            metal = Mathf.Max(0f, metal - oreUsed);
            power = Mathf.Max(0f, power - RockCrusherPowerCost);
            float produced = RockCrusherRefinedMetalYield * (oreUsed / RockCrusherOrePerJob);
            float stored = StoreDryResource(ref refinedMetal, produced);
            refinedMetalProduced += stored;
            AddHeat(job.Cell, 1.8f, 1);
            WearEquipment(job.Cell, 0.020f);
            Log(worker.Name + " refined " + stored.ToString("0.0") + " kg metal.");
        }

        private void GrantWorkerExperience(Worker worker, Job job)
        {
            if (worker == null || job == null || worker.Health <= 0f)
            {
                return;
            }

            float amount = ExperienceForJob(job.Type);
            if (amount <= 0f)
            {
                return;
            }

            int oldLevel = WorkerSkillLevel(worker);
            worker.Experience = Mathf.Min(MaxWorkerExperience(), Mathf.Max(0f, worker.Experience) + amount);
            int newLevel = WorkerSkillLevel(worker);
            if (newLevel > oldLevel)
            {
                worker.Stress = Mathf.Max(0f, worker.Stress - 2f);
                Log(worker.Name + " reached Skill Lv " + newLevel + ". Work speed improved; morale need is " + WorkerMoraleNeed(worker).ToString("0.0") + ".");
            }
        }

        private float ExperienceForJob(JobType type)
        {
            switch (type)
            {
                case JobType.Research:
                    return 28f;
                case JobType.Treat:
                    return 24f;
                case JobType.WashHands:
                    return 10f;
                case JobType.Cook:
                    return 20f;
                case JobType.RefineMetal:
                    return 18f;
                case JobType.Build:
                    return 18f;
                case JobType.Dig:
                    return 16f;
                case JobType.OperateGenerator:
                case JobType.PumpWater:
                case JobType.EmptyBottle:
                    return 14f;
                case JobType.TendCrop:
                    return 14f;
                case JobType.BuildWire:
                case JobType.BuildAutomationWire:
                case JobType.BuildPipe:
                case JobType.BuildGasPipe:
                case JobType.BuildShippingRail:
                case JobType.Deconstruct:
                case JobType.Mop:
                case JobType.Repair:
                case JobType.Rescue:
                case JobType.Sweep:
                case JobType.Harvest:
                    return 12f;
                case JobType.Compost:
                    return 14f;
                case JobType.GroomHatch:
                    return 16f;
                default:
                    return 0f;
            }
        }

        private int WorkerSkillLevel(Worker worker)
        {
            if (worker == null)
            {
                return 1;
            }

            return Mathf.Clamp(1 + Mathf.FloorToInt(Mathf.Max(0f, worker.Experience) / SkillExperiencePerLevel), 1, MaxWorkerSkillLevel);
        }

        private float WorkerSkillSpeedMultiplier(Worker worker)
        {
            return 0.94f + WorkerSkillLevel(worker) * 0.06f;
        }

        private float MaxWorkerExperience()
        {
            return SkillExperiencePerLevel * MaxWorkerSkillLevel;
        }

        private string WorkerSkillText(Worker worker)
        {
            int level = WorkerSkillLevel(worker);
            string text = "Skill Lv " + level + "  Work x" + WorkerSkillSpeedMultiplier(worker).ToString("0.00");
            if (level >= MaxWorkerSkillLevel)
            {
                return text + "  XP max";
            }

            float nextExperience = SkillExperiencePerLevel * level;
            return text + "  XP " + Mathf.FloorToInt(Mathf.Max(0f, worker.Experience)) + "/" + Mathf.FloorToInt(nextExperience);
        }

        private void UpdateWorkerMorale(Worker worker, float deltaTime)
        {
            if (worker == null || worker.Health <= 0f)
            {
                return;
            }

            float target = WorkerMoraleTarget(worker);
            float current = Mathf.Clamp(worker.Morale, 0f, 10f);
            float adjustRate = MoraleAdjustRate * (target < current ? 0.65f : 1f);
            worker.Morale = Mathf.MoveTowards(current, target, adjustRate * deltaTime);

            float deficit = WorkerMoraleDeficit(worker);
            if (deficit > 0.05f)
            {
                float beforeStress = worker.Stress;
                worker.Stress = Mathf.Min(100f, worker.Stress + deficit * MoraleDeficitStressRate * deltaTime);
                moralePressureSeconds += deltaTime;
                moraleStressAdded += Mathf.Max(0f, worker.Stress - beforeStress);
                return;
            }

            float surplus = Mathf.Clamp(worker.Morale - WorkerMoraleNeed(worker), 0f, 6f);
            if (surplus > 0.05f)
            {
                worker.Stress = Mathf.Max(0f, worker.Stress - surplus * MoraleSurplusStressReliefRate * deltaTime);
            }
        }

        private float WorkerMoraleNeed(Worker worker)
        {
            int skillLevel = WorkerSkillLevel(worker);
            return BaseWorkerMorale + Mathf.Max(0, skillLevel - 1) * MoraleNeedPerSkillLevel;
        }

        private float WorkerMoraleTarget(Worker worker)
        {
            if (worker == null || !IsInside(worker.Cell.x, worker.Cell.y))
            {
                return BaseWorkerMorale;
            }

            float morale = BaseWorkerMorale;
            switch (RoomKindAt(worker.Cell.x, worker.Cell.y))
            {
                case RoomKind.Barracks:
                    morale += worker.Activity == "Sleeping" ? 1.45f : 0.65f;
                    break;
                case RoomKind.MessHall:
                    morale += 1.8f;
                    break;
                case RoomKind.Washroom:
                    morale += 0.7f;
                    break;
                case RoomKind.Clinic:
                    morale += worker.Sickness > 0f || worker.Health < 80f ? 0.8f : 0.35f;
                    break;
                case RoomKind.RecreationRoom:
                    morale += worker.Activity == "Relaxing" ? 2.2f : 1.25f;
                    break;
                case RoomKind.BasicRoom:
                    morale += 0.35f;
                    break;
            }

            morale += DecorScoreAt(worker.Cell.x, worker.Cell.y) * 3f;
            if (worker.Calories > 2200f)
            {
                morale += 0.7f;
            }
            else if (worker.Calories > 1400f)
            {
                morale += 0.35f;
            }

            if (CountCells(CellKind.MessTable) >= workers.Count && food > 700f)
            {
                morale += foodFreshness >= 0.65f ? 0.65f : 0.35f;
            }

            if (worker.Fatigue < 45f)
            {
                morale += 0.4f;
            }

            if (worker.GermExposure > 35f || worker.Sickness > 20f)
            {
                morale -= 0.55f;
            }

            if (worker.StressBreakSeconds > 0f)
            {
                morale -= 1f;
            }

            return Mathf.Clamp(morale, 0f, 10f);
        }

        private float WorkerMoraleDeficit(Worker worker)
        {
            if (worker == null || worker.Health <= 0f)
            {
                return 0f;
            }

            return Mathf.Max(0f, WorkerMoraleNeed(worker) - worker.Morale);
        }

        private float AverageWorkerMorale()
        {
            float total = 0f;
            int count = 0;
            foreach (Worker worker in workers)
            {
                if (worker.Health <= 0f)
                {
                    continue;
                }

                total += Mathf.Clamp(worker.Morale, 0f, 10f);
                count++;
            }

            return count == 0 ? 0f : total / count;
        }

        private float AverageWorkerMoraleNeed()
        {
            float total = 0f;
            int count = 0;
            foreach (Worker worker in workers)
            {
                if (worker.Health <= 0f)
                {
                    continue;
                }

                total += WorkerMoraleNeed(worker);
                count++;
            }

            return count == 0 ? BaseWorkerMorale : total / count;
        }

        private int CountLowMoraleWorkers()
        {
            int count = 0;
            foreach (Worker worker in workers)
            {
                if (WorkerMoraleDeficit(worker) > 0.5f)
                {
                    count++;
                }
            }

            return count;
        }

        private float MaxWorkerMoraleDeficit()
        {
            float maxDeficit = 0f;
            foreach (Worker worker in workers)
            {
                maxDeficit = Mathf.Max(maxDeficit, WorkerMoraleDeficit(worker));
            }

            return maxDeficit;
        }

        private string WorkerMoraleText(Worker worker)
        {
            float need = WorkerMoraleNeed(worker);
            float morale = Mathf.Clamp(worker == null ? BaseWorkerMorale : worker.Morale, 0f, 10f);
            float deficit = Mathf.Max(0f, need - morale);
            string text = "Morale " + morale.ToString("0.0") + "/" + need.ToString("0.0");
            if (deficit > 0.05f)
            {
                text += "  Deficit " + deficit.ToString("0.0");
            }
            else
            {
                text += "  Stable";
            }

            return text;
        }

        private float DecorScoreAt(int x, int y)
        {
            if (!IsInside(x, y))
            {
                return 0f;
            }

            float score = 0f;
            for (int dy = -DecorPlantRadius; dy <= DecorPlantRadius; dy++)
            {
                for (int dx = -DecorPlantRadius; dx <= DecorPlantRadius; dx++)
                {
                    int nx = x + dx;
                    int ny = y + dy;
                    int distance = Mathf.Abs(dx) + Mathf.Abs(dy);
                    if (distance > DecorPlantRadius || !IsInside(nx, ny))
                    {
                        continue;
                    }

                    if (cells[nx, ny] == CellKind.DecorPlant)
                    {
                        score += Mathf.Lerp(0.12f, 1f, 1f - distance / (float)(DecorPlantRadius + 1));
                    }
                    else if (cells[nx, ny] == CellKind.MessTable || cells[nx, ny] == CellKind.MassageTable)
                    {
                        score += Mathf.Lerp(0.03f, 0.18f, 1f - distance / (float)(DecorPlantRadius + 1));
                    }
                }
            }

            return Mathf.Clamp01(score);
        }

        private float DecorStressReliefRate(float decorScore)
        {
            return Mathf.Lerp(0.25f, 1.25f, Mathf.Clamp01(decorScore));
        }

        private float AverageWorkerDecor()
        {
            float total = 0f;
            int count = 0;
            foreach (Worker worker in workers)
            {
                if (worker.Health <= 0f)
                {
                    continue;
                }

                total += DecorScoreAt(worker.Cell.x, worker.Cell.y);
                count++;
            }

            return count == 0 ? 0f : total / count;
        }

        private void EnsureRooms()
        {
            if (!roomsDirty)
            {
                return;
            }

            rooms.Clear();
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    roomIds[x, y] = -1;
                }
            }

            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (roomIds[x, y] >= 0 || !IsRoomInteriorCell(x, y))
                    {
                        continue;
                    }

                    RoomInfo room = new RoomInfo
                    {
                        Id = rooms.Count,
                        Enclosed = true
                    };
                    rooms.Add(room);
                    roomIds[x, y] = room.Id;
                    queue.Enqueue(new Vector2Int(x, y));

                    while (queue.Count > 0)
                    {
                        Vector2Int cell = queue.Dequeue();
                        AccumulateRoomCell(room, cell.x, cell.y);

                        AddRoomNeighbor(queue, room, cell.x + 1, cell.y);
                        AddRoomNeighbor(queue, room, cell.x - 1, cell.y);
                        AddRoomNeighbor(queue, room, cell.x, cell.y + 1);
                        AddRoomNeighbor(queue, room, cell.x, cell.y - 1);
                    }

                    if (room.Tiles > 0)
                    {
                        room.AverageOxygen /= room.Tiles;
                        room.AverageTemperature /= room.Tiles;
                    }

                    room.Kind = ClassifyRoom(room);
                }
            }

            roomsDirty = false;
        }

        private void AddRoomNeighbor(Queue<Vector2Int> queue, RoomInfo room, int x, int y)
        {
            if (!IsInside(x, y))
            {
                room.Enclosed = false;
                return;
            }

            if (!IsRoomInteriorCell(x, y) || roomIds[x, y] >= 0)
            {
                return;
            }

            roomIds[x, y] = room.Id;
            queue.Enqueue(new Vector2Int(x, y));
        }

        private void AccumulateRoomCell(RoomInfo room, int x, int y)
        {
            room.Tiles++;
            room.AverageOxygen += oxygen[x, y];
            room.AverageTemperature += temperature[x, y];

            switch (cells[x, y])
            {
                case CellKind.Bed:
                    room.Beds++;
                    break;
                case CellKind.MessTable:
                    room.MessTables++;
                    break;
                case CellKind.Outhouse:
                    room.Outhouses++;
                    break;
                case CellKind.WashBasin:
                    room.WashBasins++;
                    break;
                case CellKind.MedicalCot:
                    room.MedicalCots++;
                    break;
                case CellKind.MassageTable:
                    room.MassageTables++;
                    break;
                case CellKind.StorageBin:
                    room.StorageBins++;
                    break;
                case CellKind.DecorPlant:
                    room.DecorPlants++;
                    break;
            }

            if (IsRoomMachine(cells[x, y]))
            {
                room.MachineBuildings++;
            }
        }

        private bool IsRoomInteriorCell(int x, int y)
        {
            return IsInside(x, y) && IsPassable(x, y) && cells[x, y] != CellKind.ManualAirlock;
        }

        private bool IsRoomMachine(CellKind kind)
        {
            return kind == CellKind.ManualGenerator ||
                kind == CellKind.Battery ||
                kind == CellKind.SmartBattery ||
                kind == CellKind.PowerTransformer ||
                kind == CellKind.CoalGenerator ||
                kind == CellKind.HydrogenGenerator ||
                kind == CellKind.NaturalGasGenerator ||
                kind == CellKind.SteamTurbine ||
                kind == CellKind.SolarPanel ||
                kind == CellKind.SpaceScanner ||
                kind == CellKind.HydrogenFilter ||
                kind == CellKind.RockCrusher ||
                kind == CellKind.OxygenDiffuser ||
                kind == CellKind.WaterPump ||
                kind == CellKind.GasPump ||
                kind == CellKind.Electrolyzer ||
                kind == CellKind.CarbonSkimmer ||
                kind == CellKind.WaterSieve ||
                kind == CellKind.ThermoRegulator ||
                kind == CellKind.SpaceHeater ||
                kind == CellKind.Refrigerator ||
                kind == CellKind.AutoSweeper ||
                kind == CellKind.ConveyorLoader ||
                kind == CellKind.ConveyorChute ||
                kind == CellKind.LiquidReservoir ||
                kind == CellKind.GasReservoir ||
                kind == CellKind.LiquidPipeSensor ||
                kind == CellKind.LiquidShutoff ||
                kind == CellKind.GasPipeSensor ||
                kind == CellKind.GasShutoff ||
                kind == CellKind.AtmoSuitDock ||
                kind == CellKind.AtmoSuitCheckpoint ||
                kind == CellKind.PrintingPod;
        }

        private RoomKind ClassifyRoom(RoomInfo room)
        {
            if (!room.Enclosed || room.Tiles < 4 || room.Tiles > MaxRecognizedRoomTiles)
            {
                return RoomKind.OpenArea;
            }

            int categories = 0;
            if (room.Beds > 0)
            {
                categories++;
            }

            if (room.MessTables > 0)
            {
                categories++;
            }

            if (room.Outhouses > 0 || room.WashBasins > 0)
            {
                categories++;
            }

            if (room.MedicalCots > 0)
            {
                categories++;
            }

            if (room.MassageTables > 0)
            {
                categories++;
            }

            if (room.MachineBuildings > 0)
            {
                categories++;
            }

            if (room.StorageBins > 0)
            {
                categories++;
            }

            if (categories > 1)
            {
                return RoomKind.MixedRoom;
            }

            if (room.Beds > 0)
            {
                return RoomKind.Barracks;
            }

            if (room.MessTables > 0)
            {
                return RoomKind.MessHall;
            }

            if (room.Outhouses > 0 || room.WashBasins > 0)
            {
                return RoomKind.Washroom;
            }

            if (room.MedicalCots > 0)
            {
                return RoomKind.Clinic;
            }

            if (room.MassageTables > 0 || room.DecorPlants >= 2)
            {
                return RoomKind.RecreationRoom;
            }

            if (room.MachineBuildings > 0)
            {
                return RoomKind.MachineRoom;
            }

            if (room.StorageBins > 0)
            {
                return RoomKind.StorageRoom;
            }

            return RoomKind.BasicRoom;
        }

        private RoomInfo RoomAt(int x, int y)
        {
            if (!IsInside(x, y))
            {
                return null;
            }

            EnsureRooms();
            int id = roomIds[x, y];
            return id >= 0 && id < rooms.Count ? rooms[id] : null;
        }

        private RoomKind RoomKindAt(int x, int y)
        {
            RoomInfo room = RoomAt(x, y);
            return room == null ? RoomKind.None : room.Kind;
        }

        private int CountRoomsOfKind(RoomKind kind)
        {
            EnsureRooms();
            int count = 0;
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].Kind == kind)
                {
                    count++;
                }
            }

            return count;
        }

        private string RoomSummary()
        {
            EnsureRooms();
            int barracks = 0;
            int messHalls = 0;
            int washrooms = 0;
            int clinics = 0;
            int recreation = 0;
            int other = 0;
            for (int i = 0; i < rooms.Count; i++)
            {
                switch (rooms[i].Kind)
                {
                    case RoomKind.Barracks:
                        barracks++;
                        break;
                    case RoomKind.MessHall:
                        messHalls++;
                        break;
                    case RoomKind.Washroom:
                        washrooms++;
                        break;
                    case RoomKind.Clinic:
                        clinics++;
                        break;
                    case RoomKind.RecreationRoom:
                        recreation++;
                        break;
                    case RoomKind.BasicRoom:
                    case RoomKind.MachineRoom:
                    case RoomKind.StorageRoom:
                    case RoomKind.MixedRoom:
                        other++;
                        break;
                }
            }

            return "B" + barracks + " M" + messHalls + " W" + washrooms + " C" + clinics + " R" + recreation + " O" + other;
        }

        private float RoomPassiveStressReliefRate(int x, int y)
        {
            switch (RoomKindAt(x, y))
            {
                case RoomKind.Barracks:
                    return 0.30f;
                case RoomKind.MessHall:
                    return 0.22f;
                case RoomKind.Washroom:
                case RoomKind.Clinic:
                    return 0.18f;
                case RoomKind.RecreationRoom:
                    return 0.46f;
                case RoomKind.BasicRoom:
                    return 0.12f;
                default:
                    return 0f;
            }
        }

        private string RoomKindLabel(RoomKind kind)
        {
            switch (kind)
            {
                case RoomKind.OpenArea:
                    return "Open Area";
                case RoomKind.BasicRoom:
                    return "Basic Room";
                case RoomKind.Barracks:
                    return "Barracks";
                case RoomKind.MessHall:
                    return "Mess Hall";
                case RoomKind.Washroom:
                    return "Washroom";
                case RoomKind.Clinic:
                    return "Clinic";
                case RoomKind.RecreationRoom:
                    return "Recreation Room";
                case RoomKind.MachineRoom:
                    return "Machine Room";
                case RoomKind.StorageRoom:
                    return "Storage Room";
                case RoomKind.MixedRoom:
                    return "Mixed Room";
                default:
                    return "No Room";
            }
        }

        private string RoomBonusText(RoomInfo room)
        {
            if (room == null)
            {
                return string.Empty;
            }

            switch (room.Kind)
            {
                case RoomKind.Barracks:
                    return "Bonus: faster rest and passive stress relief.";
                case RoomKind.MessHall:
                    return "Bonus: seated meals remove extra stress.";
                case RoomKind.Washroom:
                    return room.WashBasins > 0
                        ? "Bonus: outhouses and basins lower sickness risk."
                        : "Bonus: outhouse use creates fewer germs.";
                case RoomKind.Clinic:
                    return "Bonus: treatment works faster.";
                case RoomKind.RecreationRoom:
                    return "Bonus: relaxation removes stress faster.";
                case RoomKind.BasicRoom:
                    return "Bonus: small passive stress relief.";
                case RoomKind.MixedRoom:
                    return "Mixed rooms have no specialty bonus.";
                case RoomKind.OpenArea:
                    return room.Enclosed ? "Area is too large or too small for a room." : "Area is open to the asteroid.";
                default:
                    return string.Empty;
            }
        }

        private void LogMiningResult(CellKind mined, string resourceName, float minedAmount, float droppedAmount)
        {
            string minedLabel = CellLabel(mined).ToLowerInvariant();
            if (string.IsNullOrEmpty(resourceName))
            {
                Log("Mined " + minedLabel + ".");
                return;
            }

            if (droppedAmount >= minedAmount - 0.01f)
            {
                Log("Mined " + minedLabel + ". Dropped " + droppedAmount.ToString("0.#") + " " + resourceName + " debris.");
                return;
            }

            Log("Mined " + minedLabel + ". Storage full; lost " + Mathf.Max(0f, minedAmount - droppedAmount).ToString("0.#") + " " + resourceName + ".");
        }

        private void CompleteBuild(Job job)
        {
            cells[job.Cell.x, job.Cell.y] = job.BuildKind;
            equipmentCondition[job.Cell.x, job.Cell.y] = DefaultEquipmentCondition(job.BuildKind);
            looseResourceKind[job.Cell.x, job.Cell.y] = LooseResourceKind.None;
            looseResourceAmount[job.Cell.x, job.Cell.y] = 0f;
            if (job.BuildKind == CellKind.ManualAirlock)
            {
                airlockOpen[job.Cell.x, job.Cell.y] = true;
            }

            if (!IsPassable(job.Cell.x, job.Cell.y))
            {
                oxygen[job.Cell.x, job.Cell.y] = 0f;
                carbonDioxide[job.Cell.x, job.Cell.y] = 0f;
                pollutedOxygen[job.Cell.x, job.Cell.y] = 0f;
                hydrogen[job.Cell.x, job.Cell.y] = 0f;
                chlorine[job.Cell.x, job.Cell.y] = 0f;
                naturalGas[job.Cell.x, job.Cell.y] = 0f;
                germs[job.Cell.x, job.Cell.y] = 0f;
            }

            if (job.BuildKind == CellKind.Battery)
            {
                maxPower += techPowerRegulation ? 100f : 60f;
            }

            if (job.BuildKind == CellKind.SmartBattery)
            {
                maxPower += 180f;
            }

            if (job.BuildKind == CellKind.Planter)
            {
                plantGrowth[job.Cell.x, job.Cell.y] = 0f;
                cropTendedSeconds[job.Cell.x, job.Cell.y] = 0f;
                cropStress[job.Cell.x, job.Cell.y] = 0f;
            }

            if (job.BuildKind == CellKind.SignalSwitch)
            {
                automationSwitchState[job.Cell.x, job.Cell.y] = false;
            }

            terrainDirty = true;
            InvalidateRooms();
            Log("Built " + CellLabel(job.BuildKind) + ".");
        }

        private void CompletePumpWater(Job job, Worker worker)
        {
            if (!TryFindAdjacentWater(job.Cell, out Vector2Int source))
            {
                Log(worker.Name + " found no water to pump.");
                return;
            }

            float pumped = Mathf.Min(24f, waterMass[source.x, source.y]);
            waterMass[source.x, source.y] -= pumped;
            water += pumped;
            WearEquipment(job.Cell, 0.012f);

            if (waterMass[source.x, source.y] <= 0.5f)
            {
                waterMass[source.x, source.y] = 0f;
                cells[source.x, source.y] = CellKind.Empty;
                equipmentCondition[source.x, source.y] = 0f;
                oxygen[source.x, source.y] = NeighborAverage(oxygen, source.x, source.y, 0.12f);
                carbonDioxide[source.x, source.y] = NeighborAverage(carbonDioxide, source.x, source.y, 0.04f);
                hydrogen[source.x, source.y] = NeighborAverage(hydrogen, source.x, source.y, 0f);
                chlorine[source.x, source.y] = NeighborAverage(chlorine, source.x, source.y, 0f);
                naturalGas[source.x, source.y] = NeighborAverage(naturalGas, source.x, source.y, 0f);
                terrainDirty = true;
                gasDirty = true;
                InvalidateRooms();
            }

            Log(worker.Name + " pumped " + pumped.ToString("0") + " kg water.");
        }

        private void CompleteMop(Job job, Worker worker)
        {
            Vector2Int cell = job.Cell;
            if (!IsMoppableSpill(cell))
            {
                Log(worker.Name + " found no shallow spill to mop.");
                return;
            }

            float amount = Mathf.Min(waterMass[cell.x, cell.y], MoppableSpillMaxMass);
            float recovered = amount * MopRecoveryEfficiency;
            bool polluted = IsPollutedMopCell(cell);
            if (polluted)
            {
                pollutedWater += recovered;
                ReduceContaminationAround(cell, 1, 0.34f);
            }
            else
            {
                water += recovered;
            }

            moppedLiquid += amount;
            waterMass[cell.x, cell.y] = 0f;
            if (cells[cell.x, cell.y] == CellKind.Water)
            {
                cells[cell.x, cell.y] = CellKind.Empty;
                equipmentCondition[cell.x, cell.y] = 0f;
            }

            oxygen[cell.x, cell.y] = NeighborAverage(oxygen, cell.x, cell.y, 0.14f);
            carbonDioxide[cell.x, cell.y] = NeighborAverage(carbonDioxide, cell.x, cell.y, 0.04f);
            pollutedOxygen[cell.x, cell.y] = polluted ? NeighborAverage(pollutedOxygen, cell.x, cell.y, 0f) * 0.25f : NeighborAverage(pollutedOxygen, cell.x, cell.y, 0f);
            hydrogen[cell.x, cell.y] = NeighborAverage(hydrogen, cell.x, cell.y, 0f);
            chlorine[cell.x, cell.y] = NeighborAverage(chlorine, cell.x, cell.y, 0f);
            naturalGas[cell.x, cell.y] = NeighborAverage(naturalGas, cell.x, cell.y, 0f);
            germs[cell.x, cell.y] = polluted ? Mathf.Clamp01(NeighborAverage(germs, cell.x, cell.y, 0f) * 0.2f) : Mathf.Clamp01(NeighborAverage(germs, cell.x, cell.y, 0f));

            terrainDirty = true;
            gasDirty = true;
            overlayDirty = true;
            InvalidateRooms();
            Log(worker.Name + " mopped " + amount.ToString("0") + " kg " + (polluted ? "polluted spill." : "spill water."));
        }

        private void CompleteEmptyBottle(Job job, Worker worker)
        {
            Vector2Int emptier = job.Cell;
            if (!CanEmptyBottleAt(emptier))
            {
                Log(worker.Name + " found no valid bottle emptying target.");
                return;
            }

            bool polluted = pollutedWater > 0.5f;
            float available = polluted ? pollutedWater : CleanWaterAvailableForBottleEmptier();
            if (available <= 0.01f)
            {
                Log(worker.Name + " found no bottled liquid to empty.");
                return;
            }

            if (!TryFindBottleEmptierOutput(emptier, out Vector2Int output))
            {
                Log("Bottle Emptier output is blocked or full.");
                return;
            }

            float amount = Mathf.Min(BottleEmptierPourAmount, available);
            amount = Mathf.Min(amount, 120f - waterMass[output.x, output.y]);
            if (amount <= 0.01f)
            {
                Log("Bottle Emptier output is full.");
                return;
            }

            if (polluted)
            {
                pollutedWater = Mathf.Max(0f, pollutedWater - amount);
            }
            else
            {
                water = Mathf.Max(0f, water - amount);
            }

            ReleaseWaterToCell(output, amount);
            if (polluted)
            {
                pollutedOxygen[output.x, output.y] = Mathf.Min(2.2f, pollutedOxygen[output.x, output.y] + 0.16f + amount * 0.004f);
                germs[output.x, output.y] = Mathf.Clamp01(germs[output.x, output.y] + 0.58f);
            }

            bottleEmptiedLiquid += amount;
            milestoneBottleEmptying |= bottleEmptiedLiquid >= 12f;
            WearEquipment(emptier, 0.006f);
            terrainDirty = true;
            gasDirty = true;
            overlayDirty = true;
            InvalidateRooms();
            Log(worker.Name + " emptied " + amount.ToString("0.#") + " kg " + (polluted ? "polluted water." : "water."));
        }

        private bool CanEmptyBottleAt(Vector2Int cell)
        {
            return IsInside(cell.x, cell.y) &&
                cells[cell.x, cell.y] == CellKind.BottleEmptier &&
                CanUseEquipment(cell) &&
                HasStoredLiquidForBottleEmptier() &&
                TryFindBottleEmptierOutput(cell, out _);
        }

        private bool HasStoredLiquidForBottleEmptier()
        {
            return pollutedWater > 0.5f || CleanWaterAvailableForBottleEmptier() > 0.5f;
        }

        private float CleanWaterAvailableForBottleEmptier()
        {
            return Mathf.Max(0f, water - BottleEmptierCleanWaterReserve);
        }

        private bool TryFindBottleEmptierOutput(Vector2Int emptier, out Vector2Int output)
        {
            Vector2Int[] offsets =
            {
                new Vector2Int(0, -1),
                new Vector2Int(-1, 0),
                new Vector2Int(1, 0),
                new Vector2Int(0, 1)
            };

            foreach (Vector2Int offset in offsets)
            {
                Vector2Int candidate = new Vector2Int(emptier.x + offset.x, emptier.y + offset.y);
                if (!IsInside(candidate.x, candidate.y))
                {
                    continue;
                }

                if (CanLiquidOccupy(candidate.x, candidate.y) &&
                    LiquidFreeCapacity(candidate.x, candidate.y) > LiquidTileCapacity - 120f)
                {
                    output = candidate;
                    return true;
                }
            }

            output = default;
            return false;
        }

        private bool AnyBottleEmptierHasOutput()
        {
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] == CellKind.BottleEmptier &&
                        TryFindBottleEmptierOutput(new Vector2Int(x, y), out _))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void ReduceContaminationAround(Vector2Int center, int radius, float multiplier)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    int x = center.x + dx;
                    int y = center.y + dy;
                    if (!IsInside(x, y))
                    {
                        continue;
                    }

                    pollutedOxygen[x, y] = Mathf.Max(0f, pollutedOxygen[x, y] * multiplier);
                    germs[x, y] = Mathf.Clamp01(germs[x, y] * multiplier);
                }
            }
        }

        private void CompleteRepair(Job job, Worker worker)
        {
            Vector2Int cell = job.Cell;
            if (!NeedsRepair(cell))
            {
                Log(worker.Name + " found no repair needed.");
                return;
            }

            if (metal < RepairMetalCost)
            {
                Log("Repair needs " + RepairMetalCost.ToString("0.#") + " kg metal.");
                return;
            }

            metal = Mathf.Max(0f, metal - RepairMetalCost);
            equipmentCondition[cell.x, cell.y] = 1f;
            repairsCompleted++;
            milestoneMaintenance = true;
            terrainDirty = true;
            overlayDirty = true;
            Log(worker.Name + " repaired " + CellLabel(cells[cell.x, cell.y]) + ".");
        }

        private void CompleteRescue(Job job, Worker rescuer)
        {
            Worker patient = FindWorkerByName(job.TargetWorkerName);
            if (!NeedsRescue(patient))
            {
                Log(rescuer.Name + " found no incapacitated duplicant to rescue.");
                return;
            }

            if (!TryFindRescueCot(patient, out Vector2Int cotCell))
            {
                Log("No Medical Cot available for rescue.");
                return;
            }

            ClearAssignment(patient);
            patient.Cell = cotCell;
            if (patient.Transform != null)
            {
                patient.Transform.position = CellCenter(cotCell);
            }

            patient.Health = 18f;
            patient.Fatigue = Mathf.Min(100f, Mathf.Max(patient.Fatigue, 82f));
            patient.Stress = Mathf.Min(100f, Mathf.Max(patient.Stress, 78f));
            patient.Calories = Mathf.Max(patient.Calories, 450f);
            patient.IncapacitatedSeconds = 0f;
            patient.Activity = "Recovering";
            rescuesCompleted++;
            milestoneEmergencyResponse = true;
            overlayDirty = true;
            gasDirty = true;
            Log(rescuer.Name + " rescued " + patient.Name + " to a Medical Cot.");

            if (NeedsTreatment(patient) && !HasTreatmentJobFor(patient.Name))
            {
                TryCreateTreatmentJob(patient);
            }
        }

        private bool TryFindRescueCot(Worker patient, out Vector2Int cotCell)
        {
            cotCell = new Vector2Int(-1, -1);
            int bestDistance = int.MaxValue;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    Vector2Int candidate = new Vector2Int(x, y);
                    if (cells[x, y] != CellKind.MedicalCot || !IsPassable(x, y) || IsMedicalCotReserved(candidate))
                    {
                        continue;
                    }

                    int distance = Mathf.Abs(patient.Cell.x - x) + Mathf.Abs(patient.Cell.y - y);
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        cotCell = candidate;
                    }
                }
            }

            if (cotCell.x >= 0)
            {
                return true;
            }

            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] == CellKind.MedicalCot && IsPassable(x, y))
                    {
                        cotCell = new Vector2Int(x, y);
                        return true;
                    }
                }
            }

            return false;
        }

        private void CompleteTreatment(Job job, Worker worker)
        {
            float waterUsed = Mathf.Min(water, 3f);
            water -= waterUsed;
            float clinicFactor = RoomKindAt(job.Cell.x, job.Cell.y) == RoomKind.Clinic ? 1.22f : 1f;
            float careQuality = (waterUsed >= 1f ? 1f : 0.55f) * clinicFactor;
            worker.GermExposure = Mathf.Max(0f, worker.GermExposure - 42f * careQuality);
            worker.Sickness = Mathf.Max(0f, worker.Sickness - 34f * careQuality);
            worker.Health = Mathf.Min(100f, worker.Health + 22f * careQuality);
            worker.Stress = Mathf.Max(0f, worker.Stress - 14f);
            Log(worker.Name + " received treatment.");
        }

        private void CompleteToiletUse(Job job, Worker worker)
        {
            bool washroom = RoomKindAt(job.Cell.x, job.Cell.y) == RoomKind.Washroom;
            worker.Bladder = 0f;
            worker.Stress = Mathf.Max(0f, worker.Stress - (washroom ? 8f : 5f));
            worker.GermExposure = Mathf.Min(100f, worker.GermExposure + (washroom ? 8f : 14f));
            if (IsInside(job.Cell.x, job.Cell.y))
            {
                pollutedOxygen[job.Cell.x, job.Cell.y] = Mathf.Min(1.8f, pollutedOxygen[job.Cell.x, job.Cell.y] + (washroom ? 0.10f : 0.18f));
                germs[job.Cell.x, job.Cell.y] = Mathf.Clamp01(germs[job.Cell.x, job.Cell.y] + (washroom ? 0.08f : 0.16f));
                AddPollutedDirt(3.5f, job.Cell);
                gasDirty = true;
                overlayDirty = true;
            }

            Log(worker.Name + " used the outhouse. Polluted dirt accumulated.");
            if (NeedsHandWash(worker) && !HasWashHandsJobFor(worker.Name))
            {
                TryCreateWashHandsJob(worker);
            }
        }

        private void CompleteWashHands(Job job, Worker worker)
        {
            float waterUsed = Mathf.Min(water, WashBasinWaterUse);
            if (waterUsed <= 0.05f)
            {
                Log("Wash Basin needs clean water.");
                return;
            }

            water = Mathf.Max(0f, water - waterUsed);
            pollutedWater += waterUsed * WashBasinPollutedWaterOutput;
            float washQuality = Mathf.Clamp01(waterUsed / WashBasinWaterUse);
            worker.GermExposure = Mathf.Max(0f, worker.GermExposure - WashBasinGermReduction * washQuality);
            worker.Sickness = Mathf.Max(0f, worker.Sickness - 5f * washQuality);
            worker.Stress = Mathf.Max(0f, worker.Stress - 1.5f);
            if (IsInside(job.Cell.x, job.Cell.y))
            {
                germs[job.Cell.x, job.Cell.y] = Mathf.Clamp01(germs[job.Cell.x, job.Cell.y] * 0.25f);
                pollutedOxygen[job.Cell.x, job.Cell.y] = Mathf.Max(0f, pollutedOxygen[job.Cell.x, job.Cell.y] - 0.06f * washQuality);
                WearEquipment(job.Cell, 0.006f);
                gasDirty = true;
                overlayDirty = true;
            }

            handsWashed++;
            milestoneHygiene |= CountCells(CellKind.WashBasin) > 0 && handsWashed > 0 && worker.GermExposure <= 30f;
            Log(worker.Name + " washed hands at the Wash Basin.");
        }

        private void CompleteRelaxation(Job job, Worker worker)
        {
            worker.Stress = Mathf.Min(worker.Stress, RoomKindAt(job.Cell.x, job.Cell.y) == RoomKind.RecreationRoom ? 22f : 30f);
            worker.Morale = Mathf.Min(10f, worker.Morale + (RoomKindAt(job.Cell.x, job.Cell.y) == RoomKind.RecreationRoom ? 1.2f : 0.8f));
            Log(worker.Name + " finished stress relief.");
        }

        private void CompleteEat(Job job, Worker worker)
        {
            if (!EatStoredFood(worker, true))
            {
                Log(worker.Name + " found no food at the Mess Table.");
            }
        }

        private void CompleteCompost(Job job, Worker worker)
        {
            float processed = Mathf.Min(pollutedDirt, 8f);
            if (processed <= 0.01f)
            {
                Log(worker.Name + " found no polluted dirt to compost.");
                return;
            }

            pollutedDirt = Mathf.Max(0f, pollutedDirt - processed);
            float recovered = StoreDryResource(ref dirt, processed * 0.78f);
            compostedPollutedDirt += processed;
            VentPollutedOxygen(job.Cell, 0.04f, 0.08f);
            AddHeat(job.Cell, 0.35f, 1);
            Log(worker.Name + " composted " + processed.ToString("0.#") + " kg polluted dirt into " + recovered.ToString("0.#") + " kg dirt.");
        }

        private void CompleteGroomHatch(Job job, Worker worker)
        {
            HatchCritter hatch = FindGroomableHatch(job.Cell);
            if (hatch == null)
            {
                Log(worker.Name + " found no hatch ready for grooming.");
                return;
            }

            hatch.GroomedSeconds = HatchGroomedSeconds;
            hatch.Happiness = Mathf.Max(hatch.Happiness, 78f);
            hatchesGroomed++;
            worker.Stress = Mathf.Max(0f, worker.Stress - 3f);
            Log(worker.Name + " groomed " + hatch.Name + ". Hatch coal output improved.");
        }

        private void AddFreshFood(float amount, float freshness)
        {
            if (amount <= 0f)
            {
                return;
            }

            float totalFood = food + amount;
            foodFreshness = totalFood <= 0f
                ? Mathf.Clamp01(freshness)
                : Mathf.Clamp01((foodFreshness * food + Mathf.Clamp01(freshness) * amount) / totalFood);
            food = totalFood;
        }

        private float NeighborAverage(float[,] gas, int x, int y, float fallback)
        {
            float total = 0f;
            int count = 0;
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (Mathf.Abs(dx) + Mathf.Abs(dy) != 1)
                    {
                        continue;
                    }

                    int nx = x + dx;
                    int ny = y + dy;
                    if (IsInside(nx, ny) && IsPassable(nx, ny))
                    {
                        total += gas[nx, ny];
                        count++;
                    }
                }
            }

            return count == 0 ? fallback : total / count;
        }

        private void ClearAssignment(Worker worker)
        {
            if (worker.AssignedJob != null && worker.AssignedJob.AssignedWorker == worker)
            {
                worker.AssignedJob.AssignedWorker = null;
            }

            worker.AssignedJob = null;
            worker.Path.Clear();
            worker.PathIndex = 0;
            worker.Activity = "Idle";
        }

        private bool TryFindPathToJob(Vector2Int start, Job job, out List<Vector2Int> path)
        {
            List<Vector2Int> targets = GetWorkPositions(job);
            return TryFindPath(start, targets, out path);
        }

        private int CountUnreachableJobs()
        {
            int count = 0;
            foreach (Job job in jobs)
            {
                if (job.AssignedWorker != null || !IsJobValid(job))
                {
                    continue;
                }

                if (!CanAnyActiveWorkerReachJob(job, out _))
                {
                    count++;
                }
            }

            return count;
        }

        private bool CanAnyActiveWorkerReachJob(Job job, out int shortestPath)
        {
            shortestPath = int.MaxValue;
            foreach (Worker worker in workers)
            {
                if (worker.Health <= 0f || !CanWorkerTakeJob(worker, job))
                {
                    continue;
                }

                if (TryFindPathToJob(worker.Cell, job, out List<Vector2Int> path))
                {
                    shortestPath = Mathf.Min(shortestPath, path.Count);
                }
            }

            return shortestPath < int.MaxValue;
        }

        private string JobReachabilityText(Job job)
        {
            if (job == null)
            {
                return string.Empty;
            }

            if (job.AssignedWorker != null)
            {
                return "Assigned to " + job.AssignedWorker.Name + ".";
            }

            if (!IsJobValid(job))
            {
                return "Job invalid: target changed or resources missing.";
            }

            List<Vector2Int> workPositions = GetWorkPositions(job);
            if (workPositions.Count == 0)
            {
                return "Job blocked: no standable work cell. Build floor/ladder access.";
            }

            if (CountActiveWorkers() == 0)
            {
                return "Job blocked: no active duplicants.";
            }

            if (CanAnyActiveWorkerReachJob(job, out int shortestPath))
            {
                return "Job reachable: " + shortestPath + " steps.";
            }

            return string.IsNullOrEmpty(job.TargetWorkerName)
                ? "Job unreachable: build ladder/floor/door access to a work cell."
                : "Job blocked: assigned duplicant is unavailable.";
        }

        private List<Vector2Int> GetWorkPositions(Job job)
        {
            List<Vector2Int> targets = new List<Vector2Int>();
            if (job.Type == JobType.Dig ||
                job.Type == JobType.Mop ||
                (job.Type == JobType.Deconstruct && job.BuildKind != CellKind.Empty && IsSolidTile(job.BuildKind)))
            {
                AddPassableTarget(targets, job.Cell.x + 1, job.Cell.y);
                AddPassableTarget(targets, job.Cell.x - 1, job.Cell.y);
                AddPassableTarget(targets, job.Cell.x, job.Cell.y + 1);
                AddPassableTarget(targets, job.Cell.x, job.Cell.y - 1);
            }
            else if (job.Type == JobType.Rescue)
            {
                AddPassableTarget(targets, job.Cell.x, job.Cell.y);
                AddPassableTarget(targets, job.Cell.x + 1, job.Cell.y);
                AddPassableTarget(targets, job.Cell.x - 1, job.Cell.y);
                AddPassableTarget(targets, job.Cell.x, job.Cell.y + 1);
                AddPassableTarget(targets, job.Cell.x, job.Cell.y - 1);
            }
            else if (job.Type == JobType.Sleep || job.Type == JobType.Treat || job.Type == JobType.UseToilet || job.Type == JobType.Relax)
            {
                AddPassableTarget(targets, job.Cell.x, job.Cell.y);
            }
            else
            {
                AddPassableTarget(targets, job.Cell.x, job.Cell.y);
                AddPassableTarget(targets, job.Cell.x + 1, job.Cell.y);
                AddPassableTarget(targets, job.Cell.x - 1, job.Cell.y);
                AddPassableTarget(targets, job.Cell.x, job.Cell.y + 1);
                AddPassableTarget(targets, job.Cell.x, job.Cell.y - 1);
            }

            return targets;
        }

        private void AddPassableTarget(List<Vector2Int> targets, int x, int y)
        {
            if (IsCharacterStandableCell(new Vector2Int(x, y)))
            {
                targets.Add(new Vector2Int(x, y));
            }
        }

        private bool TryFindPath(Vector2Int start, List<Vector2Int> targets, out List<Vector2Int> path)
        {
            path = null;
            if (targets.Count == 0 || !IsInside(start.x, start.y))
            {
                return false;
            }

            HashSet<int> targetKeys = new HashSet<int>();
            foreach (Vector2Int target in targets)
            {
                targetKeys.Add(Key(target.x, target.y));
            }

            bool[,] visited = new bool[WorldWidth, WorldHeight];
            Vector2Int[,] previous = new Vector2Int[WorldWidth, WorldHeight];
            Queue<Vector2Int> frontier = new Queue<Vector2Int>();
            frontier.Enqueue(start);
            visited[start.x, start.y] = true;
            previous[start.x, start.y] = new Vector2Int(-1, -1);

            Vector2Int found = new Vector2Int(-1, -1);
            while (frontier.Count > 0)
            {
                Vector2Int current = frontier.Dequeue();
                if (targetKeys.Contains(Key(current.x, current.y)))
                {
                    found = current;
                    break;
                }

                EnqueuePathNeighbor(current, 1, 0, visited, previous, frontier);
                EnqueuePathNeighbor(current, -1, 0, visited, previous, frontier);
                EnqueuePathNeighbor(current, 0, 1, visited, previous, frontier);
                EnqueuePathNeighbor(current, 0, -1, visited, previous, frontier);
            }

            if (found.x < 0)
            {
                return false;
            }

            List<Vector2Int> reversed = new List<Vector2Int>();
            Vector2Int step = found;
            while (step != start)
            {
                reversed.Add(step);
                step = previous[step.x, step.y];
            }

            reversed.Reverse();
            path = reversed;
            return true;
        }

        private void EnqueuePathNeighbor(Vector2Int current, int dx, int dy, bool[,] visited, Vector2Int[,] previous, Queue<Vector2Int> frontier)
        {
            int nx = current.x + dx;
            int ny = current.y + dy;
            if (!IsInside(nx, ny) || visited[nx, ny])
            {
                return;
            }

            Vector2Int next = new Vector2Int(nx, ny);
            if (!CanTraversePathStep(current, next))
            {
                return;
            }

            visited[nx, ny] = true;
            previous[nx, ny] = current;
            frontier.Enqueue(new Vector2Int(nx, ny));
        }

        private bool IsJobValid(Job job)
        {
            if (job == null || job.Cancelled || !IsInside(job.Cell.x, job.Cell.y))
            {
                return false;
            }

            switch (job.Type)
            {
                case JobType.Dig:
                    return IsNaturalSolid(cells[job.Cell.x, job.Cell.y]);
                case JobType.Build:
                    return CanPlaceBuild(job.Cell, job.BuildKind);
                case JobType.BuildWire:
                    return CanPlacePowerWire(job.Cell);
                case JobType.BuildAutomationWire:
                    return CanPlaceAutomationWire(job.Cell) && techPowerRegulation;
                case JobType.BuildPipe:
                    return CanPlaceLiquidPipe(job.Cell);
                case JobType.BuildGasPipe:
                    return CanPlaceGasPipe(job.Cell) && techAirSystems;
                case JobType.BuildShippingRail:
                    return CanPlaceShippingRail(job.Cell) && techPowerRegulation;
                case JobType.Deconstruct:
                    return IsDeconstructJobValid(job);
                case JobType.Mop:
                    return IsMoppableSpill(job.Cell);
                case JobType.Repair:
                    return cells[job.Cell.x, job.Cell.y] == job.BuildKind && NeedsRepair(job.Cell) && metal >= RepairMetalCost;
                case JobType.Rescue:
                    Worker rescueTarget = FindWorkerByName(job.TargetWorkerName);
                    return rescueTarget != null &&
                        rescueTarget.Health <= 0f &&
                        rescueTarget.Cell == job.Cell &&
                        CountCells(CellKind.MedicalCot) > 0;
                case JobType.Sweep:
                    return HasLooseResource(job.Cell) && DryResourceFreeSpace() > 0.01f;
                case JobType.OperateGenerator:
                    return cells[job.Cell.x, job.Cell.y] == CellKind.ManualGenerator && CanGeneratorOperate(job.Cell);
                case JobType.Harvest:
                    return cells[job.Cell.x, job.Cell.y] == CellKind.Planter && plantGrowth[job.Cell.x, job.Cell.y] >= 1f;
                case JobType.TendCrop:
                    return IsCropTendingTarget(job.Cell) &&
                        techFoodPreparation &&
                        (pollutedDirt >= CropTendPollutedDirtCost || dirt >= CropTendDirtFallbackCost) &&
                        TryFindFarmStationForCrop(job.Cell, out _);
                case JobType.PumpWater:
                    return cells[job.Cell.x, job.Cell.y] == CellKind.WaterPump && CanUseEquipment(job.Cell) && TryFindAdjacentWater(job.Cell, out _);
                case JobType.EmptyBottle:
                    return CanEmptyBottleAt(job.Cell);
                case JobType.Research:
                    return cells[job.Cell.x, job.Cell.y] == CellKind.ResearchStation && researchPoints < 32f && power > 1f && CanPoweredMachineRun(job.Cell);
                case JobType.Cook:
                    return cells[job.Cell.x, job.Cell.y] == CellKind.MicrobeMusher && techFoodPreparation && water >= 4f && dirt >= 1f && power > 1f && CanPoweredMachineRun(job.Cell);
                case JobType.RefineMetal:
                    return cells[job.Cell.x, job.Cell.y] == CellKind.RockCrusher && techPowerRegulation && metal >= RockCrusherOrePerJob && power >= RockCrusherPowerCost && CanPoweredMachineRun(job.Cell);
                case JobType.Compost:
                    return cells[job.Cell.x, job.Cell.y] == CellKind.Compost && CanUseEquipment(job.Cell) && pollutedDirt >= 4f;
                case JobType.Sleep:
                    return cells[job.Cell.x, job.Cell.y] == CellKind.Bed && (job.AssignedWorker != null || IsRestTime() || AnyWorkerNeedsSleep());
                case JobType.Treat:
                    Worker patient = FindWorkerByName(job.TargetWorkerName);
                    return cells[job.Cell.x, job.Cell.y] == CellKind.MedicalCot &&
                        patient != null &&
                        patient.Health > 0f &&
                        (job.AssignedWorker != null || NeedsTreatment(patient));
                case JobType.UseToilet:
                    Worker toiletUser = FindWorkerByName(job.TargetWorkerName);
                    return cells[job.Cell.x, job.Cell.y] == CellKind.Outhouse &&
                        toiletUser != null &&
                        toiletUser.Health > 0f &&
                        (job.AssignedWorker != null || NeedsToilet(toiletUser));
                case JobType.WashHands:
                    Worker washer = FindWorkerByName(job.TargetWorkerName);
                    return cells[job.Cell.x, job.Cell.y] == CellKind.WashBasin &&
                        CanUseEquipment(job.Cell) &&
                        water > 0.05f &&
                        washer != null &&
                        washer.Health > 0f &&
                        (job.AssignedWorker != null || NeedsHandWash(washer));
                case JobType.Eat:
                    Worker diner = FindWorkerByName(job.TargetWorkerName);
                    return cells[job.Cell.x, job.Cell.y] == CellKind.MessTable &&
                        diner != null &&
                        diner.Health > 0f &&
                        food >= 700f &&
                        (job.AssignedWorker != null || NeedsFood(diner));
                case JobType.Relax:
                    Worker stressedWorker = FindWorkerByName(job.TargetWorkerName);
                    return cells[job.Cell.x, job.Cell.y] == CellKind.MassageTable &&
                        stressedWorker != null &&
                        stressedWorker.Health > 0f &&
                        (job.AssignedWorker != null || NeedsRelaxation(stressedWorker));
                case JobType.GroomHatch:
                    return cells[job.Cell.x, job.Cell.y] == CellKind.RanchingStation &&
                        (job.AssignedWorker != null || FindGroomableHatch(job.Cell) != null);
                default:
                    return false;
            }
        }

        private bool IsDeconstructJobValid(Job job)
        {
            if (job == null || !IsInside(job.Cell.x, job.Cell.y))
            {
                return false;
            }

            if (job.BuildKind != CellKind.Empty)
            {
                return cells[job.Cell.x, job.Cell.y] == job.BuildKind && IsDeconstructableBuilding(job.BuildKind);
            }

            return (job.RemovePowerWire && powerWire[job.Cell.x, job.Cell.y]) ||
                (job.RemoveAutomationWire && automationWire[job.Cell.x, job.Cell.y]) ||
                (job.RemoveLiquidPipe && liquidPipe[job.Cell.x, job.Cell.y]) ||
                (job.RemoveGasPipe && gasPipe[job.Cell.x, job.Cell.y]) ||
                (job.RemoveShippingRail && shippingRail[job.Cell.x, job.Cell.y]);
        }

        private string DeconstructTargetLabel(Job job)
        {
            if (job.BuildKind != CellKind.Empty)
            {
                return CellLabel(job.BuildKind);
            }

            if (job.RemovePowerWire)
            {
                return "Power Wire";
            }

            if (job.RemoveAutomationWire)
            {
                return "Automation Wire";
            }

            if (job.RemoveLiquidPipe)
            {
                return "Liquid Pipe";
            }

            if (job.RemoveGasPipe)
            {
                return "Gas Pipe";
            }

            if (job.RemoveShippingRail)
            {
                return "Shipping Rail";
            }

            return "Structure";
        }

        private void UpdatePoweredWires()
        {
            Array.Clear(poweredWire, 0, poweredWire.Length);
            Queue<Vector2Int> frontier = new Queue<Vector2Int>();
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (powerWire[x, y] && IsWireTouchingPowerInfrastructure(x, y))
                    {
                        poweredWire[x, y] = true;
                        frontier.Enqueue(new Vector2Int(x, y));
                    }
                }
            }

            while (frontier.Count > 0)
            {
                Vector2Int cell = frontier.Dequeue();
                AddPoweredWireNeighbor(cell.x + 1, cell.y, frontier);
                AddPoweredWireNeighbor(cell.x - 1, cell.y, frontier);
                AddPoweredWireNeighbor(cell.x, cell.y + 1, frontier);
                AddPoweredWireNeighbor(cell.x, cell.y - 1, frontier);
            }
        }

        private void UpdatePowerLoad(float deltaTime, bool applyEffects)
        {
            RecalculatePowerLoad(applyEffects ? Mathf.Max(0f, deltaTime) : 0f);
            if (!applyEffects)
            {
                return;
            }

            if (ApplyWireOverloadEffects(deltaTime))
            {
                UpdatePoweredWires();
                RecalculatePowerLoad(0f);
            }
        }

        private void RecalculatePowerLoad(float deliveredDeltaTime)
        {
            Array.Clear(wireLoad, 0, wireLoad.Length);
            Array.Clear(overloadedWire, 0, overloadedWire.Length);

            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    CellKind kind = cells[x, y];
                    Vector2Int cell = new Vector2Int(x, y);
                    if (!RequiresPower(kind) || IsBrokenEquipment(cell) || IsEquipmentSubmerged(cell) || !HasPoweredCircuit(cell) || !MachineAutomationAllows(cell))
                    {
                        continue;
                    }

                    AddPowerDemandNear(cell, PowerDemandForKind(kind), deliveredDeltaTime);
                }
            }

            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (powerWire[x, y] && poweredWire[x, y] && wireLoad[x, y] > PowerLoadLimitForWire(x, y) + 0.001f)
                    {
                        overloadedWire[x, y] = true;
                    }
                }
            }
        }

        private bool ApplyWireOverloadEffects(float deltaTime)
        {
            if (deltaTime <= 0f)
            {
                return false;
            }

            bool brokeWire = false;
            int firstBreakX = -1;
            int firstBreakY = -1;
            int overloadedCount = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (powerWire[x, y] && poweredWire[x, y] && overloadedWire[x, y])
                    {
                        overloadedCount++;
                        float limit = Mathf.Max(0.001f, PowerLoadLimitForWire(x, y));
                        float overloadRatio = Mathf.Clamp01((wireLoad[x, y] - limit) / limit);
                        wireOverloadStress[x, y] += deltaTime * (0.65f + overloadRatio);
                        AddHeat(new Vector2Int(x, y), WireOverloadHeatRate * deltaTime * (1f + overloadRatio), 1);

                        if (wireOverloadStress[x, y] >= WireOverloadBreakStress)
                        {
                            powerWire[x, y] = false;
                            poweredWire[x, y] = false;
                            overloadedWire[x, y] = false;
                            wireLoad[x, y] = 0f;
                            wireOverloadStress[x, y] = 0f;
                            if (!brokeWire)
                            {
                                firstBreakX = x;
                                firstBreakY = y;
                            }

                            brokeWire = true;
                        }
                    }
                    else
                    {
                        wireOverloadStress[x, y] = Mathf.Max(0f, wireOverloadStress[x, y] - deltaTime * 0.5f);
                    }
                }
            }

            if (overloadedCount > 0)
            {
                overloadedWireSeconds += deltaTime;
                gasDirty = true;
                overlayDirty = true;
            }

            if (brokeWire)
            {
                gasDirty = true;
                overlayDirty = true;
                Log("Power Wire overloaded and burned out at " + firstBreakX + ", " + firstBreakY + ".");
            }

            return brokeWire;
        }

        private void AddPowerDemandNear(Vector2Int cell, float demand, float deliveredDeltaTime)
        {
            if (demand <= 0f)
            {
                return;
            }

            int wireCount = CountNearbyPoweredWires(cell);
            if (wireCount <= 0)
            {
                return;
            }

            float share = demand / wireCount;
            AddPowerDemandToWire(cell.x, cell.y, share, deliveredDeltaTime);
            AddPowerDemandToWire(cell.x + 1, cell.y, share, deliveredDeltaTime);
            AddPowerDemandToWire(cell.x - 1, cell.y, share, deliveredDeltaTime);
            AddPowerDemandToWire(cell.x, cell.y + 1, share, deliveredDeltaTime);
            AddPowerDemandToWire(cell.x, cell.y - 1, share, deliveredDeltaTime);
        }

        private int CountNearbyPoweredWires(Vector2Int cell)
        {
            int count = 0;
            count += IsPoweredWireCell(cell.x, cell.y) ? 1 : 0;
            count += IsPoweredWireCell(cell.x + 1, cell.y) ? 1 : 0;
            count += IsPoweredWireCell(cell.x - 1, cell.y) ? 1 : 0;
            count += IsPoweredWireCell(cell.x, cell.y + 1) ? 1 : 0;
            count += IsPoweredWireCell(cell.x, cell.y - 1) ? 1 : 0;
            return count;
        }

        private void AddPowerDemandToWire(int x, int y, float amount, float deliveredDeltaTime)
        {
            if (!IsPoweredWireCell(x, y))
            {
                return;
            }

            wireLoad[x, y] += amount;
            if (deliveredDeltaTime > 0f && HasTransformerProtection(x, y))
            {
                transformedPowerDelivered += amount * deliveredDeltaTime;
            }
        }

        private bool IsPoweredWireCell(int x, int y)
        {
            return IsInside(x, y) && powerWire[x, y] && poweredWire[x, y];
        }

        private float PowerLoadLimitForWire(int x, int y)
        {
            if (!IsInside(x, y) || !powerWire[x, y])
            {
                return 0f;
            }

            return WireSafeLoad + (HasTransformerProtection(x, y) ? PowerTransformerLoadBonus : 0f);
        }

        private bool HasTransformerProtection(int x, int y)
        {
            return IsPowerTransformerCell(x, y) ||
                IsPowerTransformerCell(x + 1, y) ||
                IsPowerTransformerCell(x - 1, y) ||
                IsPowerTransformerCell(x, y + 1) ||
                IsPowerTransformerCell(x, y - 1);
        }

        private bool IsPowerTransformerCell(int x, int y)
        {
            return IsInside(x, y) && cells[x, y] == CellKind.PowerTransformer && !IsBrokenEquipment(new Vector2Int(x, y));
        }

        private float PowerDemandForKind(CellKind kind)
        {
            switch (kind)
            {
                case CellKind.OxygenDiffuser:
                    return 0.95f;
                case CellKind.AirDeodorizer:
                    return 0.18f;
                case CellKind.ResearchStation:
                    return 0.40f;
                case CellKind.MicrobeMusher:
                    return 0.42f;
                case CellKind.RockCrusher:
                    return 1.20f;
                case CellKind.AtmoSuitDock:
                    return SuitDockPowerRate;
                case CellKind.GasPump:
                    return 0.50f;
                case CellKind.Electrolyzer:
                    return ElectrolyzerPowerRate;
                case CellKind.CarbonSkimmer:
                    return CarbonSkimmerPowerRate;
                case CellKind.WaterSieve:
                    return WaterSievePowerRate;
                case CellKind.HydrogenFilter:
                    return HydrogenFilterPowerRate;
                case CellKind.SpaceScanner:
                    return SpaceScannerPowerRate;
                case CellKind.Refrigerator:
                    return 0.24f;
                case CellKind.AutoSweeper:
                    return AutoSweeperPowerRate;
                case CellKind.ConveyorLoader:
                    return ConveyorLoaderPowerRate;
                case CellKind.SpaceHeater:
                    return 0.80f;
                case CellKind.ThermoRegulator:
                    return 1.10f;
                default:
                    return 0f;
            }
        }

        private int CountOverloadedPowerWires()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (overloadedWire[x, y])
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private float MaxPowerWireLoad()
        {
            float maxLoad = 0f;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (wireLoad[x, y] > maxLoad)
                    {
                        maxLoad = wireLoad[x, y];
                    }
                }
            }

            return maxLoad;
        }

        private bool IsCropTendingTarget(Vector2Int crop)
        {
            return IsInside(crop.x, crop.y) &&
                cells[crop.x, crop.y] == CellKind.Planter &&
                plantGrowth[crop.x, crop.y] < 1f &&
                cropStress[crop.x, crop.y] < CropWiltThresholdSeconds &&
                cropTendedSeconds[crop.x, crop.y] <= 5f;
        }

        private string CropStressReason(Vector2Int crop)
        {
            if (!IsInside(crop.x, crop.y) || cells[crop.x, crop.y] != CellKind.Planter)
            {
                return "no crop";
            }

            if (waterMass[crop.x, crop.y] >= CropFloodWaterMass)
            {
                return "flooding";
            }

            float localTemperature = temperature[crop.x, crop.y];
            if (localTemperature < -2f || localTemperature > 45f)
            {
                return "lethal temperature";
            }

            if (localTemperature < 8f || localTemperature > 34f)
            {
                return "bad temperature";
            }

            if (oxygen[crop.x, crop.y] <= 0.18f)
            {
                return "low oxygen";
            }

            if (TileGasTotal(crop.x, crop.y) > OverpressureDamageThreshold)
            {
                return "overpressure";
            }

            if (water <= 0.05f)
            {
                return "no irrigation water";
            }

            return cropStress[crop.x, crop.y] > 0.05f ? "recovering" : "healthy";
        }

        private int CountStressedCrops()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] == CellKind.Planter && cropStress[x, y] >= CropStressThresholdSeconds)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountWiltingCrops()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] == CellKind.Planter && cropStress[x, y] >= CropWiltThresholdSeconds)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private bool TryFindFarmStationForCrop(Vector2Int crop, out Vector2Int station)
        {
            station = new Vector2Int(-1, -1);
            int bestDistance = int.MaxValue;
            for (int y = Mathf.Max(0, crop.y - FarmStationRange); y <= Mathf.Min(WorldHeight - 1, crop.y + FarmStationRange); y++)
            {
                for (int x = Mathf.Max(0, crop.x - FarmStationRange); x <= Mathf.Min(WorldWidth - 1, crop.x + FarmStationRange); x++)
                {
                    if (cells[x, y] != CellKind.FarmStation || !CanUseEquipment(new Vector2Int(x, y)))
                    {
                        continue;
                    }

                    int distance = Mathf.Abs(crop.x - x) + Mathf.Abs(crop.y - y);
                    if (distance <= FarmStationRange && distance < bestDistance)
                    {
                        bestDistance = distance;
                        station = new Vector2Int(x, y);
                    }
                }
            }

            return station.x >= 0;
        }

        private int CountCropTendingTargets()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (IsCropTendingTarget(new Vector2Int(x, y)) && TryFindFarmStationForCrop(new Vector2Int(x, y), out _))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private void UpdateAutomationWires()
        {
            Array.Clear(automationControlledWire, 0, automationControlledWire.Length);
            Array.Clear(automationSignalWire, 0, automationSignalWire.Length);
            Queue<Vector2Int> frontier = new Queue<Vector2Int>();

            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (automationWire[x, y] && TryGetAutomationSourceSignalNearWire(x, y, out bool signalActive))
                    {
                        automationControlledWire[x, y] = true;
                        automationSignalWire[x, y] = signalActive;
                        frontier.Enqueue(new Vector2Int(x, y));
                    }
                }
            }

            while (frontier.Count > 0)
            {
                Vector2Int cell = frontier.Dequeue();
                bool signalActive = automationSignalWire[cell.x, cell.y];
                AddAutomationWireNeighbor(cell.x + 1, cell.y, signalActive, frontier);
                AddAutomationWireNeighbor(cell.x - 1, cell.y, signalActive, frontier);
                AddAutomationWireNeighbor(cell.x, cell.y + 1, signalActive, frontier);
                AddAutomationWireNeighbor(cell.x, cell.y - 1, signalActive, frontier);
            }
        }

        private void AddAutomationWireNeighbor(int x, int y, bool signalActive, Queue<Vector2Int> frontier)
        {
            if (!IsInside(x, y) || !automationWire[x, y])
            {
                return;
            }

            if (automationControlledWire[x, y] && (!signalActive || automationSignalWire[x, y]))
            {
                return;
            }

            automationControlledWire[x, y] = true;
            automationSignalWire[x, y] |= signalActive;
            frontier.Enqueue(new Vector2Int(x, y));
        }

        private bool TryGetAutomationSourceSignalNearWire(int x, int y, out bool signalActive)
        {
            signalActive = false;
            bool foundSource = false;
            foundSource |= TryMergeAutomationSourceSignal(x, y, ref signalActive);
            foundSource |= TryMergeAutomationSourceSignal(x + 1, y, ref signalActive);
            foundSource |= TryMergeAutomationSourceSignal(x - 1, y, ref signalActive);
            foundSource |= TryMergeAutomationSourceSignal(x, y + 1, ref signalActive);
            foundSource |= TryMergeAutomationSourceSignal(x, y - 1, ref signalActive);
            return foundSource;
        }

        private bool TryMergeAutomationSourceSignal(int x, int y, ref bool signalActive)
        {
            if (!TryGetAutomationSourceSignal(x, y, out bool sourceSignal))
            {
                return false;
            }

            signalActive |= sourceSignal;
            return true;
        }

        private bool TryGetAutomationSourceSignal(int x, int y, out bool signalActive)
        {
            signalActive = false;
            if (!IsInside(x, y))
            {
                return false;
            }

            CellKind kind = cells[x, y];
            if (kind == CellKind.SmartBattery)
            {
                signalActive = SmartBatterySignalActive();
                return true;
            }

            if (kind == CellKind.LiquidPipeSensor)
            {
                signalActive = LiquidSensorSignalActive(x, y);
                return true;
            }

            if (kind == CellKind.GasPipeSensor)
            {
                signalActive = GasSensorSignalActive(x, y);
                return true;
            }

            if (kind == CellKind.SpaceScanner)
            {
                signalActive = SpaceScannerSignalActive(x, y);
                return true;
            }

            if (kind == CellKind.SignalSwitch)
            {
                signalActive = automationSwitchState[x, y];
                return true;
            }

            return false;
        }

        private bool LiquidSensorSignalActive(int x, int y)
        {
            return IsInside(x, y) && liquidPipe[x, y] && pipeWater[x, y] >= LiquidSensorThreshold;
        }

        private bool GasSensorSignalActive(int x, int y)
        {
            return IsInside(x, y) &&
                gasPipe[x, y] &&
                (GasPipeTotal(x, y) >= GasSensorPressureThreshold || gasPipeHydrogen[x, y] >= GasSensorHydrogenThreshold);
        }

        private bool IsAutomationWireTouchingSmartBattery(int x, int y)
        {
            return IsSmartBatteryCell(x, y) ||
                IsSmartBatteryCell(x + 1, y) ||
                IsSmartBatteryCell(x - 1, y) ||
                IsSmartBatteryCell(x, y + 1) ||
                IsSmartBatteryCell(x, y - 1);
        }

        private bool IsSmartBatteryCell(int x, int y)
        {
            return IsInside(x, y) && cells[x, y] == CellKind.SmartBattery;
        }

        private bool SmartBatterySignalActive()
        {
            return CountCells(CellKind.SmartBattery) > 0 && power <= maxPower * SmartBatteryLowThreshold;
        }

        private bool AnySignalSwitchLinked()
        {
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] == CellKind.SignalSwitch && HasAutomationWireAccess(new Vector2Int(x, y)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool AnySignalSwitchGreen()
        {
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] == CellKind.SignalSwitch && automationSwitchState[x, y])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool HasLinkedSpaceScanner()
        {
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] == CellKind.SpaceScanner && HasAutomationWireAccess(new Vector2Int(x, y)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool AnyClosedAirlock()
        {
            return CountClosedAirlocks() > 0;
        }

        private bool HasAutomationControl(Vector2Int cell)
        {
            UpdateAutomationWires();
            return HasCachedAutomationControl(cell);
        }

        private bool HasAutomationSignal(Vector2Int cell)
        {
            UpdateAutomationWires();
            return HasCachedAutomationSignal(cell);
        }

        private bool HasCachedAutomationControl(Vector2Int cell)
        {
            return IsAutomationWireCell(cell.x, cell.y, true, false) ||
                IsAutomationWireCell(cell.x + 1, cell.y, true, false) ||
                IsAutomationWireCell(cell.x - 1, cell.y, true, false) ||
                IsAutomationWireCell(cell.x, cell.y + 1, true, false) ||
                IsAutomationWireCell(cell.x, cell.y - 1, true, false);
        }

        private bool HasCachedAutomationSignal(Vector2Int cell)
        {
            return IsAutomationWireCell(cell.x, cell.y, true, true) ||
                IsAutomationWireCell(cell.x + 1, cell.y, true, true) ||
                IsAutomationWireCell(cell.x - 1, cell.y, true, true) ||
                IsAutomationWireCell(cell.x, cell.y + 1, true, true) ||
                IsAutomationWireCell(cell.x, cell.y - 1, true, true);
        }

        private bool IsAutomationWireCell(int x, int y, bool requireControlled, bool requireSignal)
        {
            if (!IsInside(x, y) || !automationWire[x, y])
            {
                return false;
            }

            if (requireControlled && !automationControlledWire[x, y])
            {
                return false;
            }

            return !requireSignal || automationSignalWire[x, y];
        }

        private bool CanGeneratorOperate(Vector2Int cell)
        {
            if (!CanUseEquipment(cell) || !HasWireAccess(cell) || power >= maxPower - 4f)
            {
                return false;
            }

            if (!HasAutomationControl(cell))
            {
                return true;
            }

            return HasAutomationSignal(cell);
        }

        private void AddPoweredWireNeighbor(int x, int y, Queue<Vector2Int> frontier)
        {
            if (!IsInside(x, y) || !powerWire[x, y] || poweredWire[x, y])
            {
                return;
            }

            poweredWire[x, y] = true;
            frontier.Enqueue(new Vector2Int(x, y));
        }

        private bool IsWireTouchingPowerInfrastructure(int x, int y)
        {
            return IsPowerInfrastructureCell(x, y) ||
                IsPowerInfrastructureCell(x + 1, y) ||
                IsPowerInfrastructureCell(x - 1, y) ||
                IsPowerInfrastructureCell(x, y + 1) ||
                IsPowerInfrastructureCell(x, y - 1);
        }

        private bool IsPowerInfrastructureCell(int x, int y)
        {
            if (!IsInside(x, y))
            {
                return false;
            }

            return cells[x, y] == CellKind.ManualGenerator || cells[x, y] == CellKind.CoalGenerator || cells[x, y] == CellKind.HydrogenGenerator || cells[x, y] == CellKind.NaturalGasGenerator || cells[x, y] == CellKind.SteamTurbine || cells[x, y] == CellKind.SolarPanel || cells[x, y] == CellKind.Battery || cells[x, y] == CellKind.SmartBattery;
        }

        private bool HasWireAccess(Vector2Int cell)
        {
            return HasWireNear(cell, false);
        }

        private bool HasAutomationWireAccess(Vector2Int cell)
        {
            return IsAutomationWireRaw(cell.x, cell.y) ||
                IsAutomationWireRaw(cell.x + 1, cell.y) ||
                IsAutomationWireRaw(cell.x - 1, cell.y) ||
                IsAutomationWireRaw(cell.x, cell.y + 1) ||
                IsAutomationWireRaw(cell.x, cell.y - 1);
        }

        private bool IsAutomationWireRaw(int x, int y)
        {
            return IsInside(x, y) && automationWire[x, y];
        }

        private bool HasPoweredCircuit(Vector2Int cell)
        {
            return power > 0.05f && HasWireNear(cell, true);
        }

        private bool CanPoweredMachineRun(Vector2Int cell)
        {
            return CanUseEquipment(cell) &&
                HasPoweredCircuit(cell) &&
                MachineAutomationAllows(cell);
        }

        private bool MachineAutomationAllows(Vector2Int cell)
        {
            return !HasCachedAutomationControl(cell) || HasCachedAutomationSignal(cell);
        }

        private string MachineAutomationStateText(Vector2Int cell)
        {
            UpdateAutomationWires();
            if (!RequiresPower(cells[cell.x, cell.y]))
            {
                return string.Empty;
            }

            if (!HasCachedAutomationControl(cell))
            {
                return "Automation: no signal, defaults enabled.";
            }

            return HasCachedAutomationSignal(cell)
                ? "Automation: green signal, machine enabled."
                : "Automation: red signal, machine disabled.";
        }

        private bool HasWireNear(Vector2Int cell, bool requirePowered)
        {
            return IsWireCell(cell.x, cell.y, requirePowered) ||
                IsWireCell(cell.x + 1, cell.y, requirePowered) ||
                IsWireCell(cell.x - 1, cell.y, requirePowered) ||
                IsWireCell(cell.x, cell.y + 1, requirePowered) ||
                IsWireCell(cell.x, cell.y - 1, requirePowered);
        }

        private bool IsWireCell(int x, int y, bool requirePowered)
        {
            if (!IsInside(x, y) || !powerWire[x, y])
            {
                return false;
            }

            return !requirePowered || poweredWire[x, y];
        }

        private bool RequiresPower(CellKind kind)
        {
            return kind == CellKind.OxygenDiffuser ||
                kind == CellKind.AirDeodorizer ||
                kind == CellKind.ResearchStation ||
                kind == CellKind.MicrobeMusher ||
                kind == CellKind.RockCrusher ||
                kind == CellKind.AtmoSuitDock ||
                kind == CellKind.GasPump ||
                kind == CellKind.Electrolyzer ||
                kind == CellKind.CarbonSkimmer ||
                kind == CellKind.WaterSieve ||
                kind == CellKind.HydrogenFilter ||
                kind == CellKind.SpaceScanner ||
                kind == CellKind.Refrigerator ||
                kind == CellKind.AutoSweeper ||
                kind == CellKind.ConveyorLoader ||
                kind == CellKind.SpaceHeater ||
                kind == CellKind.ThermoRegulator;
        }

        private void RenderTerrain()
        {
            UpdateAutomationWires();
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    terrainTexture.SetPixel(x, y, TerrainColor(x, y, cells[x, y]));
                }
            }

            terrainTexture.Apply(false, false);
            terrainDirty = false;
        }

        private void RenderGas()
        {
            UpdatePoweredWires();
            UpdateAutomationWires();
            UpdatePowerLoad(0f, false);

            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    gasTexture.SetPixel(x, y, OverlayColor(x, y));
                }
            }

            gasTexture.Apply(false, false);
            gasDirty = false;
        }

        private void RenderOverlay()
        {
            UpdateAutomationWires();
            Color clear = new Color(0f, 0f, 0f, 0f);
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    overlayTexture.SetPixel(x, y, clear);
                }
            }

            if (currentOverlayMode == OverlayMode.Power || currentMode == CommandMode.PowerWire)
            {
                UpdatePoweredWires();
                UpdateAutomationWires();
                UpdatePowerLoad(0f, false);
                for (int y = 0; y < WorldHeight; y++)
                {
                    for (int x = 0; x < WorldWidth; x++)
                    {
                        if (powerWire[x, y])
                        {
                            overlayTexture.SetPixel(x, y, overloadedWire[x, y] ? new Color(1f, 0.16f, 0.04f, 0.84f) : poweredWire[x, y] ? new Color(1f, 0.86f, 0.22f, 0.76f) : new Color(0.55f, 0.32f, 0.08f, 0.62f));
                        }
                    }
                }
            }

            if (currentOverlayMode == OverlayMode.Power || currentMode == CommandMode.AutomationWire || currentMode == CommandMode.SignalSwitch)
            {
                UpdateAutomationWires();
                for (int y = 0; y < WorldHeight; y++)
                {
                    for (int x = 0; x < WorldWidth; x++)
                    {
                        if (automationWire[x, y])
                        {
                            overlayTexture.SetPixel(x, y, automationControlledWire[x, y]
                                ? automationSignalWire[x, y] ? new Color(0.28f, 1f, 0.32f, 0.72f) : new Color(1f, 0.18f, 0.10f, 0.66f)
                                : new Color(0.52f, 0.42f, 0.85f, 0.42f));
                        }
                        else if (cells[x, y] == CellKind.SignalSwitch)
                        {
                            overlayTexture.SetPixel(x, y, automationSwitchState[x, y] ? new Color(0.28f, 1f, 0.32f, 0.68f) : new Color(1f, 0.18f, 0.10f, 0.62f));
                        }
                    }
                }
            }

            if (currentOverlayMode == OverlayMode.Plumbing ||
                currentMode == CommandMode.LiquidPipe ||
                currentMode == CommandMode.LiquidPipeSensor ||
                currentMode == CommandMode.LiquidShutoff ||
                currentMode == CommandMode.LiquidReservoir ||
                currentMode == CommandMode.BottleEmptier)
            {
                for (int y = 0; y < WorldHeight; y++)
                {
                    for (int x = 0; x < WorldWidth; x++)
                    {
                        if (cells[x, y] == CellKind.LiquidPipeSensor)
                        {
                            overlayTexture.SetPixel(x, y, LiquidSensorSignalActive(x, y) ? new Color(0.26f, 1f, 0.38f, 0.80f) : new Color(1f, 0.20f, 0.12f, 0.72f));
                        }
                        else if (cells[x, y] == CellKind.LiquidShutoff)
                        {
                            overlayTexture.SetPixel(x, y, IsConduitShutoffOpen(new Vector2Int(x, y)) ? new Color(0.30f, 0.95f, 1f, 0.78f) : new Color(1f, 0.18f, 0.12f, 0.78f));
                        }
                        else if (liquidPipe[x, y])
                        {
                            float fill = Mathf.Clamp01(pipeWater[x, y] / LiquidPipeCapacity);
                            overlayTexture.SetPixel(x, y, new Color(0.08f, 0.58f + fill * 0.25f, 1f, 0.38f + fill * 0.42f));
                        }
                        else if (cells[x, y] == CellKind.LiquidReservoir)
                        {
                            float fill = Mathf.Clamp01(liquidReservoirWater[x, y] / LiquidReservoirCapacity);
                            overlayTexture.SetPixel(x, y, new Color(0.08f, 0.58f + fill * 0.30f, 1f, 0.34f + fill * 0.48f));
                        }
                        else if (cells[x, y] == CellKind.BottleEmptier)
                        {
                            overlayTexture.SetPixel(x, y, CanEmptyBottleAt(new Vector2Int(x, y)) ? new Color(0.10f, 0.92f, 0.86f, 0.72f) : new Color(0.42f, 0.54f, 0.56f, 0.54f));
                        }
                    }
                }
            }

            if (currentOverlayMode == OverlayMode.Ventilation ||
                currentMode == CommandMode.GasPipe ||
                currentMode == CommandMode.GasPipeSensor ||
                currentMode == CommandMode.GasShutoff ||
                currentMode == CommandMode.GasReservoir)
            {
                for (int y = 0; y < WorldHeight; y++)
                {
                    for (int x = 0; x < WorldWidth; x++)
                    {
                        if (cells[x, y] == CellKind.GasPipeSensor)
                        {
                            overlayTexture.SetPixel(x, y, GasSensorSignalActive(x, y) ? new Color(0.28f, 1f, 0.36f, 0.80f) : new Color(1f, 0.20f, 0.12f, 0.72f));
                        }
                        else if (cells[x, y] == CellKind.GasShutoff)
                        {
                            overlayTexture.SetPixel(x, y, IsConduitShutoffOpen(new Vector2Int(x, y)) ? new Color(0.52f, 0.96f, 1f, 0.78f) : new Color(1f, 0.18f, 0.12f, 0.78f));
                        }
                        else if (gasPipe[x, y])
                        {
                            overlayTexture.SetPixel(x, y, GasPipeOverlayColor(x, y));
                        }
                        else if (cells[x, y] == CellKind.GasReservoir)
                        {
                            float fill = Mathf.Clamp01(GasReservoirTotal(x, y) / GasReservoirCapacity);
                            overlayTexture.SetPixel(x, y, new Color(0.45f, 0.92f, 1f, 0.32f + fill * 0.48f));
                        }
                    }
                }
            }

            if (currentOverlayMode == OverlayMode.Logistics ||
                currentMode == CommandMode.ShippingRail ||
                currentMode == CommandMode.ConveyorLoader ||
                currentMode == CommandMode.ConveyorChute)
            {
                for (int y = 0; y < WorldHeight; y++)
                {
                    for (int x = 0; x < WorldWidth; x++)
                    {
                        Color color = LogisticsOverlayColor(x, y);
                        if (color.a > 0f)
                        {
                            overlayTexture.SetPixel(x, y, color);
                        }
                    }
                }
            }

            foreach (Job job in jobs)
            {
                Color color = JobOverlayColor(job);
                overlayTexture.SetPixel(job.Cell.x, job.Cell.y, color);
            }

            if (inspectedCell.HasValue)
            {
                Vector2Int cell = inspectedCell.Value;
                if (IsInside(cell.x, cell.y))
                {
                    overlayTexture.SetPixel(cell.x, cell.y, new Color(1f, 1f, 1f, 0.42f));
                }
            }

            overlayTexture.Apply(false, false);
            overlayDirty = false;
        }

        private Color TerrainColor(int x, int y, CellKind kind)
        {
            float variation = ((x * 31 + y * 17) % 11) / 100f;
            if (IsBrokenEquipment(new Vector2Int(x, y)))
            {
                return new Color(0.72f, 0.12f, 0.10f, 1f);
            }

            if (IsRepairableEquipment(kind) && equipmentCondition[x, y] <= EquipmentAutoRepairThreshold)
            {
                return new Color(0.92f, 0.46f, 0.18f, 1f);
            }

            if (kind == CellKind.Empty && looseResourceKind[x, y] != LooseResourceKind.None && looseResourceAmount[x, y] > 0.05f)
            {
                return LooseResourceColor(looseResourceKind[x, y]);
            }

            switch (kind)
            {
                case CellKind.Empty:
                    return new Color(0.035f, 0.042f + y * 0.0008f, 0.055f + y * 0.001f, 1f);
                case CellKind.Dirt:
                    return new Color(0.38f + variation, 0.25f + variation * 0.5f, 0.14f, 1f);
                case CellKind.Rock:
                    return new Color(0.30f + variation, 0.32f + variation, 0.34f + variation, 1f);
                case CellKind.Sand:
                    return new Color(0.66f + variation, 0.56f + variation, 0.31f, 1f);
                case CellKind.Regolith:
                    return new Color(0.58f + variation * 0.4f, 0.55f + variation * 0.35f, 0.50f + variation * 0.25f, 1f);
                case CellKind.MetalOre:
                    return new Color(0.32f, 0.45f + variation, 0.50f + variation, 1f);
                case CellKind.Algae:
                    return new Color(0.26f, 0.54f + variation, 0.24f, 1f);
                case CellKind.Coal:
                    return new Color(0.10f + variation * 0.25f, 0.105f + variation * 0.18f, 0.095f + variation * 0.12f, 1f);
                case CellKind.Slime:
                    return new Color(0.18f, 0.46f + variation, 0.30f, 1f);
                case CellKind.Ice:
                    return new Color(0.62f, 0.88f + variation * 0.3f, 0.98f, 1f);
                case CellKind.Ladder:
                    return new Color(0.78f, 0.64f, 0.35f, 1f);
                case CellKind.Floor:
                    return new Color(0.48f, 0.50f, 0.52f, 1f);
                case CellKind.OxygenDiffuser:
                    return new Color(0.55f, 0.84f, 0.92f, 1f);
                case CellKind.ManualGenerator:
                    return new Color(0.92f, 0.55f, 0.25f, 1f);
                case CellKind.Battery:
                    return new Color(0.94f, 0.82f, 0.26f, 1f);
                case CellKind.SmartBattery:
                    return SmartBatterySignalActive() ? new Color(0.38f, 0.92f, 0.36f, 1f) : new Color(0.92f, 0.36f, 0.26f, 1f);
                case CellKind.PowerTransformer:
                    return new Color(0.28f, 0.76f, 0.96f, 1f);
                case CellKind.CoalGenerator:
                    return CanCoalGeneratorRun(new Vector2Int(x, y)) ? new Color(0.98f, 0.62f, 0.20f, 1f) : new Color(0.43f, 0.32f, 0.24f, 1f);
                case CellKind.HydrogenGenerator:
                    return CanHydrogenGeneratorRun(new Vector2Int(x, y)) ? new Color(0.82f, 0.42f, 1f, 1f) : new Color(0.38f, 0.25f, 0.48f, 1f);
                case CellKind.NaturalGasGenerator:
                    return CanNaturalGasGeneratorRun(new Vector2Int(x, y)) ? new Color(1f, 0.70f, 0.26f, 1f) : new Color(0.48f, 0.30f, 0.16f, 1f);
                case CellKind.SteamTurbine:
                    return CanSteamTurbineRun(new Vector2Int(x, y)) ? new Color(0.62f, 0.94f, 1f, 1f) : new Color(0.36f, 0.50f, 0.58f, 1f);
                case CellKind.SolarPanel:
                    return CanSolarPanelRun(new Vector2Int(x, y)) ? new Color(1f, 0.92f, 0.22f, 1f) : IsSolarPanelSkyExposed(new Vector2Int(x, y)) ? new Color(0.42f, 0.54f, 0.64f, 1f) : new Color(0.22f, 0.26f, 0.30f, 1f);
                case CellKind.BunkerDoor:
                    return IsBunkerDoorClosed(new Vector2Int(x, y)) ? new Color(0.24f, 0.32f, 0.36f, 1f) : new Color(0.58f, 0.68f, 0.72f, 1f);
                case CellKind.SpaceScanner:
                    return SpaceScannerSignalActive(x, y) ? new Color(0.48f, 1f, 0.82f, 1f) : IsSpaceScannerSkyExposed(new Vector2Int(x, y)) ? new Color(0.34f, 0.64f, 0.86f, 1f) : new Color(0.22f, 0.28f, 0.34f, 1f);
                case CellKind.HydrogenFilter:
                    return TryFindAdjacentMixedHydrogenPipe(new Vector2Int(x, y), out _) ? new Color(0.88f, 0.50f, 1f, 1f) : new Color(0.46f, 0.34f, 0.56f, 1f);
                case CellKind.RockCrusher:
                    return new Color(0.64f, 0.58f, 0.50f, 1f);
                case CellKind.AtmoSuitDock:
                    return suitOxygen > 0.25f ? new Color(0.42f, 0.86f, 0.98f, 1f) : new Color(0.32f, 0.42f, 0.48f, 1f);
                case CellKind.AtmoSuitCheckpoint:
                    return HasAdjacentSuitDock(new Vector2Int(x, y)) ? new Color(0.56f, 0.98f, 0.92f, 1f) : new Color(0.30f, 0.54f, 0.56f, 1f);
                case CellKind.InsulatedTile:
                    return new Color(0.50f, 0.58f, 0.62f, 1f);
                case CellKind.PrintingPod:
                    return printingPodProgress >= 0.98f ? new Color(0.68f, 1f, 0.82f, 1f) : new Color(0.38f, 0.84f, 0.78f, 1f);
                case CellKind.Bed:
                    return new Color(0.45f, 0.65f, 0.92f, 1f);
                case CellKind.Planter:
                    if (cropStress[x, y] >= CropWiltThresholdSeconds)
                    {
                        return new Color(0.58f, 0.34f, 0.14f, 1f);
                    }

                    if (cropStress[x, y] >= CropStressThresholdSeconds)
                    {
                        return new Color(0.78f, 0.72f, 0.26f, 1f);
                    }

                    return plantGrowth[x, y] >= 1f ? new Color(0.56f, 0.92f, 0.38f, 1f) : new Color(0.31f, 0.62f, 0.26f, 1f);
                case CellKind.FarmStation:
                    return CountCropTendingTargets() > 0 ? new Color(0.82f, 0.96f, 0.42f, 1f) : new Color(0.48f, 0.64f, 0.26f, 1f);
                case CellKind.Water:
                    return new Color(0.06f, 0.31f, 0.86f + Mathf.Clamp01(waterMass[x, y] / 140f) * 0.08f, 1f);
                case CellKind.WaterPump:
                    return new Color(0.18f, 0.62f, 0.92f, 1f);
                case CellKind.BottleEmptier:
                    return CanEmptyBottleAt(new Vector2Int(x, y)) ? new Color(0.18f, 0.82f, 0.78f, 1f) : new Color(0.28f, 0.48f, 0.52f, 1f);
                case CellKind.ResearchStation:
                    return new Color(0.72f, 0.42f, 0.95f, 1f);
                case CellKind.MicrobeMusher:
                    return new Color(0.78f, 0.72f, 0.32f, 1f);
                case CellKind.AirDeodorizer:
                    return new Color(0.72f, 0.90f, 0.78f, 1f);
                case CellKind.MedicalCot:
                    return new Color(0.95f, 0.78f, 0.84f, 1f);
                case CellKind.SpaceHeater:
                    return new Color(0.95f, 0.40f, 0.24f, 1f);
                case CellKind.ThermoRegulator:
                    return new Color(0.34f, 0.72f, 0.96f, 1f);
                case CellKind.Outhouse:
                    return new Color(0.74f, 0.86f, 0.72f, 1f);
                case CellKind.WashBasin:
                    return water >= WashBasinWaterUse ? new Color(0.42f, 0.86f, 0.96f, 1f) : new Color(0.38f, 0.52f, 0.58f, 1f);
                case CellKind.MassageTable:
                    return new Color(0.70f, 0.56f, 0.90f, 1f);
                case CellKind.ManualAirlock:
                    return airlockOpen[x, y] ? new Color(0.64f, 0.78f, 0.82f, 1f) : new Color(0.34f, 0.46f, 0.54f, 1f);
                case CellKind.Refrigerator:
                    return IsPoweredRefrigeratorAt(x, y) ? new Color(0.38f, 0.82f, 0.96f, 1f) : new Color(0.46f, 0.55f, 0.60f, 1f);
                case CellKind.StorageBin:
                    return new Color(0.62f, 0.46f, 0.28f, 1f);
                case CellKind.AutoSweeper:
                    return TryFindAutoSweeperTarget(new Vector2Int(x, y), out _) ? new Color(0.96f, 0.78f, 0.24f, 1f) : new Color(0.48f, 0.46f, 0.42f, 1f);
                case CellKind.ConveyorLoader:
                    return CanPoweredMachineRun(new Vector2Int(x, y)) ? new Color(0.98f, 0.62f, 0.20f, 1f) : new Color(0.46f, 0.34f, 0.22f, 1f);
                case CellKind.ConveyorChute:
                    return new Color(0.84f, 0.54f, 0.18f, 1f);
                case CellKind.SignalSwitch:
                    return automationSwitchState[x, y] ? new Color(0.34f, 0.95f, 0.38f, 1f) : new Color(0.78f, 0.26f, 0.22f, 1f);
                case CellKind.LiquidPipeSensor:
                    return LiquidSensorSignalActive(x, y) ? new Color(0.42f, 0.96f, 0.46f, 1f) : new Color(0.74f, 0.32f, 0.28f, 1f);
                case CellKind.LiquidShutoff:
                    return IsConduitShutoffOpen(new Vector2Int(x, y)) ? new Color(0.24f, 0.76f, 0.96f, 1f) : new Color(0.78f, 0.22f, 0.18f, 1f);
                case CellKind.LiquidReservoir:
                    return liquidReservoirWater[x, y] > 0.2f ? new Color(0.16f, 0.58f, 0.98f, 1f) : new Color(0.24f, 0.38f, 0.58f, 1f);
                case CellKind.LiquidVent:
                    return new Color(0.22f, 0.64f, 0.92f, 1f);
                case CellKind.GasPump:
                    return new Color(0.58f, 0.86f, 0.96f, 1f);
                case CellKind.GasPipeSensor:
                    return GasSensorSignalActive(x, y) ? new Color(0.42f, 0.96f, 0.46f, 1f) : new Color(0.74f, 0.32f, 0.28f, 1f);
                case CellKind.GasShutoff:
                    return IsConduitShutoffOpen(new Vector2Int(x, y)) ? new Color(0.42f, 0.84f, 0.96f, 1f) : new Color(0.78f, 0.22f, 0.18f, 1f);
                case CellKind.SteamVent:
                    return NaturalVentActive(new Vector2Int(x, y)) ? new Color(0.98f, 0.56f, 0.30f, 1f) : new Color(0.48f, 0.72f, 0.78f, 1f);
                case CellKind.HydrogenVent:
                    return NaturalVentActive(new Vector2Int(x, y)) ? new Color(0.92f, 0.42f, 1f, 1f) : new Color(0.48f, 0.32f, 0.58f, 1f);
                case CellKind.NaturalGasVent:
                    return NaturalVentActive(new Vector2Int(x, y)) ? new Color(1f, 0.74f, 0.24f, 1f) : new Color(0.52f, 0.36f, 0.18f, 1f);
                case CellKind.GasReservoir:
                    return GasReservoirTotal(x, y) > 0.05f ? new Color(0.72f, 0.92f, 0.98f, 1f) : new Color(0.42f, 0.56f, 0.62f, 1f);
                case CellKind.GasVent:
                    return new Color(0.68f, 0.90f, 0.96f, 1f);
                case CellKind.RanchingStation:
                    return FindGroomableHatch(new Vector2Int(x, y)) != null ? new Color(0.94f, 0.72f, 0.38f, 1f) : new Color(0.58f, 0.42f, 0.24f, 1f);
                case CellKind.Electrolyzer:
                    return new Color(0.52f, 0.90f, 0.98f, 1f);
                case CellKind.CarbonSkimmer:
                    return new Color(0.42f, 0.72f, 0.88f, 1f);
                case CellKind.WaterSieve:
                    return new Color(0.36f, 0.66f, 0.72f, 1f);
                case CellKind.MessTable:
                    return new Color(0.82f, 0.64f, 0.38f, 1f);
                case CellKind.DecorPlant:
                    return new Color(0.82f, 0.46f, 0.90f, 1f);
                case CellKind.Compost:
                    return new Color(0.36f, 0.50f, 0.22f, 1f);
                default:
                    return Color.magenta;
            }
        }

        private Color LooseResourceColor(LooseResourceKind kind)
        {
            switch (kind)
            {
                case LooseResourceKind.Dirt:
                    return new Color(0.55f, 0.37f, 0.18f, 1f);
                case LooseResourceKind.Metal:
                    return new Color(0.42f, 0.58f, 0.66f, 1f);
                case LooseResourceKind.Algae:
                    return new Color(0.30f, 0.72f, 0.26f, 1f);
                case LooseResourceKind.Coal:
                    return new Color(0.12f, 0.12f, 0.11f, 1f);
                case LooseResourceKind.RefinedMetal:
                    return new Color(0.78f, 0.74f, 0.66f, 1f);
                case LooseResourceKind.PollutedDirt:
                    return new Color(0.36f, 0.50f, 0.22f, 1f);
                default:
                    return new Color(0.66f, 0.56f, 0.36f, 1f);
            }
        }

        private Color GasColor(int x, int y)
        {
            if (!IsPassable(x, y))
            {
                return new Color(0f, 0f, 0f, 0f);
            }

            float o2 = oxygen[x, y];
            float co2 = carbonDioxide[x, y];
            float po2 = pollutedOxygen[x, y];
            float h2 = hydrogen[x, y];
            float st = steam[x, y];
            float cl = chlorine[x, y];
            float ng = naturalGas[x, y];
            if (st > 0.05f && st > o2 * 0.72f && st > co2 * 0.72f && st > po2 * 0.72f && st > h2 * 0.72f && st > cl * 0.72f && st > ng * 0.72f)
            {
                float alpha = Mathf.Clamp01(st * 0.32f + 0.12f);
                return new Color(0.92f, 0.96f, 1f, alpha);
            }

            if (ng > 0.05f && ng > o2 * 0.72f && ng > co2 * 0.72f && ng > po2 * 0.72f && ng > h2 * 0.72f && ng > st * 0.72f && ng > cl * 0.72f)
            {
                return new Color(1f, 0.58f, 0.16f, Mathf.Clamp01(ng * 0.34f + 0.10f));
            }

            if (cl > 0.05f && cl > o2 * 0.72f && cl > co2 * 0.72f && cl > po2 * 0.72f && cl > h2 * 0.72f && cl > st * 0.72f && cl > ng * 0.72f)
            {
                return new Color(0.72f, 0.95f, 0.16f, Mathf.Clamp01(cl * 0.34f + 0.10f));
            }

            if (h2 > o2 * 0.72f && h2 > co2 * 0.72f && h2 > po2 * 0.72f && h2 > st * 0.72f && h2 > cl * 0.72f && h2 > ng * 0.72f)
            {
                return new Color(0.82f, 0.38f, 1f, Mathf.Clamp01(h2 * 0.30f));
            }

            if (po2 > o2 * 0.72f && po2 > co2 * 0.72f && po2 > st * 0.72f && po2 > cl * 0.72f && po2 > ng * 0.72f)
            {
                float alpha = Mathf.Clamp01(po2 * 0.34f + germs[x, y] * 0.18f);
                return new Color(0.28f, 0.92f, 0.36f, alpha);
            }

            if (o2 < 0.05f && co2 < 0.05f && po2 < 0.05f && h2 < 0.05f && st < 0.05f && cl < 0.05f && ng < 0.05f)
            {
                return new Color(0f, 0f, 0f, 0.45f);
            }

            if (co2 > o2 * 0.85f)
            {
                float alpha = Mathf.Clamp01(co2 * 0.35f);
                return new Color(0.85f, 0.34f, 0.16f, alpha);
            }

            return new Color(0.2f, 0.72f, 1f, Mathf.Clamp01(o2 * 0.22f));
        }

        private Color OverlayColor(int x, int y)
        {
            switch (currentOverlayMode)
            {
                case OverlayMode.Temperature:
                    return TemperatureOverlayColor(x, y);
                case OverlayMode.Power:
                    return PowerOverlayColor(x, y);
                case OverlayMode.Germs:
                    return GermOverlayColor(x, y);
                case OverlayMode.Plumbing:
                    return PlumbingOverlayColor(x, y);
                case OverlayMode.Ventilation:
                    return VentilationOverlayColor(x, y);
                case OverlayMode.Logistics:
                    return LogisticsOverlayColor(x, y);
                case OverlayMode.Decor:
                    return DecorOverlayColor(x, y);
                case OverlayMode.Rooms:
                    return RoomOverlayColor(x, y);
                default:
                    return GasColor(x, y);
            }
        }

        private Color RoomOverlayColor(int x, int y)
        {
            if (cells[x, y] == CellKind.ManualAirlock || cells[x, y] == CellKind.InsulatedTile)
            {
                return new Color(0.82f, 0.90f, 0.94f, 0.50f);
            }

            RoomInfo room = RoomAt(x, y);
            if (room == null)
            {
                return new Color(0f, 0f, 0f, 0f);
            }

            Color color = RoomKindColor(room.Kind);
            if (room.Kind == RoomKind.OpenArea)
            {
                color.a = 0.08f;
                return color;
            }

            color.a = room.Kind == RoomKind.MixedRoom ? 0.34f : 0.46f;
            return color;
        }

        private Color RoomKindColor(RoomKind kind)
        {
            switch (kind)
            {
                case RoomKind.Barracks:
                    return new Color(0.28f, 0.60f, 1f, 1f);
                case RoomKind.MessHall:
                    return new Color(1f, 0.82f, 0.28f, 1f);
                case RoomKind.Washroom:
                    return new Color(0.28f, 0.90f, 0.86f, 1f);
                case RoomKind.Clinic:
                    return new Color(1f, 0.52f, 0.66f, 1f);
                case RoomKind.RecreationRoom:
                    return new Color(0.72f, 0.50f, 1f, 1f);
                case RoomKind.MachineRoom:
                    return new Color(1f, 0.48f, 0.20f, 1f);
                case RoomKind.StorageRoom:
                    return new Color(0.50f, 0.82f, 0.52f, 1f);
                case RoomKind.MixedRoom:
                    return new Color(0.78f, 0.78f, 0.74f, 1f);
                case RoomKind.BasicRoom:
                    return new Color(0.46f, 0.76f, 0.48f, 1f);
                default:
                    return new Color(0.20f, 0.24f, 0.28f, 1f);
            }
        }

        private Color DecorOverlayColor(int x, int y)
        {
            float decor = DecorScoreAt(x, y);
            if (decor <= 0.01f)
            {
                return new Color(0f, 0f, 0f, 0f);
            }

            if (cells[x, y] == CellKind.DecorPlant)
            {
                return new Color(1f, 0.54f, 0.96f, 0.82f);
            }

            return new Color(0.92f, 0.44f + decor * 0.22f, 1f, 0.12f + decor * 0.54f);
        }

        private Color PlumbingOverlayColor(int x, int y)
        {
            if (cells[x, y] == CellKind.LiquidPipeSensor)
            {
                return LiquidSensorSignalActive(x, y) ? new Color(0.26f, 1f, 0.38f, 0.70f) : new Color(1f, 0.20f, 0.12f, 0.62f);
            }

            if (cells[x, y] == CellKind.LiquidShutoff)
            {
                return IsConduitShutoffOpen(new Vector2Int(x, y)) ? new Color(0.30f, 0.95f, 1f, 0.70f) : new Color(1f, 0.18f, 0.12f, 0.68f);
            }

            if (liquidPipe[x, y])
            {
                float fill = Mathf.Clamp01(pipeWater[x, y] / LiquidPipeCapacity);
                return new Color(0.08f, 0.58f + fill * 0.25f, 1f, 0.30f + fill * 0.40f);
            }

            if (cells[x, y] == CellKind.LiquidReservoir)
            {
                float fill = Mathf.Clamp01(liquidReservoirWater[x, y] / LiquidReservoirCapacity);
                return new Color(0.10f, 0.62f + fill * 0.24f, 1f, 0.26f + fill * 0.48f);
            }

            if (cells[x, y] == CellKind.LiquidVent || cells[x, y] == CellKind.WaterPump || cells[x, y] == CellKind.WaterSieve)
            {
                return new Color(0.12f, 0.72f, 1f, 0.36f);
            }

            if (cells[x, y] == CellKind.SteamVent)
            {
                return NaturalVentActive(new Vector2Int(x, y)) ? new Color(1f, 0.54f, 0.18f, 0.72f) : new Color(0.34f, 0.78f, 1f, 0.34f);
            }

            return new Color(0f, 0f, 0f, 0f);
        }

        private Color VentilationOverlayColor(int x, int y)
        {
            if (cells[x, y] == CellKind.GasPipeSensor)
            {
                return GasSensorSignalActive(x, y) ? new Color(0.28f, 1f, 0.36f, 0.70f) : new Color(1f, 0.20f, 0.12f, 0.62f);
            }

            if (cells[x, y] == CellKind.GasShutoff)
            {
                return IsConduitShutoffOpen(new Vector2Int(x, y)) ? new Color(0.52f, 0.96f, 1f, 0.70f) : new Color(1f, 0.18f, 0.12f, 0.68f);
            }

            if (gasPipe[x, y])
            {
                return GasPipeOverlayColor(x, y);
            }

            if (cells[x, y] == CellKind.GasReservoir)
            {
                float fill = Mathf.Clamp01(GasReservoirTotal(x, y) / GasReservoirCapacity);
                return new Color(0.45f, 0.92f, 1f, 0.28f + fill * 0.48f);
            }

            if (cells[x, y] == CellKind.GasPump || cells[x, y] == CellKind.GasVent)
            {
                return new Color(0.38f, 0.90f, 1f, 0.42f);
            }

            if (cells[x, y] == CellKind.HydrogenVent)
            {
                return NaturalVentActive(new Vector2Int(x, y)) ? new Color(0.90f, 0.30f, 1f, 0.76f) : new Color(0.52f, 0.34f, 0.72f, 0.36f);
            }

            if (cells[x, y] == CellKind.NaturalGasVent)
            {
                return NaturalVentActive(new Vector2Int(x, y)) ? new Color(1f, 0.62f, 0.16f, 0.76f) : new Color(0.62f, 0.42f, 0.20f, 0.36f);
            }

            return GasColor(x, y);
        }

        private Color LogisticsOverlayColor(int x, int y)
        {
            if (shippingRail[x, y])
            {
                float fill = Mathf.Clamp01(shippingRailAmount[x, y] / ShippingRailCapacity);
                return new Color(1f, 0.56f + fill * 0.20f, 0.12f, 0.34f + fill * 0.48f);
            }

            if (cells[x, y] == CellKind.ConveyorLoader)
            {
                return CanPoweredMachineRun(new Vector2Int(x, y)) ? new Color(1f, 0.72f, 0.20f, 0.66f) : new Color(1f, 0.22f, 0.12f, 0.60f);
            }

            if (cells[x, y] == CellKind.ConveyorChute)
            {
                return new Color(0.95f, 0.48f, 0.12f, 0.62f);
            }

            if (cells[x, y] == CellKind.AutoSweeper)
            {
                return new Color(0.95f, 0.82f, 0.20f, 0.42f);
            }

            return new Color(0f, 0f, 0f, 0f);
        }

        private Color GasPipeOverlayColor(int x, int y)
        {
            float total = GasPipeTotal(x, y);
            float fill = Mathf.Clamp01(total / GasPipeCapacity);
            if (total <= 0.001f)
            {
                return new Color(0.48f, 0.72f, 0.86f, 0.38f);
            }

            if (gasPipeNaturalGas[x, y] >= gasPipeOxygen[x, y] && gasPipeNaturalGas[x, y] >= gasPipeCarbonDioxide[x, y] && gasPipeNaturalGas[x, y] >= gasPipePollutedOxygen[x, y] && gasPipeNaturalGas[x, y] >= gasPipeHydrogen[x, y] && gasPipeNaturalGas[x, y] >= gasPipeChlorine[x, y])
            {
                return new Color(1f, 0.58f, 0.14f, 0.42f + fill * 0.42f);
            }

            if (gasPipeChlorine[x, y] >= gasPipeOxygen[x, y] && gasPipeChlorine[x, y] >= gasPipeCarbonDioxide[x, y] && gasPipeChlorine[x, y] >= gasPipePollutedOxygen[x, y] && gasPipeChlorine[x, y] >= gasPipeHydrogen[x, y] && gasPipeChlorine[x, y] >= gasPipeNaturalGas[x, y])
            {
                return new Color(0.76f, 1f, 0.18f, 0.42f + fill * 0.42f);
            }

            if (gasPipeHydrogen[x, y] >= gasPipeOxygen[x, y] && gasPipeHydrogen[x, y] >= gasPipeCarbonDioxide[x, y] && gasPipeHydrogen[x, y] >= gasPipePollutedOxygen[x, y] && gasPipeHydrogen[x, y] >= gasPipeNaturalGas[x, y])
            {
                return new Color(0.86f, 0.36f, 1f, 0.42f + fill * 0.42f);
            }

            if (gasPipePollutedOxygen[x, y] >= gasPipeOxygen[x, y] && gasPipePollutedOxygen[x, y] >= gasPipeCarbonDioxide[x, y])
            {
                return new Color(0.28f, 1f, 0.32f, 0.42f + fill * 0.42f);
            }

            if (gasPipeCarbonDioxide[x, y] > gasPipeOxygen[x, y] * 0.9f)
            {
                return new Color(0.95f, 0.42f, 0.18f, 0.40f + fill * 0.40f);
            }

            return new Color(0.20f, 0.84f, 1f, 0.40f + fill * 0.42f);
        }

        private Color TemperatureOverlayColor(int x, int y)
        {
            float temp = temperature[x, y];
            if (temp > 34f)
            {
                return new Color(1f, 0.16f, 0.04f, Mathf.Clamp01((temp - 28f) * 0.018f + 0.16f));
            }

            if (temp < 8f)
            {
                return new Color(0.18f, 0.58f, 1f, Mathf.Clamp01((12f - temp) * 0.025f + 0.16f));
            }

            return new Color(0.2f, 1f, 0.55f, 0.08f);
        }

        private Color PowerOverlayColor(int x, int y)
        {
            if (powerWire[x, y])
            {
                if (overloadedWire[x, y])
                {
                    return new Color(1f, 0.14f, 0.04f, 0.84f);
                }

                return poweredWire[x, y] ? new Color(1f, 0.86f, 0.18f, 0.78f) : new Color(0.52f, 0.30f, 0.08f, 0.58f);
            }

            CellKind kind = cells[x, y];
            if (kind == CellKind.PowerTransformer)
            {
                return new Color(0.30f, 0.88f, 1f, 0.62f);
            }

            if (kind == CellKind.ManualGenerator || kind == CellKind.CoalGenerator || kind == CellKind.HydrogenGenerator || kind == CellKind.NaturalGasGenerator || kind == CellKind.SteamTurbine || kind == CellKind.SolarPanel || kind == CellKind.Battery || kind == CellKind.SmartBattery)
            {
                return new Color(1f, 0.72f, 0.18f, 0.5f);
            }

            if (RequiresPower(kind))
            {
                return CanPoweredMachineRun(new Vector2Int(x, y))
                    ? new Color(0.35f, 1f, 0.44f, 0.52f)
                    : new Color(1f, 0.12f, 0.08f, 0.62f);
            }

            return new Color(0f, 0f, 0f, 0f);
        }

        private Color GermOverlayColor(int x, int y)
        {
            if (!IsPassable(x, y))
            {
                return new Color(0f, 0f, 0f, 0f);
            }

            float germLevel = germs[x, y];
            float po2 = pollutedOxygen[x, y];
            if (germLevel <= 0.01f && po2 <= 0.02f)
            {
                return new Color(0f, 0f, 0f, 0f);
            }

            float alpha = Mathf.Clamp01(germLevel * 0.75f + po2 * 0.2f);
            return new Color(0.2f + germLevel * 0.55f, 1f, 0.18f, Mathf.Max(0.16f, alpha));
        }

        private Color JobOverlayColor(Job job)
        {
            float alpha = job.AssignedWorker == null ? 0.62f : 0.85f;
            switch (job.Type)
            {
                case JobType.Dig:
                    return new Color(1f, 0.86f, 0.22f, alpha);
                case JobType.Build:
                    return new Color(0.3f, 1f, 0.55f, alpha);
                case JobType.BuildWire:
                    return new Color(1f, 0.82f, 0.18f, alpha);
                case JobType.BuildPipe:
                    return new Color(0.18f, 0.78f, 1f, alpha);
                case JobType.BuildGasPipe:
                    return new Color(0.44f, 0.92f, 1f, alpha);
                case JobType.BuildShippingRail:
                    return new Color(1f, 0.62f, 0.18f, alpha);
                case JobType.Deconstruct:
                    return new Color(1f, 0.22f, 0.18f, alpha);
                case JobType.Mop:
                    return new Color(0.1f, 0.86f, 1f, alpha);
                case JobType.Repair:
                    return new Color(1f, 0.64f, 0.18f, alpha);
                case JobType.Rescue:
                    return new Color(1f, 0.32f, 0.56f, alpha);
                case JobType.Sweep:
                    return new Color(0.82f, 0.72f, 0.38f, alpha);
                case JobType.OperateGenerator:
                    return new Color(1f, 0.48f, 0.18f, alpha);
                case JobType.Harvest:
                    return new Color(0.45f, 1f, 0.2f, alpha);
                case JobType.PumpWater:
                    return new Color(0.18f, 0.72f, 1f, alpha);
                case JobType.EmptyBottle:
                    return new Color(0.10f, 0.88f, 0.86f, alpha);
                case JobType.Research:
                    return new Color(0.74f, 0.38f, 1f, alpha);
                case JobType.Cook:
                    return new Color(1f, 0.76f, 0.22f, alpha);
                case JobType.RefineMetal:
                    return new Color(0.78f, 0.74f, 0.66f, alpha);
                case JobType.Sleep:
                    return new Color(0.45f, 0.58f, 1f, alpha);
                case JobType.Treat:
                    return new Color(1f, 0.38f, 0.58f, alpha);
                case JobType.UseToilet:
                    return new Color(0.62f, 1f, 0.78f, alpha);
                case JobType.Eat:
                    return new Color(1f, 0.78f, 0.32f, alpha);
                case JobType.Relax:
                    return new Color(0.86f, 0.58f, 1f, alpha);
                default:
                    return Color.white;
            }
        }

        private bool IsRestTime()
        {
            float normalized = cycleTimer / CycleLengthSeconds;
            return sleepStartCycleTime <= sleepEndCycleTime
                ? normalized >= sleepStartCycleTime && normalized <= sleepEndCycleTime
                : normalized >= sleepStartCycleTime || normalized <= sleepEndCycleTime;
        }

        private string ScheduleLabel()
        {
            if (IsRestTime())
            {
                return "Sleep " + SleepWindowLabel();
            }

            float normalized = cycleTimer / CycleLengthSeconds;
            return normalized < 0.18f ? "Breakfast " + SleepWindowLabel() : normalized > 0.66f ? "Downtime " + SleepWindowLabel() : "Work " + SleepWindowLabel();
        }

        private string SleepWindowLabel()
        {
            return CycleClockLabel(sleepStartCycleTime) + "-" + CycleClockLabel(sleepEndCycleTime);
        }

        private string CycleClockLabel(float normalized)
        {
            normalized = NormalizeCycleTime(normalized);
            int minutes = Mathf.RoundToInt(normalized * 24f * 60f) % (24 * 60);
            int hour = minutes / 60;
            int minute = minutes % 60;
            return hour.ToString("00") + ":" + minute.ToString("00");
        }

        private bool IsSleeping(Worker worker)
        {
            return worker.AssignedJob != null && worker.AssignedJob.Type == JobType.Sleep && worker.PathIndex >= worker.Path.Count;
        }

        private bool ShouldSleep(Worker worker)
        {
            return worker.Health > 0f && (worker.Fatigue >= 88f || worker.Stress >= 94f || (IsRestTime() && worker.Fatigue >= 35f));
        }

        private bool AnyWorkerNeedsSleep()
        {
            foreach (Worker worker in workers)
            {
                if (ShouldSleep(worker))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsBedReserved(Vector2Int bedCell)
        {
            foreach (Job job in jobs)
            {
                if (job.Type == JobType.Sleep && job.Cell == bedCell)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsMedicalCotReserved(Vector2Int cotCell)
        {
            foreach (Job job in jobs)
            {
                if ((job.Type == JobType.Treat && job.Cell == cotCell) ||
                    (job.Type == JobType.Rescue && job.AssignedWorker != null))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsOuthouseReserved(Vector2Int toiletCell)
        {
            foreach (Job job in jobs)
            {
                if (job.Type == JobType.UseToilet && job.Cell == toiletCell)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsWashBasinReserved(Vector2Int basinCell)
        {
            foreach (Job job in jobs)
            {
                if (job.Type == JobType.WashHands && job.Cell == basinCell)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsMessTableReserved(Vector2Int tableCell)
        {
            foreach (Job job in jobs)
            {
                if (job.Type == JobType.Eat && job.Cell == tableCell)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsMassageTableReserved(Vector2Int tableCell)
        {
            foreach (Job job in jobs)
            {
                if (job.Type == JobType.Relax && job.Cell == tableCell)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasTreatmentJobFor(string workerName)
        {
            if (string.IsNullOrEmpty(workerName))
            {
                return false;
            }

            foreach (Job job in jobs)
            {
                if (job.Type == JobType.Treat && job.TargetWorkerName == workerName)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasRescueJobFor(string workerName)
        {
            if (string.IsNullOrEmpty(workerName))
            {
                return false;
            }

            foreach (Job job in jobs)
            {
                if (job.Type == JobType.Rescue && job.TargetWorkerName == workerName)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasToiletJobFor(string workerName)
        {
            if (string.IsNullOrEmpty(workerName))
            {
                return false;
            }

            foreach (Job job in jobs)
            {
                if (job.Type == JobType.UseToilet && job.TargetWorkerName == workerName)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasWashHandsJobFor(string workerName)
        {
            if (string.IsNullOrEmpty(workerName))
            {
                return false;
            }

            foreach (Job job in jobs)
            {
                if (job.Type == JobType.WashHands && job.TargetWorkerName == workerName)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasEatJobFor(string workerName)
        {
            if (string.IsNullOrEmpty(workerName))
            {
                return false;
            }

            foreach (Job job in jobs)
            {
                if (job.Type == JobType.Eat && job.TargetWorkerName == workerName)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasRelaxJobFor(string workerName)
        {
            if (string.IsNullOrEmpty(workerName))
            {
                return false;
            }

            foreach (Job job in jobs)
            {
                if (job.Type == JobType.Relax && job.TargetWorkerName == workerName)
                {
                    return true;
                }
            }

            return false;
        }

        private Worker FindWorkerByName(string workerName)
        {
            if (string.IsNullOrEmpty(workerName))
            {
                return null;
            }

            foreach (Worker worker in workers)
            {
                if (worker.Name == workerName)
                {
                    return worker;
                }
            }

            return null;
        }

        private bool AnyWorkerNeedsTreatment()
        {
            foreach (Worker worker in workers)
            {
                if (NeedsTreatment(worker))
                {
                    return true;
                }
            }

            return false;
        }

        private bool AnyWorkerNeedsRescue()
        {
            foreach (Worker worker in workers)
            {
                if (NeedsRescue(worker))
                {
                    return true;
                }
            }

            return false;
        }

        private int CountIncapacitatedWorkers()
        {
            int count = 0;
            foreach (Worker worker in workers)
            {
                if (NeedsRescue(worker))
                {
                    count++;
                }
            }

            return count;
        }

        private bool AnyWorkerNeedsToilet()
        {
            foreach (Worker worker in workers)
            {
                if (NeedsToilet(worker))
                {
                    return true;
                }
            }

            return false;
        }

        private bool AnyWorkerNeedsRelaxation()
        {
            foreach (Worker worker in workers)
            {
                if (NeedsRelaxation(worker))
                {
                    return true;
                }
            }

            return false;
        }

        private bool NeedsTreatment(Worker worker)
        {
            return worker != null &&
                worker.Health > 0f &&
                (worker.Sickness >= 35f || worker.GermExposure >= 70f || worker.Health <= 72f);
        }

        private bool NeedsRescue(Worker worker)
        {
            return worker != null && worker.Health <= 0f;
        }

        private bool NeedsToilet(Worker worker)
        {
            return worker != null && worker.Health > 0f && worker.Bladder >= 68f;
        }

        private bool NeedsHandWash(Worker worker)
        {
            return worker != null &&
                worker.Health > 0f &&
                worker.GermExposure >= 8f &&
                CountCells(CellKind.WashBasin) > 0 &&
                water > 0.05f;
        }

        private bool NeedsFood(Worker worker)
        {
            return worker != null && worker.Health > 0f && worker.Calories < 1100f && food >= 700f;
        }

        private bool NeedsRelaxation(Worker worker)
        {
            return worker != null && worker.Health > 0f && worker.StressBreakSeconds <= 0f && worker.Stress >= 62f;
        }

        private float TreatmentWorkRequired(Worker worker)
        {
            float severity = Mathf.Max(
                Mathf.Clamp01(worker.Sickness / 100f),
                Mathf.Clamp01((100f - worker.Health) / 100f));
            return Mathf.Lerp(3.5f, 8f, severity);
        }

        private float RescueWorkRequired(Worker worker)
        {
            float urgency = Mathf.Clamp01((worker == null ? 0f : worker.IncapacitatedSeconds) / 45f);
            return Mathf.Lerp(2.6f, 4.8f, urgency);
        }

        private int RescuePriority(Worker worker)
        {
            return worker != null && worker.IncapacitatedSeconds > 30f ? 10 : 9;
        }

        private int TreatmentPriority(Worker worker)
        {
            if (worker.Health <= 45f || worker.Sickness >= 70f)
            {
                return 10;
            }

            if (worker.Health <= 62f || worker.Sickness >= 50f || worker.GermExposure >= 85f)
            {
                return 9;
            }

            return 7;
        }

        private float ToiletWorkRequired(Worker worker)
        {
            return Mathf.Lerp(2.4f, 4.2f, Mathf.Clamp01(worker.Bladder / 100f));
        }

        private int ToiletPriority(Worker worker)
        {
            if (worker.Bladder >= 94f)
            {
                return 10;
            }

            if (worker.Bladder >= 82f)
            {
                return 9;
            }

            return 7;
        }

        private float WashHandsWorkRequired(Worker worker)
        {
            return Mathf.Lerp(1.4f, 3.0f, Mathf.Clamp01((worker == null ? 0f : worker.GermExposure) / 100f));
        }

        private int WashHandsPriority(Worker worker)
        {
            if (worker != null && worker.GermExposure >= 70f)
            {
                return 9;
            }

            if (worker != null && worker.GermExposure >= 35f)
            {
                return 8;
            }

            return 7;
        }

        private float EatWorkRequired(Worker worker)
        {
            return Mathf.Lerp(1.4f, 3.4f, Mathf.Clamp01((1400f - worker.Calories) / 1400f));
        }

        private int EatPriority(Worker worker)
        {
            if (worker.Calories <= 350f)
            {
                return 10;
            }

            if (worker.Calories <= 850f)
            {
                return 9;
            }

            return 7;
        }

        private float DiningStressRelief(Vector2Int cell)
        {
            float relief = CountCells(CellKind.MessTable) >= workers.Count ? 7f : 4f;
            if (RoomKindAt(cell.x, cell.y) == RoomKind.MessHall)
            {
                relief += 4f;
            }

            return relief;
        }

        private float RelaxWorkRequired(Worker worker)
        {
            return Mathf.Lerp(4f, 10f, Mathf.Clamp01(worker.Stress / 100f));
        }

        private int RelaxPriority(Worker worker)
        {
            if (worker.Stress >= 90f)
            {
                return 10;
            }

            if (worker.Stress >= 76f)
            {
                return 9;
            }

            return 7;
        }

        private float SleepWorkRequired(Worker worker)
        {
            return Mathf.Lerp(4f, 12f, Mathf.Clamp01(worker.Fatigue / 100f));
        }

        private int SleepPriority(Worker worker)
        {
            if (worker.Fatigue >= 95f || worker.Stress >= 96f)
            {
                return 10;
            }

            return IsRestTime() ? 9 : 7;
        }

        private int JobPriority(Job job)
        {
            if (job.Priority > 0)
            {
                return job.Priority;
            }

            return DefaultPriority(job.Type);
        }

        private int EffectiveJobPriority(Job job)
        {
            int basePriority = JobPriority(job);
            if (basePriority >= 8)
            {
                return basePriority;
            }

            int maxBoost = job.AutoGenerated ? 1 : 2;
            int boost = Mathf.Clamp(Mathf.FloorToInt(job.AgeSeconds / JobAgingPriorityStepSeconds), 0, maxBoost);
            return Mathf.Min(8, basePriority + boost);
        }

        private int JobAssignmentScore(Job job, int pathLength)
        {
            int score = EffectiveJobPriority(job) * 1000;
            score -= Mathf.Max(0, pathLength) * 8;
            score -= Mathf.RoundToInt(job.Progress);
            score += Mathf.Min(220, Mathf.RoundToInt(job.AgeSeconds * 0.8f));
            return score;
        }

        private string JobWaitText(Job job)
        {
            if (job == null)
            {
                return string.Empty;
            }

            int effectivePriority = EffectiveJobPriority(job);
            int basePriority = JobPriority(job);
            string text = "Waiting " + job.AgeSeconds.ToString("0") + "s";
            if (effectivePriority != basePriority)
            {
                text += "  Effective priority " + effectivePriority;
            }

            return text;
        }

        private string BuildJobQueueText(int assignedJobs, int openJobs)
        {
            int categoryCount = Enum.GetValues(typeof(JobCategory)).Length;
            int[] openByCategory = new int[categoryCount];
            int[] assignedByCategory = new int[categoryCount];
            int[] blockedByCategory = new int[categoryCount];
            int blockedJobs = 0;
            int invalidJobs = 0;
            int activeWorkers = CountActiveWorkers();
            Job oldestOpenJob = null;
            Job highestPriorityOpenJob = null;
            int highestPriority = int.MinValue;

            foreach (Job job in jobs)
            {
                int categoryIndex = (int)JobCategoryFor(job.Type);
                if (job.AssignedWorker != null)
                {
                    assignedByCategory[categoryIndex]++;
                    continue;
                }

                openByCategory[categoryIndex]++;
                if (!IsJobValid(job))
                {
                    invalidJobs++;
                    blockedJobs++;
                    blockedByCategory[categoryIndex]++;
                    continue;
                }

                if (!CanAnyActiveWorkerReachJob(job, out _))
                {
                    blockedJobs++;
                    blockedByCategory[categoryIndex]++;
                }

                if (oldestOpenJob == null || job.AgeSeconds > oldestOpenJob.AgeSeconds)
                {
                    oldestOpenJob = job;
                }

                int effectivePriority = EffectiveJobPriority(job);
                if (highestPriorityOpenJob == null ||
                    effectivePriority > highestPriority ||
                    (effectivePriority == highestPriority && job.AgeSeconds > highestPriorityOpenJob.AgeSeconds))
                {
                    highestPriorityOpenJob = job;
                    highestPriority = effectivePriority;
                }
            }

            int reachableJobs = Mathf.Max(0, openJobs - blockedJobs);
            StringBuilder builder = new StringBuilder();
            builder.Append("Errands ");
            builder.Append(assignedJobs);
            builder.Append("/");
            builder.Append(jobs.Count);
            builder.Append("  Active ");
            builder.Append(activeWorkers);
            builder.AppendLine();
            builder.Append("Open ");
            builder.Append(openJobs);
            builder.Append("  Reachable ");
            builder.Append(reachableJobs);
            builder.Append("  Blocked ");
            builder.Append(blockedJobs);
            builder.Append("  Assigned ");
            builder.Append(assignedJobs);
            if (invalidJobs > 0)
            {
                builder.Append("  Invalid ");
                builder.Append(invalidJobs);
            }

            builder.AppendLine();
            builder.Append("Focus: ");
            builder.Append(BuildJobQueueFocusText(openJobs, assignedJobs, blockedJobs, invalidJobs, activeWorkers, highestPriorityOpenJob));
            builder.AppendLine();
            builder.Append("Categories (open/assigned/blocked): ");
            builder.Append(TopJobCategorySummary(openByCategory, assignedByCategory, blockedByCategory));

            if (oldestOpenJob != null)
            {
                builder.AppendLine();
                builder.Append("Oldest: ");
                builder.Append(JobLabel(oldestOpenJob));
                builder.Append("  ");
                builder.Append(JobWaitText(oldestOpenJob));
            }
            else if (jobs.Count == 0)
            {
                builder.AppendLine();
                builder.Append("No queued errands. Use Dig/Build/Operate commands to create work.");
            }

            return builder.ToString();
        }

        private string BuildJobQueueFocusText(
            int openJobs,
            int assignedJobs,
            int blockedJobs,
            int invalidJobs,
            int activeWorkers,
            Job highestPriorityOpenJob)
        {
            if (activeWorkers == 0)
            {
                return "No active duplicants.";
            }

            if (invalidJobs > 0)
            {
                return "Invalid jobs need cancellation.";
            }

            if (blockedJobs > 0)
            {
                return "Build access to blocked work cells.";
            }

            if (openJobs == 0 && assignedJobs == 0)
            {
                return "No queued errands.";
            }

            if (openJobs == 0)
            {
                return "Duplicants are working.";
            }

            if (openJobs > Mathf.Max(1, activeWorkers) * 3)
            {
                return "Reduce backlog with priorities.";
            }

            if (highestPriorityOpenJob != null)
            {
                return JobCategoryName(JobCategoryFor(highestPriorityOpenJob.Type)) +
                    " priority " +
                    EffectiveJobPriority(highestPriorityOpenJob);
            }

            return "Duplicants are working.";
        }

        private string TopJobCategorySummary(int[] openByCategory, int[] assignedByCategory, int[] blockedByCategory)
        {
            bool[] used = new bool[openByCategory.Length];
            StringBuilder builder = new StringBuilder();
            int added = 0;

            for (int slot = 0; slot < 4; slot++)
            {
                int bestIndex = -1;
                int bestTotal = 0;
                for (int i = 0; i < openByCategory.Length; i++)
                {
                    int total = openByCategory[i] + assignedByCategory[i] + blockedByCategory[i];
                    if (!used[i] && total > bestTotal)
                    {
                        bestIndex = i;
                        bestTotal = total;
                    }
                }

                if (bestIndex < 0)
                {
                    break;
                }

                if (added > 0)
                {
                    builder.Append(" | ");
                }

                used[bestIndex] = true;
                added++;
                builder.Append(JobCategoryName((JobCategory)bestIndex));
                builder.Append(" ");
                builder.Append(openByCategory[bestIndex]);
                builder.Append("/");
                builder.Append(assignedByCategory[bestIndex]);
                builder.Append("/");
                builder.Append(blockedByCategory[bestIndex]);
            }

            return added == 0 ? "none" : builder.ToString();
        }

        private JobCategory JobCategoryFor(JobType type)
        {
            switch (type)
            {
                case JobType.Sleep:
                case JobType.UseToilet:
                case JobType.WashHands:
                case JobType.Eat:
                case JobType.Treat:
                case JobType.Rescue:
                    return JobCategory.Survival;
                case JobType.Build:
                case JobType.BuildWire:
                case JobType.BuildAutomationWire:
                case JobType.BuildPipe:
                case JobType.BuildGasPipe:
                case JobType.BuildShippingRail:
                case JobType.Deconstruct:
                    return JobCategory.Construction;
                case JobType.PumpWater:
                case JobType.EmptyBottle:
                    return JobCategory.LifeSupport;
                case JobType.Harvest:
                case JobType.Cook:
                case JobType.TendCrop:
                    return JobCategory.FoodOps;
                case JobType.OperateGenerator:
                    return JobCategory.PowerOps;
                case JobType.Research:
                    return JobCategory.ResearchOps;
                case JobType.Sweep:
                    return JobCategory.Logistics;
                case JobType.Mop:
                case JobType.Repair:
                case JobType.Compost:
                    return JobCategory.Maintenance;
                case JobType.RefineMetal:
                    return JobCategory.Industry;
                case JobType.GroomHatch:
                    return JobCategory.Ranching;
                case JobType.Relax:
                    return JobCategory.MoraleCare;
                default:
                    return JobCategory.Logistics;
            }
        }

        private string JobCategoryName(JobCategory category)
        {
            switch (category)
            {
                case JobCategory.Survival:
                    return "Survival";
                case JobCategory.Construction:
                    return "Construction";
                case JobCategory.LifeSupport:
                    return "Life Support";
                case JobCategory.FoodOps:
                    return "Food Ops";
                case JobCategory.PowerOps:
                    return "Power Ops";
                case JobCategory.ResearchOps:
                    return "Research Ops";
                case JobCategory.Logistics:
                    return "Logistics";
                case JobCategory.Maintenance:
                    return "Maintenance";
                case JobCategory.Industry:
                    return "Industry";
                case JobCategory.Ranching:
                    return "Ranching";
                case JobCategory.MoraleCare:
                    return "Morale Care";
                default:
                    return category.ToString();
            }
        }

        private int DefaultPriority(JobType type)
        {
            switch (type)
            {
                case JobType.Rescue:
                    return 9;
                case JobType.Treat:
                    return 9;
                case JobType.Sleep:
                case JobType.UseToilet:
                case JobType.WashHands:
                case JobType.Eat:
                case JobType.Relax:
                    return 8;
                case JobType.OperateGenerator:
                    return 6;
                case JobType.Cook:
                    return 6;
                case JobType.Mop:
                    return 6;
                case JobType.Repair:
                    return 6;
                case JobType.EmptyBottle:
                    return 6;
                case JobType.RefineMetal:
                    return 5;
                case JobType.PumpWater:
                    return 5;
                case JobType.TendCrop:
                    return 5;
                case JobType.Compost:
                case JobType.GroomHatch:
                    return 5;
                case JobType.Sweep:
                    return 4;
                case JobType.Build:
                case JobType.BuildWire:
                case JobType.BuildAutomationWire:
                case JobType.BuildPipe:
                case JobType.BuildGasPipe:
                case JobType.BuildShippingRail:
                case JobType.Dig:
                case JobType.Deconstruct:
                    return 5;
                case JobType.Harvest:
                    return 4;
                case JobType.Research:
                    return 4;
                default:
                    return 3;
            }
        }

        private void UpdateColonyStatus(bool force)
        {
            int previousScenarioMilestones = CountCompletedScenarioMilestones();
            ApplyResearchUnlocks(false);
            UpdatePoweredWires();
            UpdateAutomationWires();
            UpdatePowerLoad(0f, false);
            float averageOxygen = AverageGas(oxygen);
            float averagePollutedOxygen = AverageGas(pollutedOxygen);
            float averageHydrogen = AverageGas(hydrogen);
            float averageNaturalGas = AverageGas(naturalGas);
            float averageTemperature = AverageTemperature();
            int unsafeTemperatureTiles = CountUnsafeTemperatureTiles();
            int beds = CountCells(CellKind.Bed);
            int planters = CountCells(CellKind.Planter);
            int farmStations = CountCells(CellKind.FarmStation);
            int floors = CountCells(CellKind.Floor);
            int diffusers = CountCells(CellKind.OxygenDiffuser);
            int researchStations = CountCells(CellKind.ResearchStation);
            int mushers = CountCells(CellKind.MicrobeMusher);
            int pumps = CountCells(CellKind.WaterPump);
            int bottleEmptiers = CountCells(CellKind.BottleEmptier);
            int deodorizers = CountCells(CellKind.AirDeodorizer);
            int medicalCots = CountCells(CellKind.MedicalCot);
            int heaters = CountCells(CellKind.SpaceHeater);
            int regulators = CountCells(CellKind.ThermoRegulator);
            int outhouses = CountCells(CellKind.Outhouse);
            int washBasins = CountCells(CellKind.WashBasin);
            int composts = CountCells(CellKind.Compost);
            int massageTables = CountCells(CellKind.MassageTable);
            int airlocks = CountCells(CellKind.ManualAirlock);
            int closedAirlocks = CountClosedAirlocks();
            int refrigerators = CountCells(CellKind.Refrigerator);
            int poweredRefrigerators = CountPoweredRefrigerators();
            int storageBins = CountCells(CellKind.StorageBin);
            int autoSweepers = CountCells(CellKind.AutoSweeper);
            int conveyorLoaders = CountCells(CellKind.ConveyorLoader);
            int conveyorChutes = CountCells(CellKind.ConveyorChute);
            int signalSwitches = CountCells(CellKind.SignalSwitch);
            int shippingRailTiles = CountShippingRailTiles();
            int liquidPipeTiles = CountLiquidPipeTiles();
            int liquidVents = CountCells(CellKind.LiquidVent);
            float totalPipeWater = TotalPipeWater();
            int liquidReservoirs = CountCells(CellKind.LiquidReservoir);
            int moppableSpills = CountMoppableSpills();
            int gasPumps = CountCells(CellKind.GasPump);
            int gasPipeTiles = CountGasPipeTiles();
            int gasVents = CountCells(CellKind.GasVent);
            float totalPipeGas = TotalGasPipeMass();
            int gasReservoirs = CountCells(CellKind.GasReservoir);
            float totalReservoirMass = TotalReservoirMass();
            float mixedHydrogenPipeGas = TotalMixedHydrogenPipeGas();
            int naturalVents = CountCells(CellKind.SteamVent) + CountCells(CellKind.HydrogenVent) + CountCells(CellKind.NaturalGasVent);
            int electrolyzers = CountCells(CellKind.Electrolyzer);
            int carbonSkimmers = CountCells(CellKind.CarbonSkimmer);
            int waterSieves = CountCells(CellKind.WaterSieve);
            int messTables = CountCells(CellKind.MessTable);
            int highestSkillLevel = HighestWorkerSkillLevel();
            int decorPlants = CountCells(CellKind.DecorPlant);
            float averageWorkerDecor = AverageWorkerDecor();
            int wireTiles = CountPowerWireTiles();
            int automationWireTiles = CountAutomationWireTiles();
            int automationControlledGenerators = CountAutomationControlledGenerators();
            int conduitSensors = CountCells(CellKind.LiquidPipeSensor) + CountCells(CellKind.GasPipeSensor);
            int conduitShutoffs = CountCells(CellKind.LiquidShutoff) + CountCells(CellKind.GasShutoff);
            int controlledConduitShutoffs = CountAutomationControlledConduitShutoffs();
            int unwiredPowerBuildings = CountUnwiredPowerBuildings();
            int overloadedWires = CountOverloadedPowerWires();
            int powerTransformers = CountCells(CellKind.PowerTransformer);
            int coalGenerators = CountCells(CellKind.CoalGenerator);
            int hydrogenGenerators = CountCells(CellKind.HydrogenGenerator);
            int naturalGasGenerators = CountCells(CellKind.NaturalGasGenerator);
            int steamTurbines = CountCells(CellKind.SteamTurbine);
            int solarPanels = CountCells(CellKind.SolarPanel);
            int bunkerDoors = CountCells(CellKind.BunkerDoor);
            int spaceScanners = CountCells(CellKind.SpaceScanner);
            int hydrogenFilters = CountCells(CellKind.HydrogenFilter);
            int rockCrushers = CountCells(CellKind.RockCrusher);
            int ranchingStations = CountCells(CellKind.RanchingStation);
            int suitDocks = CountCells(CellKind.AtmoSuitDock);
            int suitCheckpoints = CountCells(CellKind.AtmoSuitCheckpoint);
            int insulatedTiles = CountCells(CellKind.InsulatedTile);
            int printingPods = CountCells(CellKind.PrintingPod);
            int damagedEquipment = CountDamagedEquipment();
            int brokenEquipment = CountBrokenEquipment();
            int incapacitatedWorkers = CountIncapacitatedWorkers();
            float maxWorkerGermExposure = MaxWorkerGermExposure();
            int lowMoraleWorkers = CountLowMoraleWorkers();
            float looseResources = TotalLooseResources();
            EnsureRooms();
            int barracksRooms = CountRoomsOfKind(RoomKind.Barracks);
            int messHallRooms = CountRoomsOfKind(RoomKind.MessHall);

            milestoneBasicShelter |= floors >= 12 && beds >= workers.Count;
            milestoneWaterSupply |= water >= 120f || pumps > 0;
            milestoneBottleEmptying |= bottleEmptiers > 0 && bottleEmptiedLiquid >= 12f;
            milestoneResearchProgram |= researchStations > 0 && techAirSystems;
            milestoneStableOxygen |= diffusers > 0 && averageOxygen >= 0.34f;
            milestoneFoodPreparation |= techFoodPreparation && mushers > 0;
            milestoneFoodProduction |= planters >= 3 || food >= 5200f || (mushers > 0 && food >= 4200f);
            milestoneCropTending |= techFoodPreparation && farmStations > 0 && cropsTended > 0 && CountCropTendingTargets() == 0;
            milestonePowerBuffer |= maxPower >= 160f && power >= 75f;
            milestonePowerGrid |= wireTiles >= 8 && unwiredPowerBuildings == 0 && CountPoweredBuildings() > 0;
            milestonePowerLoadManagement |= techPowerRegulation && powerTransformers > 0 && transformedPowerDelivered >= 4f && overloadedWires == 0;
            milestoneFuelPower |= techPowerRegulation && coalGenerators > 0 && coalPowerGenerated >= 12f;
            milestoneHydrogenFiltering |= techAirSystems && hydrogenFilters > 0 && hydrogenFilteredGas >= 0.8f;
            milestoneHydrogenPower |= techPowerRegulation && hydrogenGenerators > 0 && hydrogenPowerGenerated >= 8f;
            milestoneMetalRefining |= techPowerRegulation && rockCrushers > 0 && refinedMetalProduced >= RockCrusherRefinedMetalYield;
            milestoneAtmoSuits |= techPowerRegulation && suitDocks > 0 && suitCheckpoints > 0 && suitCheckpointUses > 0 && suitOxygenUsed > 0.01f;
            milestoneInsulation |= insulatedTiles >= 4;
            milestoneRoomPlanning |= barracksRooms > 0 && messHallRooms > 0;
            milestoneReconfiguration |= deconstructionsCompleted > 0;
            milestoneColonyExpansion |= printingPods > 0 && (duplicantsPrinted > 0 || workers.Count >= 4);
            milestoneAutomation |= techPowerRegulation && CountCells(CellKind.SmartBattery) > 0 && automationWireTiles >= 3 && automationControlledGenerators > 0;
            milestoneSignalSwitching |= techPowerRegulation && signalSwitches > 0 && automationWireTiles > 0 && signalSwitchesToggled > 0 && AnySignalSwitchLinked();
            milestoneMaintenance |= repairsCompleted > 0;
            milestoneEmergencyResponse |= rescuesCompleted > 0 || (medicalCots > 0 && cycle >= 3 && incapacitatedWorkers == 0);
            milestoneResourceLogistics |= sweptResources >= 12f;
            milestoneAutoSweeping |= techPowerRegulation && autoSweepers > 0 && autoSweptResources >= 6f;
            milestoneShippingLogistics |= techPowerRegulation && conveyorLoaders > 0 && conveyorChutes > 0 && shippingRailTiles >= 3 && conveyorShippedResources >= 6f;
            milestoneThermalControl |= (heaters > 0 || regulators > 0) && averageTemperature >= 12f && averageTemperature <= 34f && unsafeTemperatureTiles < 20;
            milestoneSanitation |= outhouses > 0;
            milestoneHygiene |= washBasins > 0 && handsWashed > 0 && maxWorkerGermExposure <= 55f;
            milestoneWasteProcessing |= composts > 0 && compostedPollutedDirt >= 6f;
            milestoneMoraleCare |= massageTables > 0 && HighestStress() <= 70f && lowMoraleWorkers == 0;
            milestonePressureControl |= airlocks > 0;
            milestoneAirlockControl |= airlocks > 0 && closedAirlocks > 0 && airlockToggles > 0;
            milestoneFoodStorage |= refrigerators > 0 && poweredRefrigerators > 0 && foodFreshness >= 0.45f;
            milestoneMaterialStorage |= storageBins > 0 && DryResourceCapacity() >= BaseDryResourceCapacity + StorageBinCapacity;
            milestonePlumbing |= liquidPipeTiles >= 4 && liquidVents > 0 && totalPipeWater > 0.5f;
            milestoneSpillCleanup |= moppedLiquid >= 20f;
            milestoneVentilation |= gasPumps > 0 && gasPipeTiles >= 4 && gasVents > 0 && totalPipeGas > 0.1f;
            milestoneReservoirBuffering |= (liquidReservoirs > 0 || gasReservoirs > 0) && totalReservoirMass >= 2f && reservoirBufferedMass >= 2f;
            milestoneConduitAutomation |= techPowerRegulation && conduitSensors > 0 && conduitShutoffs > 0 && automationWireTiles >= 3 && controlledConduitShutoffs > 0 && automatedConduitFlow >= 0.5f;
            milestoneRenewableVents |= naturalVents > 0 &&
                (pumps > 0 || gasPumps > 0 || liquidPipeTiles >= 4 || gasPipeTiles >= 4) &&
                (renewableWaterGenerated >= 1f || renewableHydrogenGenerated >= 0.15f || renewableNaturalGasGenerated >= 0.15f);
            milestoneSteamPower |= techPowerRegulation && steamTurbines > 0 && steamTurbinePowerGenerated >= 8f && steamTurbineWaterRecovered >= 0.1f;
            milestoneSolarPower |= techPowerRegulation && solarPanels > 0 && solarPowerGenerated >= 8f;
            milestoneMeteorShielding |= techPowerRegulation && bunkerDoors > 0 && meteorImpactsBlocked >= 2 && CountIntactBunkerDoors() > 0;
            milestoneSpaceScanning |= techPowerRegulation && spaceScanners > 0 && CountSkyExposedSpaceScanners() > 0 && HasLinkedSpaceScanner() && spaceScannerSignalSeconds >= 3f;
            milestoneRanching |= techFoodPreparation && ranchingStations > 0 && hatchesGroomed > 0 && hatchCoalProduced >= 1f;
            milestoneAdvancedAtmosphere |= electrolyzers > 0 && carbonSkimmers > 0 && pollutedWater > 0.1f && averageOxygen >= 0.28f;
            milestoneWaterRecycling |= waterSieves > 0 && recycledWater > 1f;
            milestoneDining |= messTables >= workers.Count && mealsEatenAtTable >= workers.Count;
            milestoneSkilledLabor |= highestSkillLevel >= 3;
            milestoneDecorComfort |= decorPlants >= workers.Count && averageWorkerDecor >= 0.35f;
            milestoneCycleFive |= cycle >= 5 && CountActiveWorkers() > 0;
            unreachableJobCount = CountUnreachableJobs();

            int completedScenarioMilestones = CountCompletedScenarioMilestones();
            if (!force && completedScenarioMilestones > previousScenarioMilestones)
            {
                GrantCharterMilestoneRewards(previousScenarioMilestones, completedScenarioMilestones);
            }

            bool wasVictory = colonyVictory;
            colonyVictory = milestoneBasicShelter &&
                milestoneWaterSupply &&
                milestoneResearchProgram &&
                milestoneStableOxygen &&
                milestoneFoodPreparation &&
                milestoneFoodProduction &&
                milestoneCropTending &&
                milestonePowerBuffer &&
                milestonePowerGrid &&
                milestonePowerLoadManagement &&
                milestoneFuelPower &&
                milestoneHydrogenFiltering &&
                milestoneHydrogenPower &&
                milestoneMetalRefining &&
                milestoneAtmoSuits &&
                milestoneInsulation &&
                milestoneRoomPlanning &&
                milestoneReconfiguration &&
                milestoneColonyExpansion &&
                milestoneMaintenance &&
                milestoneEmergencyResponse &&
                milestoneResourceLogistics &&
                milestoneBottleEmptying &&
                milestoneAutomation &&
                milestoneSignalSwitching &&
                milestoneAutoSweeping &&
                milestoneShippingLogistics &&
                milestoneConduitAutomation &&
                milestoneRenewableVents &&
                milestoneSteamPower &&
                milestoneSolarPower &&
                milestoneMeteorShielding &&
                milestoneSpaceScanning &&
                milestoneRanching &&
                milestoneThermalControl &&
                milestoneSanitation &&
                milestoneHygiene &&
                milestoneWasteProcessing &&
                milestoneMoraleCare &&
                milestonePressureControl &&
                milestoneAirlockControl &&
                milestoneFoodStorage &&
                milestoneMaterialStorage &&
                milestonePlumbing &&
                milestoneSpillCleanup &&
                milestoneVentilation &&
                milestoneReservoirBuffering &&
                milestoneAdvancedAtmosphere &&
                milestoneWaterRecycling &&
                milestoneDining &&
                milestoneSkilledLabor &&
                milestoneDecorComfort &&
                milestoneCycleFive;

            if (colonyVictory)
            {
                objectiveText = "Colony charter complete. Expand into the asteroid and keep the colony alive.";
                if (!wasVictory || force)
                {
                    Log("Colony charter complete. Freeplay continues.");
                }
            }
            else if (!milestoneBasicShelter)
            {
                objectiveText = "Build a safe barracks: 12 floor tiles and one bed per duplicant.";
            }
            else if (!milestoneSanitation)
            {
                objectiveText = "Build an Outhouse so duplicants can relieve bladder pressure safely.";
            }
            else if (!milestoneHygiene)
            {
                if (washBasins == 0)
                {
                    objectiveText = "Build a Wash Basin near the outhouse so duplicants clean germs before getting sick.";
                }
                else if (water <= 0.05f)
                {
                    objectiveText = "Store clean water so the Wash Basin can remove germs from duplicants.";
                }
                else
                {
                    objectiveText = "Let a germy duplicant wash hands at the Wash Basin to prove hygiene control.";
                }
            }
            else if (!milestoneWasteProcessing)
            {
                objectiveText = "Build a Compost and process polluted dirt from outhouses or spoiled food.";
            }
            else if (!milestoneMaterialStorage)
            {
                objectiveText = "Build a Storage Bin so mined dirt, metal, algae, and coal have room to accumulate.";
            }
            else if (!milestoneResourceLogistics)
            {
                objectiveText = looseResources > 0.05f
                    ? "Use Sweep on mined debris so duplicants move resources into storage."
                    : "Dig natural tiles to create debris, then Sweep it into storage.";
            }
            else if (!milestoneWaterSupply)
            {
                objectiveText = "Secure water: build a water pump beside a blue water pocket.";
            }
            else if (!milestoneBottleEmptying)
            {
                if (bottleEmptiers == 0)
                {
                    objectiveText = "Build a Bottle Emptier over a pit so stored polluted water can return to the world.";
                }
                else if (!HasStoredLiquidForBottleEmptier())
                {
                    objectiveText = "Collect bottled liquid by mopping spills or stockpile surplus clean water for the Bottle Emptier.";
                }
                else if (!AnyBottleEmptierHasOutput())
                {
                    objectiveText = "Leave an empty or shallow water tile below or beside the Bottle Emptier.";
                }
                else
                {
                    objectiveText = "Let a duplicant empty bottles into the pit to prove liquid handling.";
                }
            }
            else if (!milestonePlumbing)
            {
                objectiveText = "Build Liquid Pipes and a Liquid Vent to move water through plumbing.";
            }
            else if (!milestoneSpillCleanup)
            {
                objectiveText = moppableSpills > 0
                    ? "Use Mop on a shallow spill to recover liquid and reduce contamination."
                    : "Create a shallow spill with a Liquid Vent, then use Mop to clean it up.";
            }
            else if (!milestoneResearchProgram)
            {
                objectiveText = "Start research: build a research station and complete Air Systems.";
            }
            else if (!milestoneStableOxygen)
            {
                objectiveText = "Stabilize oxygen: build and fuel an oxygen diffuser.";
            }
            else if (!milestonePressureControl)
            {
                objectiveText = "Control pressure: build a Manual Airlock to slow gas and heat exchange between rooms.";
            }
            else if (!milestoneAirlockControl)
            {
                objectiveText = airlocks == 0
                    ? "Build a Manual Airlock, then close it to prove room pressure control."
                    : closedAirlocks == 0
                    ? "Inspect a Manual Airlock and close it to seal pathing, gas, and heat."
                    : "Keep a Manual Airlock closed to prove room pressure control.";
            }
            else if (!milestoneVentilation)
            {
                objectiveText = "Build Gas Pump, Gas Pipe, and Gas Vent to route breathable air.";
            }
            else if (!milestoneReservoirBuffering)
            {
                if (liquidReservoirs == 0 && gasReservoirs == 0)
                {
                    objectiveText = "Build a Liquid Reservoir or Gas Reservoir on a pipe network to buffer flow.";
                }
                else if (totalReservoirMass <= 0.05f)
                {
                    objectiveText = "Connect reservoir input to a flowing pipe so it stores liquid or gas.";
                }
                else
                {
                    objectiveText = "Let the reservoir buffer at least 2 kg, then feed it back into pipes.";
                }
            }
            else if (!milestoneAdvancedAtmosphere)
            {
                objectiveText = "Build an Electrolyzer and Carbon Skimmer to process water into oxygen and scrub CO2.";
            }
            else if (!milestoneWaterRecycling)
            {
                objectiveText = "Build a Water Sieve to recycle polluted water back into clean water.";
            }
            else if (averagePollutedOxygen > 0.12f && deodorizers == 0)
            {
                objectiveText = "Polluted oxygen detected: build an Air Deodorizer near green gas pockets.";
            }
            else if (AnyWorkerNeedsTreatment() && medicalCots == 0)
            {
                objectiveText = "Build a Medical Cot to treat sick or injured duplicants.";
            }
            else if (!milestoneEmergencyResponse)
            {
                objectiveText = incapacitatedWorkers > 0
                    ? "Rescue incapacitated duplicants to a Medical Cot before the colony collapses."
                    : "Keep a Medical Cot ready through cycle 3, or rescue an incapacitated duplicant.";
            }
            else if (!milestoneMoraleCare)
            {
                objectiveText = lowMoraleWorkers > 0
                    ? "Improve morale with Mess Halls, Barracks, Decor Plants, and Massage Tables until skill needs are met."
                    : "Add stress care: build a Massage Table and keep stress under 70%.";
            }
            else if (!milestoneFoodPreparation)
            {
                objectiveText = "Research Food Preparation, then build a Microbe Musher.";
            }
            else if (!milestoneFoodStorage)
            {
                objectiveText = "Preserve food: build and power a Refrigerator before stored meals spoil.";
            }
            else if (!milestoneFoodProduction)
            {
                objectiveText = CountStressedCrops() > 0
                    ? "Restore crop oxygen, temperature, water, and pressure so mealwood can keep growing."
                    : "Secure food: grow mealwood or cook mush bars until food reserves stabilize.";
            }
            else if (!milestoneCropTending)
            {
                if (!techFoodPreparation)
                {
                    objectiveText = "Research Food Preparation before improving crop tending.";
                }
                else if (farmStations == 0)
                {
                    objectiveText = "Build a Farm Station near planters so duplicants can tend crops.";
                }
                else if (pollutedDirt < CropTendPollutedDirtCost && dirt < CropTendDirtFallbackCost)
                {
                    objectiveText = "Keep polluted dirt or dirt available so the Farm Station can fertilize crops.";
                }
                else
                {
                    objectiveText = "Let a duplicant tend a planter from the Farm Station to speed up food production.";
                }
            }
            else if (!milestoneDining)
            {
                objectiveText = "Build one Mess Table per duplicant so meals improve morale.";
            }
            else if (!milestoneSkilledLabor)
            {
                objectiveText = "Train a duplicant to Skill Lv 3 by completing skilled work.";
            }
            else if (!milestoneDecorComfort)
            {
                objectiveText = "Decorate living areas with Decor Plants to raise morale and reduce stress.";
            }
            else if (!milestonePowerBuffer)
            {
                objectiveText = "Create a power buffer: add battery capacity and store 75 power.";
            }
            else if (!milestonePowerGrid)
            {
                objectiveText = "Wire the power grid: connect generators, batteries, and powered machines.";
            }
            else if (!milestonePowerLoadManagement)
            {
                if (!techPowerRegulation)
                {
                    objectiveText = "Research Power Regulation so the grid can handle heavier machine loads.";
                }
                else if (powerTransformers == 0)
                {
                    objectiveText = "Build a Power Transformer beside a busy power wire to raise its safe load.";
                }
                else if (transformedPowerDelivered < 4f)
                {
                    objectiveText = "Run powered machines through transformer-protected wire until it proves stable.";
                }
                else
                {
                    objectiveText = "Reduce overloaded wire load by adding transformers or splitting machinery across more wires.";
                }
            }
            else if (!milestoneFuelPower)
            {
                objectiveText = "Build a Coal Generator, mine coal, and let it produce backup power.";
            }
            else if (!milestoneHydrogenFiltering)
            {
                objectiveText = mixedHydrogenPipeGas > 0.05f
                    ? "Build and power a Hydrogen Filter beside mixed H2 gas pipe to leave fuel behind."
                    : "Pump mixed hydrogen gas into gas pipes, then filter non-H2 before burning it.";
            }
            else if (!milestoneHydrogenPower)
            {
                objectiveText = averageHydrogen > 0.03f
                    ? "Build and wire a Hydrogen Generator, then feed it purple hydrogen through gas pipes or nearby gas."
                    : "Run an Electrolyzer to create hydrogen, then pump it into a Hydrogen Generator.";
            }
            else if (averageNaturalGas > 0.03f && naturalGasGenerators == 0)
            {
                objectiveText = "Natural gas found: build and wire a Natural Gas Generator to convert orange gas into power.";
            }
            else if (!milestoneMetalRefining)
            {
                objectiveText = "Build and power a Rock Crusher, then refine metal ore into Refined Metal.";
            }
            else if (!milestoneAtmoSuits)
            {
                if (suitDocks == 0)
                {
                    objectiveText = "Build an Atmo Suit Dock so duplicants can prepare for unsafe gas and temperature.";
                }
                else if (suitCheckpoints == 0)
                {
                    objectiveText = "Build an Atmo Suit Checkpoint beside a dock to control entry into unsafe areas.";
                }
                else if (suitOxygen <= SuitCheckpointMinimumCharge)
                {
                    objectiveText = suitEntryDenials > 0
                        ? "The Atmo Suit Checkpoint is blocking unsafe entry; charge the dock with oxygen."
                        : "Wire the Atmo Suit Dock and charge it with local oxygen before crossing the checkpoint.";
                }
                else
                {
                    objectiveText = "Send a duplicant through the Atmo Suit Checkpoint into unsafe air to use suit oxygen.";
                }
            }
            else if (!milestoneInsulation)
            {
                objectiveText = "Build four Insulated Tiles to separate living rooms from dangerous heat and gas.";
            }
            else if (!milestoneRoomPlanning)
            {
                objectiveText = "Create separate Barracks and Mess Hall rooms with airlocks or insulated walls.";
            }
            else if (!milestoneReconfiguration)
            {
                objectiveText = "Use Deconstruct on a building or conduit to recover materials and reconfigure the base.";
            }
            else if (!milestoneColonyExpansion)
            {
                objectiveText = "Keep a Printing Pod open until it prints a fourth duplicant.";
            }
            else if (!milestoneMaintenance)
            {
                objectiveText = damagedEquipment > 0 || brokenEquipment > 0
                    ? "Use Repair on damaged machinery before it breaks the colony loop."
                    : "Run powered machinery until it wears down, then repair it with metal.";
            }
            else if (!milestoneAutomation)
            {
                objectiveText = "Automate power: research Power Regulation, then link a Smart Battery to a generator with Automation Wire.";
            }
            else if (!milestoneSignalSwitching)
            {
                if (!techPowerRegulation)
                {
                    objectiveText = "Research Power Regulation so the colony can build manual automation controls.";
                }
                else if (signalSwitches == 0)
                {
                    objectiveText = "Build a Signal Switch to manually send green or red automation signals.";
                }
                else if (!AnySignalSwitchLinked())
                {
                    objectiveText = "Connect a Signal Switch to Automation Wire.";
                }
                else if (signalSwitchesToggled == 0)
                {
                    objectiveText = "Inspect the Signal Switch and toggle it to prove manual automation control.";
                }
                else if (!AnySignalSwitchGreen())
                {
                    objectiveText = "Toggle the Signal Switch on so it sends a green automation signal.";
                }
                else
                {
                    objectiveText = "Route the Signal Switch signal to a generator, shutoff, or sensor loop.";
                }
            }
            else if (!milestoneAutoSweeping)
            {
                if (!techPowerRegulation)
                {
                    objectiveText = "Research Power Regulation so the colony can automate debris cleanup.";
                }
                else if (autoSweepers == 0)
                {
                    objectiveText = "Build an Auto-Sweeper near mined debris and a Storage Bin.";
                }
                else if (DryResourceFreeSpace() <= 0.01f)
                {
                    objectiveText = "Free dry storage space so the Auto-Sweeper can move debris.";
                }
                else if (looseResources <= 0.05f)
                {
                    objectiveText = "Dig or leave debris inside Auto-Sweeper range so it can stockpile resources.";
                }
                else
                {
                    objectiveText = "Wire the Auto-Sweeper and let it collect nearby debris into storage.";
                }
            }
            else if (!milestoneShippingLogistics)
            {
                if (!techPowerRegulation)
                {
                    objectiveText = "Research Power Regulation so the colony can build conveyor shipping.";
                }
                else if (conveyorLoaders == 0 || conveyorChutes == 0)
                {
                    objectiveText = "Build a Conveyor Loader, Shipping Rail, and Conveyor Chute to move stored resources remotely.";
                }
                else if (shippingRailTiles < 3)
                {
                    objectiveText = "Extend Shipping Rail between the loader and chute.";
                }
                else if (DryResourceAmount() <= 0.05f)
                {
                    objectiveText = "Store dry resources so the Conveyor Loader has material to ship.";
                }
                else
                {
                    objectiveText = "Wire the Conveyor Loader and let packets reach the Conveyor Chute.";
                }
            }
            else if (!milestoneConduitAutomation)
            {
                if (conduitSensors == 0 || conduitShutoffs == 0)
                {
                    objectiveText = "Automate conduits: build a Pipe Sensor and matching Shutoff on liquid or gas pipes.";
                }
                else if (controlledConduitShutoffs == 0)
                {
                    objectiveText = "Connect the pipe sensor to the shutoff with Automation Wire.";
                }
                else
                {
                    objectiveText = "Run liquid or gas through a green automated shutoff to prove conduit control.";
                }
            }
            else if (!milestoneRenewableVents)
            {
                if (naturalVents == 0)
                {
                    objectiveText = "Explore the upper asteroid until a Steam Vent, Hydrogen Vent, or Natural Gas Vent is uncovered.";
                }
                else if (pumps == 0 && gasPumps == 0)
                {
                    objectiveText = "Tap a natural vent: build a Water Pump or Gas Pump near the vent pocket.";
                }
                else
                {
                    objectiveText = "Let a natural vent produce renewable water, hydrogen, or natural gas, then route it through pipes.";
                }
            }
            else if (!milestoneSteamPower)
            {
                if (!techPowerRegulation)
                {
                    objectiveText = "Research Power Regulation so the colony can build a Steam Turbine.";
                }
                else if (steamTurbines == 0)
                {
                    objectiveText = "Build and wire a Steam Turbine near hot steam to recover water and power.";
                }
                else if (CountSteamTiles() == 0)
                {
                    objectiveText = "Create or uncover hot steam near the Steam Turbine with phase change or a Steam Vent.";
                }
                else
                {
                    objectiveText = "Feed hot steam to the Steam Turbine until it produces power and recovered water.";
                }
            }
            else if (!milestoneSolarPower)
            {
                if (!techPowerRegulation)
                {
                    objectiveText = "Research Power Regulation so the colony can build Solar Panels.";
                }
                else if (solarPanels == 0)
                {
                    objectiveText = "Build and wire a Solar Panel under open sky for renewable daylight power.";
                }
                else if (CountSkyExposedSolarPanels() == 0)
                {
                    objectiveText = "Dig a clear vertical shaft above the Solar Panel so daylight can reach it.";
                }
                else
                {
                    objectiveText = "Let the exposed Solar Panel generate daylight power.";
                }
            }
            else if (!milestoneMeteorShielding)
            {
                if (!techPowerRegulation)
                {
                    objectiveText = "Research Power Regulation so the colony can build meteor shielding.";
                }
                else if (bunkerDoors == 0)
                {
                    objectiveText = "Build Bunker Doors above exposed surface infrastructure before the next meteor shower.";
                }
                else if (meteorImpactsBlocked == 0)
                {
                    objectiveText = IsMeteorShowerActive()
                        ? "Let Bunker Doors absorb meteor impacts, then clear regolith from the surface."
                        : "Wait for the next meteor shower or expand Bunker Door coverage over exposed shafts.";
                }
                else
                {
                    objectiveText = "Keep Bunker Doors repaired and clear regolith after meteor impacts.";
                }
            }
            else if (!milestoneSpaceScanning)
            {
                if (!techPowerRegulation)
                {
                    objectiveText = "Research Power Regulation so the colony can scan for meteor showers.";
                }
                else if (spaceScanners == 0)
                {
                    objectiveText = "Build and wire a Space Scanner under open sky to forecast meteor showers.";
                }
                else if (CountSkyExposedSpaceScanners() == 0)
                {
                    objectiveText = "Clear the sky above the Space Scanner so it can detect incoming meteors.";
                }
                else if (!HasLinkedSpaceScanner())
                {
                    objectiveText = "Connect the Space Scanner to Automation Wire so its meteor warning can control doors.";
                }
                else
                {
                    objectiveText = "Let the Space Scanner send a green warning before or during a meteor shower.";
                }
            }
            else if (!milestoneRanching)
            {
                if (hatches.Count == 0)
                {
                    objectiveText = "Explore asteroid pockets to discover wild hatches for a renewable coal loop.";
                }
                else if (ranchingStations == 0)
                {
                    objectiveText = "Build a Ranching Station near wild hatches so duplicants can groom them.";
                }
                else if (!HasHatchEdibleDebris())
                {
                    objectiveText = "Leave loose dirt, algae, or polluted dirt near hatches so they can produce coal.";
                }
                else
                {
                    objectiveText = "Groom a hatch and let it eat loose debris until it produces coal.";
                }
            }
            else if (!milestoneThermalControl)
            {
                objectiveText = "Stabilize temperature: build heaters or thermo regulators to keep living areas safe.";
            }
            else
            {
                objectiveText = "Survive until cycle 5 with at least one active duplicant.";
            }

            alertText = BuildAlertText(averageOxygen, averagePollutedOxygen, averageHydrogen, averageNaturalGas, averageTemperature, unsafeTemperatureTiles, unwiredPowerBuildings, overloadedWires, MaxPowerWireLoad(), maxWorkerGermExposure, unreachableJobCount);
        }

        private void ApplyResearchUnlocks(bool announce)
        {
            if (!techAirSystems && researchPoints >= 8f)
            {
                techAirSystems = true;
                if (announce)
                {
                    Log("Research unlocked: Air Systems.");
                }
            }

            if (!techFoodPreparation && researchPoints >= 16f)
            {
                techFoodPreparation = true;
                if (announce)
                {
                    Log("Research unlocked: Food Preparation.");
                }
            }

            if (!techPowerRegulation && researchPoints >= 28f)
            {
                techPowerRegulation = true;
                maxPower += CountCells(CellKind.Battery) * 25f;
                if (announce)
                {
                    Log("Research unlocked: Power Regulation.");
                }
            }
        }

        private string BuildAlertText(float averageOxygen, float averagePollutedOxygen, float averageHydrogen, float averageNaturalGas, float averageTemperature, int unsafeTemperatureTiles, int unwiredPowerBuildings, int overloadedWires, float maxWireLoad, float maxWorkerGermExposure, int unreachableJobs)
        {
            if (colonyFailed)
            {
                return "Colony failed. Load a save or start a new run.";
            }

            StringBuilder builder = new StringBuilder();
            if (averageOxygen < 0.18f)
            {
                AppendAlert(builder, "low oxygen");
            }

            if (unreachableJobs > 0)
            {
                AppendAlert(builder, unreachableJobs + " unreachable jobs");
            }

            if (AnyWorkerNeedsRescue())
            {
                AppendAlert(builder, "duplicant down");
                if (CountCells(CellKind.MedicalCot) == 0)
                {
                    AppendAlert(builder, "no rescue cot");
                }
            }

            if (averagePollutedOxygen > 0.16f)
            {
                AppendAlert(builder, "polluted oxygen");
            }

            if (maxWorkerGermExposure > 45f)
            {
                AppendAlert(builder, "germy duplicants");
            }
            else if (maxWorkerGermExposure > 18f && CountCells(CellKind.WashBasin) == 0)
            {
                AppendAlert(builder, "no wash basin");
            }

            int lowMoraleWorkers = CountLowMoraleWorkers();
            if (lowMoraleWorkers > 0)
            {
                AppendAlert(builder, lowMoraleWorkers + " low morale");
            }

            if (averageHydrogen > 0.08f && CountCells(CellKind.HydrogenGenerator) == 0)
            {
                AppendAlert(builder, "hydrogen buildup");
            }

            if (averageNaturalGas > 0.08f && CountCells(CellKind.NaturalGasGenerator) == 0)
            {
                AppendAlert(builder, "natural gas buildup");
            }

            if (TotalMixedHydrogenPipeGas() > 0.25f && CountCells(CellKind.HydrogenFilter) == 0)
            {
                AppendAlert(builder, "mixed H2 pipe");
            }

            if (AverageGas(carbonDioxide) > 0.28f && CountCells(CellKind.CarbonSkimmer) == 0)
            {
                AppendAlert(builder, "high CO2");
            }

            int airlocks = CountCells(CellKind.ManualAirlock);
            if (averagePollutedOxygen > 0.10f && airlocks == 0)
            {
                AppendAlert(builder, "no airlocks");
            }
            else if (averagePollutedOxygen > 0.10f && CountClosedAirlocks() == 0)
            {
                AppendAlert(builder, "airlocks open");
            }

            int unstableSand = CountUnstableSandTiles();
            if (unstableSand > 0)
            {
                AppendAlert(builder, unstableSand + " unstable sand");
            }

            int flowingLiquids = CountFlowingLiquidTiles();
            if (flowingLiquids > 0)
            {
                AppendAlert(builder, flowingLiquids + " moving liquid");
            }

            int meltingIce = CountMeltingIceTiles();
            if (meltingIce > 0)
            {
                AppendAlert(builder, meltingIce + " melting ice");
            }

            int freezingWater = CountFreezingWaterTiles();
            if (freezingWater > 0)
            {
                AppendAlert(builder, freezingWater + " freezing water");
            }

            int steamTiles = CountSteamTiles();
            if (steamTiles > 0)
            {
                AppendAlert(builder, steamTiles + " steam cloud");
                if (techPowerRegulation && CountCells(CellKind.SteamTurbine) == 0)
                {
                    AppendAlert(builder, "untapped steam power");
                }
            }

            int chlorineTiles = CountChlorineTiles();
            if (chlorineTiles > 0)
            {
                AppendAlert(builder, chlorineTiles + " chlorine pocket");
            }

            int submergedEquipment = CountSubmergedEquipment();
            if (submergedEquipment > 0)
            {
                AppendAlert(builder, submergedEquipment + " submerged equipment");
            }

            int floodedPowerWires = CountFloodedPowerWires();
            if (floodedPowerWires > 0)
            {
                AppendAlert(builder, floodedPowerWires + " flooded power wire");
            }

            int overheatingEquipment = CountOverheatingEquipment();
            if (overheatingEquipment > 0)
            {
                AppendAlert(builder, overheatingEquipment + " equipment overheating");
            }

            if (averageTemperature > 36f)
            {
                AppendAlert(builder, "base too hot");
            }
            else if (averageTemperature < 8f)
            {
                AppendAlert(builder, "base too cold");
            }

            if (unsafeTemperatureTiles > 28)
            {
                AppendAlert(builder, "dangerous temperatures");
            }

            int criticalThermalWorkers = CountCriticalThermalExposureWorkers();
            if (criticalThermalWorkers > 0)
            {
                AppendAlert(builder, criticalThermalWorkers + " thermal injury");
            }
            else
            {
                int thermalWorkers = CountThermallyExposedWorkers();
                if (thermalWorkers > 0)
                {
                    AppendAlert(builder, thermalWorkers + " temperature exposure");
                }
            }

            int highPressureTiles = CountHighPressureTiles();
            if (highPressureTiles > 0)
            {
                int dangerousPressureTiles = CountDangerousPressureTiles();
                AppendAlert(builder, dangerousPressureTiles > 0 ? dangerousPressureTiles + " dangerous pressure" : highPressureTiles + " high pressure");
            }

            if (power < 10f)
            {
                AppendAlert(builder, "low power");
            }

            if (SolarIrradiance() > 0.05f && CountCells(CellKind.SolarPanel) > 0 && CountSkyExposedSolarPanels() == 0)
            {
                AppendAlert(builder, "solar panels blocked");
            }

            if (IsMeteorShowerActive())
            {
                AppendAlert(builder, "meteor shower");
            }
            else if (techPowerRegulation && CountCells(CellKind.SpaceScanner) > 0 && CountSkyExposedSpaceScanners() == 0 && meteorCooldownSeconds < SpaceScannerWarningSeconds * 1.5f)
            {
                AppendAlert(builder, "scanner blocked");
            }
            else if (techPowerRegulation && CountCells(CellKind.SolarPanel) > 0 && CountCells(CellKind.BunkerDoor) == 0 && meteorCooldownSeconds < CycleLengthSeconds)
            {
                AppendAlert(builder, "no meteor shielding");
            }

            if (unwiredPowerBuildings > 0)
            {
                AppendAlert(builder, "unwired buildings");
            }

            if (overloadedWires > 0)
            {
                AppendAlert(builder, "wire overload");
            }
            else if (techPowerRegulation && CountCells(CellKind.PowerTransformer) == 0 && maxWireLoad > WireSafeLoad * 0.85f)
            {
                AppendAlert(builder, "no transformer");
            }

            int brokenEquipment = CountBrokenEquipment();
            int damagedEquipment = CountDamagedEquipment();
            if (brokenEquipment > 0)
            {
                AppendAlert(builder, "broken equipment");
            }
            else if (damagedEquipment > 0)
            {
                AppendAlert(builder, "damaged equipment");
            }

            if (techPowerRegulation && (CountCells(CellKind.ManualGenerator) > 0 || CountCells(CellKind.CoalGenerator) > 0 || CountCells(CellKind.NaturalGasGenerator) > 0 || CountCells(CellKind.SteamTurbine) > 0 || CountCells(CellKind.SolarPanel) > 0) && CountCells(CellKind.SmartBattery) == 0)
            {
                AppendAlert(builder, "no smart battery");
            }

            if (CountCells(CellKind.CoalGenerator) > 0 && coal < 1f)
            {
                AppendAlert(builder, "coal generator out of coal");
            }

            if (CountCells(CellKind.RockCrusher) > 0 && metal < RockCrusherOrePerJob)
            {
                AppendAlert(builder, "rock crusher needs metal ore");
            }

            if (CountCells(CellKind.AtmoSuitDock) > 0 && suitOxygen <= 0.05f)
            {
                AppendAlert(builder, "suit dock oxygen empty");
            }

            if (suitEntryDenials > 0 && CountCells(CellKind.AtmoSuitCheckpoint) > 0 && suitOxygen <= SuitCheckpointMinimumCharge)
            {
                AppendAlert(builder, "suit checkpoint blocking unsafe entry");
            }

            if (CountCells(CellKind.AtmoSuitDock) > 0 && CountCells(CellKind.AtmoSuitCheckpoint) == 0)
            {
                AppendAlert(builder, "no suit checkpoint");
            }

            int naturalVents = CountCells(CellKind.SteamVent) + CountCells(CellKind.HydrogenVent) + CountCells(CellKind.NaturalGasVent);
            if (naturalVents > 0 &&
                renewableWaterGenerated < 0.5f &&
                renewableHydrogenGenerated < 0.08f &&
                renewableNaturalGasGenerated < 0.08f &&
                CountCells(CellKind.WaterPump) == 0 &&
                CountCells(CellKind.GasPump) == 0)
            {
                AppendAlert(builder, "untapped natural vent");
            }

            if ((renewableWaterGenerated > 1f || renewableHydrogenGenerated > 0.15f || renewableNaturalGasGenerated > 0.15f) && CountCells(CellKind.InsulatedTile) == 0)
            {
                AppendAlert(builder, "hot vent exposed");
            }

            if (hatches.Count > 0 && CountCells(CellKind.RanchingStation) == 0)
            {
                AppendAlert(builder, "wild hatches");
            }

            if (hatches.Count > 0 && CountCells(CellKind.RanchingStation) > 0 && CountUngroomedHatches() > 0)
            {
                AppendAlert(builder, "hatches need grooming");
            }

            if (hatches.Count > 0 && !HasHatchEdibleDebris())
            {
                AppendAlert(builder, "hatches need debris");
            }

            if (techFoodPreparation && CountCells(CellKind.Planter) >= 3 && CountCells(CellKind.FarmStation) == 0)
            {
                AppendAlert(builder, "untended crops");
            }
            else if (CountCells(CellKind.FarmStation) > 0 && CountCropTendingTargets() > 0 && pollutedDirt < CropTendPollutedDirtCost && dirt < CropTendDirtFallbackCost)
            {
                AppendAlert(builder, "farm needs fertilizer");
            }

            int wiltingCrops = CountWiltingCrops();
            if (wiltingCrops > 0)
            {
                AppendAlert(builder, wiltingCrops + " crop wilting");
            }
            else
            {
                int stressedCrops = CountStressedCrops();
                if (stressedCrops > 0)
                {
                    AppendAlert(builder, stressedCrops + " crop stifled");
                }
            }

            if (unsafeTemperatureTiles > 12 && CountCells(CellKind.InsulatedTile) == 0)
            {
                AppendAlert(builder, "no insulated tiles");
            }

            if (CountCells(CellKind.Bed) > 0 && CountRoomsOfKind(RoomKind.Barracks) == 0)
            {
                AppendAlert(builder, "no barracks room");
            }

            if (CountCells(CellKind.MessTable) > 0 && CountRoomsOfKind(RoomKind.MessHall) == 0)
            {
                AppendAlert(builder, "no mess hall room");
            }

            if (CountCells(CellKind.PrintingPod) > 0 && workers.Count < MaxWorkers && printingPodProgress >= 0.98f && !TryFindPrintingPodSpawn(out _))
            {
                AppendAlert(builder, "printing pod blocked");
            }

            if (algae < 8f)
            {
                AppendAlert(builder, "low algae");
            }

            float dryUsed = DryResourceAmount();
            float dryCapacity = DryResourceCapacity();
            if (CountLooseResourceTiles() > 0 && DryResourceFreeSpace() > 0.01f)
            {
                AppendAlert(builder, "loose debris");
                if (techPowerRegulation && CountCells(CellKind.AutoSweeper) == 0 && CountLooseResourceTiles() >= 4)
                {
                    AppendAlert(builder, "no auto-sweeper");
                }
            }

            if (techPowerRegulation && CountCells(CellKind.ConveyorLoader) > 0)
            {
                if (CountShippingRailTiles() == 0)
                {
                    AppendAlert(builder, "no shipping rail");
                }
                else if (CountCells(CellKind.ConveyorChute) == 0)
                {
                    AppendAlert(builder, "no conveyor chute");
                }
            }

            if (techPowerRegulation && CountCells(CellKind.SignalSwitch) > 0 && !AnySignalSwitchLinked())
            {
                AppendAlert(builder, "unwired signal switch");
            }

            if (dryUsed >= dryCapacity - 0.5f)
            {
                AppendAlert(builder, "dry storage full");
            }
            else if (CountCells(CellKind.StorageBin) == 0 && dryUsed > dryCapacity * 0.75f)
            {
                AppendAlert(builder, "no storage bin");
            }

            if (water < workers.Count * 12f)
            {
                AppendAlert(builder, "low water");
            }

            if (CountMoppableSpills() > 0)
            {
                AppendAlert(builder, "liquid spill");
            }

            if (pollutedWater > 80f)
            {
                AppendAlert(builder, "polluted water stored");
            }
            if (pollutedWater > 5f && CountPollutedWaterOffgasSources() > 0)
            {
                AppendAlert(builder, "polluted water offgassing");
            }
            if (pollutedWater > 20f && CountCells(CellKind.BottleEmptier) == 0)
            {
                AppendAlert(builder, "no bottle emptier");
            }
            else if (pollutedWater > 20f && CountCells(CellKind.WaterSieve) == 0)
            {
                AppendAlert(builder, "no water sieve");
            }

            if (pollutedDirt > 36f)
            {
                AppendAlert(builder, "polluted dirt offgassing");
            }
            else if (pollutedDirt > 10f && CountCells(CellKind.Compost) == 0)
            {
                AppendAlert(builder, "no compost");
            }

            float pipeStoredWater = TotalPipeWater();
            int pipeTiles = CountLiquidPipeTiles();
            int liquidVents = CountCells(CellKind.LiquidVent);
            int unsafeLiquidPipes = CountUnsafeLiquidPipes();
            int unsafeLiquidReservoirs = CountUnsafeLiquidReservoirs();
            if (unsafeLiquidPipes > 0)
            {
                AppendAlert(builder, unsafeLiquidPipes + " pipe phase risk");
            }
            else if (pipeBurstEvents > 0)
            {
                AppendAlert(builder, pipeBurstEvents + " pipe burst");
            }

            if (unsafeLiquidReservoirs > 0)
            {
                AppendAlert(builder, unsafeLiquidReservoirs + " reservoir phase risk");
            }
            else if (reservoirBurstEvents > 0)
            {
                AppendAlert(builder, reservoirBurstEvents + " reservoir burst");
            }

            if (pipeStoredWater > LiquidPipeCapacity && liquidVents == 0)
            {
                AppendAlert(builder, "pipe water backed up");
            }
            else if (pipeStoredWater > LiquidPipeCapacity && CountCells(CellKind.LiquidReservoir) == 0)
            {
                AppendAlert(builder, "no liquid reservoir");
            }
            else if (liquidVents > 0 && pipeTiles == 0)
            {
                AppendAlert(builder, "unconnected liquid vent");
            }

            float pipeStoredGas = TotalGasPipeMass();
            int gasPipeTiles = CountGasPipeTiles();
            int gasPumps = CountCells(CellKind.GasPump);
            int gasVents = CountCells(CellKind.GasVent);
            if (pipeStoredGas > GasPipeCapacity && gasVents == 0)
            {
                AppendAlert(builder, "gas pipe backed up");
            }
            else if (pipeStoredGas > GasPipeCapacity && CountCells(CellKind.GasReservoir) == 0)
            {
                AppendAlert(builder, "no gas reservoir");
            }
            else if (gasPumps > 0 && gasPipeTiles == 0)
            {
                AppendAlert(builder, "unconnected gas pump");
            }
            else if (gasVents > 0 && gasPipeTiles == 0)
            {
                AppendAlert(builder, "unconnected gas vent");
            }

            if (food < workers.Count * 700f)
            {
                AppendAlert(builder, "low food");
            }

            if (food >= workers.Count * 700f && CountCells(CellKind.MessTable) < workers.Count)
            {
                AppendAlert(builder, "not enough mess tables");
            }

            if (food > workers.Count * 900f && CountCells(CellKind.Refrigerator) == 0)
            {
                AppendAlert(builder, "no refrigerator");
            }
            else if (foodFreshness < 0.35f)
            {
                AppendAlert(builder, "food spoiling");
            }

            if (foodPoisoningCases > 0)
            {
                AppendAlert(builder, foodPoisoningCases + " food poison");
            }
            else if (staleMealsEaten > 0)
            {
                AppendAlert(builder, staleMealsEaten + " stale meals");
            }

            if (!techFoodPreparation && CountCells(CellKind.ResearchStation) > 0)
            {
                AppendAlert(builder, "research pending");
            }

            if (IsRestTime() && CountCells(CellKind.Bed) < workers.Count)
            {
                AppendAlert(builder, "not enough beds");
            }

            if (CountCells(CellKind.Outhouse) == 0)
            {
                AppendAlert(builder, "no outhouse");
            }

            if (AnyWorkerNeedsTreatment() && CountCells(CellKind.MedicalCot) == 0)
            {
                AppendAlert(builder, "no medical cot");
            }

            if (AnyWorkerNeedsRelaxation() && CountCells(CellKind.MassageTable) == 0)
            {
                AppendAlert(builder, "no stress relief");
            }

            if (HighestStress() > 45f && AverageWorkerDecor() < 0.18f)
            {
                AppendAlert(builder, "low decor");
            }

            foreach (Worker worker in workers)
            {
                if (worker.StressBreakSeconds > 0f)
                {
                    AppendAlert(builder, worker.Name + " stress break");
                    break;
                }

                if (worker.Bladder > 88f)
                {
                    AppendAlert(builder, worker.Name + " needs toilet");
                    break;
                }

                if (worker.Fatigue > 88f)
                {
                    AppendAlert(builder, worker.Name + " exhausted");
                    break;
                }

                if (worker.Sickness > 45f)
                {
                    AppendAlert(builder, worker.Name + " sick");
                    break;
                }

                if (worker.Health < 45f)
                {
                    AppendAlert(builder, worker.Name + " injured");
                    break;
                }

                if (worker.Stress > 80f)
                {
                    AppendAlert(builder, worker.Name + " stressed");
                    break;
                }
            }

            return builder.Length == 0 ? "Stable." : builder.ToString();
        }

        private void AppendAlert(StringBuilder builder, string alert)
        {
            if (builder.Length > 0)
            {
                builder.Append(", ");
            }

            builder.Append(alert);
        }

        private void TriggerColonyFailure(string reason)
        {
            if (colonyFailed)
            {
                return;
            }

            colonyFailed = true;
            paused = true;
            Log("Colony failed: " + reason);
            UpdateColonyStatus(true);
        }

        private int CountCells(CellKind kind)
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] == kind)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountClosedAirlocks()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] == CellKind.ManualAirlock && !airlockOpen[x, y])
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountMoppableSpills()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (IsMoppableSpill(new Vector2Int(x, y)))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountDamagedEquipment()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (IsRepairableEquipment(cells[x, y]) && equipmentCondition[x, y] < 0.995f)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountBrokenEquipment()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (IsBrokenEquipment(new Vector2Int(x, y)))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountOverheatingEquipment()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (IsRepairableEquipment(cells[x, y]) &&
                        !IsBrokenEquipment(new Vector2Int(x, y)) &&
                        EquipmentOverheatSeverity(x, y) > 0.001f)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountLooseResourceTiles()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (looseResourceKind[x, y] != LooseResourceKind.None && looseResourceAmount[x, y] > 0.05f)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private float TotalLooseResources()
        {
            float total = 0f;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (looseResourceKind[x, y] != LooseResourceKind.None)
                    {
                        total += Mathf.Max(0f, looseResourceAmount[x, y]);
                    }
                }
            }

            return total;
        }

        private int CountPowerWireTiles()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (powerWire[x, y])
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountShippingRailTiles()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (shippingRail[x, y])
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private float TotalShippingRailMass()
        {
            float total = 0f;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (shippingRail[x, y])
                    {
                        total += shippingRailAmount[x, y];
                    }
                }
            }

            return total;
        }

        private bool HasShippingRailAccess(Vector2Int cell)
        {
            return IsShippingRailCell(cell.x, cell.y) ||
                IsShippingRailCell(cell.x + 1, cell.y) ||
                IsShippingRailCell(cell.x - 1, cell.y) ||
                IsShippingRailCell(cell.x, cell.y + 1) ||
                IsShippingRailCell(cell.x, cell.y - 1);
        }

        private bool IsShippingRailCell(int x, int y)
        {
            return IsInside(x, y) && shippingRail[x, y];
        }

        private int CountAutomationWireTiles()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (automationWire[x, y])
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountLiquidPipeTiles()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (liquidPipe[x, y])
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountUnsafeLiquidPipes()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (liquidPipe[x, y] &&
                        pipeWater[x, y] > PipePhaseRuptureMinimumMass &&
                        (temperature[x, y] < PipeFreezeTemperature || temperature[x, y] > PipeBoilTemperature))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountUnsafeLiquidReservoirs()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] == CellKind.LiquidReservoir &&
                        liquidReservoirWater[x, y] > PipePhaseRuptureMinimumMass &&
                        (temperature[x, y] < PipeFreezeTemperature || temperature[x, y] > PipeBoilTemperature))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private float TotalPipeWater()
        {
            float total = 0f;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (liquidPipe[x, y])
                    {
                        total += pipeWater[x, y];
                    }
                }
            }

            return total;
        }

        private float TotalLiquidReservoirWater()
        {
            float total = 0f;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] == CellKind.LiquidReservoir)
                    {
                        total += liquidReservoirWater[x, y];
                    }
                }
            }

            return total;
        }

        private int CountGasPipeTiles()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (gasPipe[x, y])
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private float TotalGasPipeMass()
        {
            float total = 0f;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (gasPipe[x, y])
                    {
                        total += GasPipeTotal(x, y);
                    }
                }
            }

            return total;
        }

        private float TotalGasReservoirMass()
        {
            float total = 0f;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (cells[x, y] == CellKind.GasReservoir)
                    {
                        total += GasReservoirTotal(x, y);
                    }
                }
            }

            return total;
        }

        private float TotalReservoirMass()
        {
            return TotalLiquidReservoirWater() + TotalGasReservoirMass();
        }

        private float TotalMixedHydrogenPipeGas()
        {
            float total = 0f;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (gasPipe[x, y] && gasPipeHydrogen[x, y] > 0.01f)
                    {
                        total += PipeNonHydrogenTotal(x, y);
                    }
                }
            }

            return total;
        }

        private float TileGasTotal(int x, int y)
        {
            return oxygen[x, y] + carbonDioxide[x, y] + pollutedOxygen[x, y] + hydrogen[x, y] + steam[x, y] + chlorine[x, y] + naturalGas[x, y];
        }

        private float TilePumpableGasTotal(int x, int y)
        {
            return oxygen[x, y] + carbonDioxide[x, y] + pollutedOxygen[x, y] + hydrogen[x, y] + chlorine[x, y] + naturalGas[x, y];
        }

        private int CountHighPressureTiles()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (IsPassable(x, y) && TileGasTotal(x, y) > OverpressureStressThreshold)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountDangerousPressureTiles()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (IsPassable(x, y) && TileGasTotal(x, y) > OverpressureDamageThreshold)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountSteamTiles()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (IsPassable(x, y) && steam[x, y] > 0.02f)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountChlorineTiles()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (IsPassable(x, y) && chlorine[x, y] > ChlorineExposureThreshold)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountNaturalGasTiles()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (IsPassable(x, y) && naturalGas[x, y] > 0.08f)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private float GasPipeTotal(int x, int y)
        {
            return gasPipeOxygen[x, y] + gasPipeCarbonDioxide[x, y] + gasPipePollutedOxygen[x, y] + gasPipeHydrogen[x, y] + gasPipeChlorine[x, y] + gasPipeNaturalGas[x, y];
        }

        private float GasReservoirTotal(int x, int y)
        {
            return gasReservoirOxygen[x, y] + gasReservoirCarbonDioxide[x, y] + gasReservoirPollutedOxygen[x, y] + gasReservoirHydrogen[x, y] + gasReservoirChlorine[x, y] + gasReservoirNaturalGas[x, y];
        }

        private float PipeNonHydrogenTotal(int x, int y)
        {
            return gasPipeOxygen[x, y] + gasPipeCarbonDioxide[x, y] + gasPipePollutedOxygen[x, y] + gasPipeChlorine[x, y] + gasPipeNaturalGas[x, y];
        }

        private float PipeHydrogenPurity(int x, int y)
        {
            float total = GasPipeTotal(x, y);
            return total <= 0.001f ? 0f : gasPipeHydrogen[x, y] / total;
        }

        private float DryResourceAmount()
        {
            return dirt + metal + algae + coal + refinedMetal + pollutedDirt;
        }

        private float DryResourceCapacity()
        {
            return BaseDryResourceCapacity + CountCells(CellKind.StorageBin) * StorageBinCapacity;
        }

        private float DryResourceFreeSpace()
        {
            return Mathf.Max(0f, DryResourceCapacity() - DryResourceAmount());
        }

        private int CountUnwiredPowerBuildings()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (RequiresPower(cells[x, y]) && !HasWireAccess(new Vector2Int(x, y)))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountAutomationControlledGenerators()
        {
            UpdateAutomationWires();
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if ((cells[x, y] == CellKind.ManualGenerator || cells[x, y] == CellKind.CoalGenerator || cells[x, y] == CellKind.HydrogenGenerator || cells[x, y] == CellKind.NaturalGasGenerator || cells[x, y] == CellKind.SteamTurbine || cells[x, y] == CellKind.SolarPanel) && HasAutomationControl(new Vector2Int(x, y)))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountAutomationControlledConduitShutoffs()
        {
            UpdateAutomationWires();
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if ((cells[x, y] == CellKind.LiquidShutoff || cells[x, y] == CellKind.GasShutoff) &&
                        HasCachedAutomationControl(new Vector2Int(x, y)))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountPoweredBuildings()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (RequiresPower(cells[x, y]) && CanPoweredMachineRun(new Vector2Int(x, y)))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountActiveWorkers()
        {
            int count = 0;
            foreach (Worker worker in workers)
            {
                if (worker.Health > 0f)
                {
                    count++;
                }
            }

            return count;
        }

        private float HighestStress()
        {
            float highest = 0f;
            foreach (Worker worker in workers)
            {
                if (worker.Health > 0f)
                {
                    highest = Mathf.Max(highest, worker.Stress);
                }
            }

            return highest;
        }

        private int HighestWorkerSkillLevel()
        {
            int highest = 1;
            foreach (Worker worker in workers)
            {
                if (worker.Health > 0f)
                {
                    highest = Mathf.Max(highest, WorkerSkillLevel(worker));
                }
            }

            return highest;
        }

        private void SaveGame(bool autosave)
        {
            try
            {
                string directory = Path.GetDirectoryName(SaveFilePath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                SaveData data = CaptureSaveData();
                File.WriteAllText(SaveFilePath, JsonUtility.ToJson(data, true));
                Log(autosave ? "Autosaved colony." : "Game saved.");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                Log("Save failed: " + exception.Message);
            }
        }

        private void LoadGame(bool silent)
        {
            try
            {
                if (!File.Exists(SaveFilePath))
                {
                    if (!silent)
                    {
                        Log("No save file found.");
                    }

                    return;
                }

                SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(SaveFilePath));
                if (data == null || data.width != WorldWidth || data.height != WorldHeight || data.cells == null)
                {
                    Log("Save file is incompatible.");
                    return;
                }

                ApplySaveData(data);
                Log("Game loaded.");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                Log("Load failed: " + exception.Message);
            }
        }

        private SaveData CaptureSaveData()
        {
            EnsureWorldState();
            EnsureWorkerRecords();
            SaveData data = new SaveData
            {
                version = SaveVersion,
                width = WorldWidth,
                height = WorldHeight,
                cells = new int[WorldWidth * WorldHeight],
                oxygen = new float[WorldWidth * WorldHeight],
                carbonDioxide = new float[WorldWidth * WorldHeight],
                pollutedOxygen = new float[WorldWidth * WorldHeight],
                hydrogen = new float[WorldWidth * WorldHeight],
                steam = new float[WorldWidth * WorldHeight],
                chlorine = new float[WorldWidth * WorldHeight],
                naturalGas = new float[WorldWidth * WorldHeight],
                germs = new float[WorldWidth * WorldHeight],
                plantGrowth = new float[WorldWidth * WorldHeight],
                cropTendedSeconds = new float[WorldWidth * WorldHeight],
                cropStress = new float[WorldWidth * WorldHeight],
                waterMass = new float[WorldWidth * WorldHeight],
                temperature = new float[WorldWidth * WorldHeight],
                equipmentCondition = new float[WorldWidth * WorldHeight],
                looseResourceKind = new int[WorldWidth * WorldHeight],
                looseResourceAmount = new float[WorldWidth * WorldHeight],
                powerWire = new bool[WorldWidth * WorldHeight],
                automationWire = new bool[WorldWidth * WorldHeight],
                automationSwitchState = new bool[WorldWidth * WorldHeight],
                airlockOpen = new bool[WorldWidth * WorldHeight],
                shippingRail = new bool[WorldWidth * WorldHeight],
                shippingRailKind = new int[WorldWidth * WorldHeight],
                shippingRailAmount = new float[WorldWidth * WorldHeight],
                liquidPipe = new bool[WorldWidth * WorldHeight],
                pipeWater = new float[WorldWidth * WorldHeight],
                liquidReservoirWater = new float[WorldWidth * WorldHeight],
                gasPipe = new bool[WorldWidth * WorldHeight],
                gasPipeOxygen = new float[WorldWidth * WorldHeight],
                gasPipeCarbonDioxide = new float[WorldWidth * WorldHeight],
                gasPipePollutedOxygen = new float[WorldWidth * WorldHeight],
                gasPipeHydrogen = new float[WorldWidth * WorldHeight],
                gasPipeChlorine = new float[WorldWidth * WorldHeight],
                gasPipeNaturalGas = new float[WorldWidth * WorldHeight],
                gasPipeGerms = new float[WorldWidth * WorldHeight],
                gasReservoirOxygen = new float[WorldWidth * WorldHeight],
                gasReservoirCarbonDioxide = new float[WorldWidth * WorldHeight],
                gasReservoirPollutedOxygen = new float[WorldWidth * WorldHeight],
                gasReservoirHydrogen = new float[WorldWidth * WorldHeight],
                gasReservoirChlorine = new float[WorldWidth * WorldHeight],
                gasReservoirNaturalGas = new float[WorldWidth * WorldHeight],
                gasReservoirGerms = new float[WorldWidth * WorldHeight],
                workers = new WorkerSave[workers.Count],
                hatches = new HatchSave[hatches.Count],
                jobs = new JobSave[jobs.Count],
                dirt = dirt,
                metal = metal,
                algae = algae,
                coal = coal,
                refinedMetal = refinedMetal,
                suitOxygen = suitOxygen,
                suitOxygenUsed = suitOxygenUsed,
                suitCheckpointUses = suitCheckpointUses,
                suitEntryDenials = suitEntryDenials,
                sandFalls = sandFalls,
                sandStrikeInjuries = sandStrikeInjuries,
                liquidFlowedMass = liquidFlowedMass,
                liquidFlowEvents = liquidFlowEvents,
                pipeBurstWater = pipeBurstWater,
                pipeBurstEvents = pipeBurstEvents,
                frozenPipeBursts = frozenPipeBursts,
                boiledPipeBursts = boiledPipeBursts,
                reservoirBurstWater = reservoirBurstWater,
                reservoirBurstEvents = reservoirBurstEvents,
                frozenReservoirBursts = frozenReservoirBursts,
                boiledReservoirBursts = boiledReservoirBursts,
                iceMeltedTiles = iceMeltedTiles,
                waterFrozenTiles = waterFrozenTiles,
                steamEvaporatedMass = steamEvaporatedMass,
                steamCondensedMass = steamCondensedMass,
                chlorineSterilizedGerms = chlorineSterilizedGerms,
                chlorineExposureSeconds = chlorineExposureSeconds,
                chlorineHealthDamage = chlorineHealthDamage,
                submergedEquipmentDamage = submergedEquipmentDamage,
                floodedWireFailures = floodedWireFailures,
                overheatedEquipmentDamage = overheatedEquipmentDamage,
                overheatedEquipmentFailures = overheatedEquipmentFailures,
                overpressureExposureSeconds = overpressureExposureSeconds,
                overpressureHealthDamage = overpressureHealthDamage,
                thermalExposureSeconds = thermalExposureSeconds,
                thermalHealthDamage = thermalHealthDamage,
                heatStrokeCases = heatStrokeCases,
                hypothermiaCases = hypothermiaCases,
                moralePressureSeconds = moralePressureSeconds,
                moraleStressAdded = moraleStressAdded,
                staleMealsEaten = staleMealsEaten,
                foodPoisoningCases = foodPoisoningCases,
                printingPodProgress = printingPodProgress,
                water = water,
                pollutedWater = pollutedWater,
                pollutedWaterOffgassedMass = pollutedWaterOffgassedMass,
                pollutedWaterOffgasEvents = pollutedWaterOffgasEvents,
                pollutedDirt = pollutedDirt,
                recycledWater = recycledWater,
                researchPoints = researchPoints,
                food = food,
                foodFreshness = foodFreshness,
                power = power,
                maxPower = maxPower,
                elapsedTime = elapsedTime,
                cycleTimer = cycleTimer,
                sleepStartCycleTime = sleepStartCycleTime,
                sleepEndCycleTime = sleepEndCycleTime,
                cycle = cycle,
                currentMode = (int)currentMode,
                currentOverlayMode = (int)currentOverlayMode,
                language = currentLanguage.ToString(),
                milestoneBasicShelter = milestoneBasicShelter,
                milestoneStableOxygen = milestoneStableOxygen,
                milestoneFoodProduction = milestoneFoodProduction,
                milestonePowerBuffer = milestonePowerBuffer,
                milestoneCycleFive = milestoneCycleFive,
                milestoneResearchProgram = milestoneResearchProgram,
                milestoneWaterSupply = milestoneWaterSupply,
                milestoneFoodPreparation = milestoneFoodPreparation,
                milestoneThermalControl = milestoneThermalControl,
                milestonePowerGrid = milestonePowerGrid,
                milestoneSanitation = milestoneSanitation,
                milestoneMoraleCare = milestoneMoraleCare,
                milestonePressureControl = milestonePressureControl,
                milestoneAirlockControl = milestoneAirlockControl,
                milestoneFoodStorage = milestoneFoodStorage,
                milestoneMaterialStorage = milestoneMaterialStorage,
                milestonePlumbing = milestonePlumbing,
                milestoneVentilation = milestoneVentilation,
                milestoneAdvancedAtmosphere = milestoneAdvancedAtmosphere,
                milestoneWaterRecycling = milestoneWaterRecycling,
                milestoneDining = milestoneDining,
                milestoneSkilledLabor = milestoneSkilledLabor,
                milestoneDecorComfort = milestoneDecorComfort,
                milestoneWasteProcessing = milestoneWasteProcessing,
                milestoneAutomation = milestoneAutomation,
                milestoneFuelPower = milestoneFuelPower,
                milestoneMetalRefining = milestoneMetalRefining,
                milestoneAtmoSuits = milestoneAtmoSuits,
                milestoneInsulation = milestoneInsulation,
                milestoneRoomPlanning = milestoneRoomPlanning,
                milestoneReconfiguration = milestoneReconfiguration,
                milestoneColonyExpansion = milestoneColonyExpansion,
                milestoneSpillCleanup = milestoneSpillCleanup,
                milestoneMaintenance = milestoneMaintenance,
                milestoneEmergencyResponse = milestoneEmergencyResponse,
                milestoneResourceLogistics = milestoneResourceLogistics,
                milestoneHydrogenPower = milestoneHydrogenPower,
                milestoneHydrogenFiltering = milestoneHydrogenFiltering,
                milestoneReservoirBuffering = milestoneReservoirBuffering,
                milestoneConduitAutomation = milestoneConduitAutomation,
                milestoneRenewableVents = milestoneRenewableVents,
                milestoneRanching = milestoneRanching,
                milestonePowerLoadManagement = milestonePowerLoadManagement,
                milestoneHygiene = milestoneHygiene,
                milestoneCropTending = milestoneCropTending,
                milestoneAutoSweeping = milestoneAutoSweeping,
                milestoneShippingLogistics = milestoneShippingLogistics,
                milestoneBottleEmptying = milestoneBottleEmptying,
                milestoneSignalSwitching = milestoneSignalSwitching,
                milestoneSteamPower = milestoneSteamPower,
                milestoneSolarPower = milestoneSolarPower,
                milestoneMeteorShielding = milestoneMeteorShielding,
                milestoneSpaceScanning = milestoneSpaceScanning,
                mealsEatenAtTable = mealsEatenAtTable,
                compostedPollutedDirt = compostedPollutedDirt,
                coalPowerGenerated = coalPowerGenerated,
                refinedMetalProduced = refinedMetalProduced,
                deconstructionsCompleted = deconstructionsCompleted,
                duplicantsPrinted = duplicantsPrinted,
                moppedLiquid = moppedLiquid,
                repairsCompleted = repairsCompleted,
                equipmentFailures = equipmentFailures,
                rescuesCompleted = rescuesCompleted,
                sweptResources = sweptResources,
                hydrogenPowerGenerated = hydrogenPowerGenerated,
                naturalGasPowerGenerated = naturalGasPowerGenerated,
                steamTurbinePowerGenerated = steamTurbinePowerGenerated,
                steamTurbineWaterRecovered = steamTurbineWaterRecovered,
                solarPowerGenerated = solarPowerGenerated,
                solarBlockedSeconds = solarBlockedSeconds,
                meteorShowerSeconds = meteorShowerSeconds,
                meteorCooldownSeconds = meteorCooldownSeconds,
                meteorStrikeTimer = meteorStrikeTimer,
                meteorStrikes = meteorStrikes,
                meteorImpactsBlocked = meteorImpactsBlocked,
                meteorDamageEvents = meteorDamageEvents,
                meteorRegolithDeposited = meteorRegolithDeposited,
                spaceScannerSignalSeconds = spaceScannerSignalSeconds,
                spaceScannerBlockedSeconds = spaceScannerBlockedSeconds,
                hydrogenFilteredGas = hydrogenFilteredGas,
                reservoirBufferedMass = reservoirBufferedMass,
                automatedConduitFlow = automatedConduitFlow,
                renewableWaterGenerated = renewableWaterGenerated,
                renewableHydrogenGenerated = renewableHydrogenGenerated,
                renewableNaturalGasGenerated = renewableNaturalGasGenerated,
                hatchCoalProduced = hatchCoalProduced,
                hatchesGroomed = hatchesGroomed,
                transformedPowerDelivered = transformedPowerDelivered,
                overloadedWireSeconds = overloadedWireSeconds,
                handsWashed = handsWashed,
                cropsTended = cropsTended,
                cropStifledSeconds = cropStifledSeconds,
                cropsWilted = cropsWilted,
                autoSweptResources = autoSweptResources,
                conveyorShippedResources = conveyorShippedResources,
                bottleEmptiedLiquid = bottleEmptiedLiquid,
                signalSwitchesToggled = signalSwitchesToggled,
                airlockToggles = airlockToggles,
                techAirSystems = techAirSystems,
                techFoodPreparation = techFoodPreparation,
                techPowerRegulation = techPowerRegulation,
                colonyVictory = colonyVictory,
                colonyFailed = colonyFailed,
                lastLog = lastLog
            };

            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    int index = Key(x, y);
                    data.cells[index] = (int)cells[x, y];
                    data.oxygen[index] = oxygen[x, y];
                    data.carbonDioxide[index] = carbonDioxide[x, y];
                    data.pollutedOxygen[index] = pollutedOxygen[x, y];
                    data.hydrogen[index] = hydrogen[x, y];
                    data.steam[index] = steam[x, y];
                    data.chlorine[index] = chlorine[x, y];
                    data.naturalGas[index] = naturalGas[x, y];
                    data.germs[index] = germs[x, y];
                    data.plantGrowth[index] = plantGrowth[x, y];
                    data.cropTendedSeconds[index] = cells[x, y] == CellKind.Planter ? Mathf.Max(0f, cropTendedSeconds[x, y]) : 0f;
                    data.cropStress[index] = cells[x, y] == CellKind.Planter ? Mathf.Max(0f, cropStress[x, y]) : 0f;
                    data.waterMass[index] = waterMass[x, y];
                    data.temperature[index] = temperature[x, y];
                    data.equipmentCondition[index] = IsRepairableEquipment(cells[x, y]) ? Mathf.Clamp01(equipmentCondition[x, y]) : 0f;
                    data.looseResourceKind[index] = (int)looseResourceKind[x, y];
                    data.looseResourceAmount[index] = looseResourceKind[x, y] == LooseResourceKind.None ? 0f : Mathf.Max(0f, looseResourceAmount[x, y]);
                    data.powerWire[index] = powerWire[x, y];
                    data.automationWire[index] = automationWire[x, y];
                    data.automationSwitchState[index] = cells[x, y] == CellKind.SignalSwitch && automationSwitchState[x, y];
                    data.airlockOpen[index] = cells[x, y] == CellKind.ManualAirlock && airlockOpen[x, y];
                    data.shippingRail[index] = shippingRail[x, y];
                    data.shippingRailKind[index] = (int)shippingRailKind[x, y];
                    data.shippingRailAmount[index] = shippingRail[x, y] && shippingRailKind[x, y] != LooseResourceKind.None ? Mathf.Max(0f, shippingRailAmount[x, y]) : 0f;
                    data.liquidPipe[index] = liquidPipe[x, y];
                    data.pipeWater[index] = pipeWater[x, y];
                    data.liquidReservoirWater[index] = liquidReservoirWater[x, y];
                    data.gasPipe[index] = gasPipe[x, y];
                    data.gasPipeOxygen[index] = gasPipeOxygen[x, y];
                    data.gasPipeCarbonDioxide[index] = gasPipeCarbonDioxide[x, y];
                    data.gasPipePollutedOxygen[index] = gasPipePollutedOxygen[x, y];
                    data.gasPipeHydrogen[index] = gasPipeHydrogen[x, y];
                    data.gasPipeChlorine[index] = gasPipeChlorine[x, y];
                    data.gasPipeNaturalGas[index] = gasPipeNaturalGas[x, y];
                    data.gasPipeGerms[index] = gasPipeGerms[x, y];
                    data.gasReservoirOxygen[index] = gasReservoirOxygen[x, y];
                    data.gasReservoirCarbonDioxide[index] = gasReservoirCarbonDioxide[x, y];
                    data.gasReservoirPollutedOxygen[index] = gasReservoirPollutedOxygen[x, y];
                    data.gasReservoirHydrogen[index] = gasReservoirHydrogen[x, y];
                    data.gasReservoirChlorine[index] = gasReservoirChlorine[x, y];
                    data.gasReservoirNaturalGas[index] = gasReservoirNaturalGas[x, y];
                    data.gasReservoirGerms[index] = gasReservoirGerms[x, y];
                }
            }

            for (int i = 0; i < workers.Count; i++)
            {
                Worker worker = workers[i];
                Vector2Int cell = WorldToCell(worker.Transform.position);
                data.workers[i] = new WorkerSave
                {
                    name = worker.Name,
                    cellX = cell.x,
                    cellY = cell.y,
                    positionX = worker.Transform.position.x,
                    positionY = worker.Transform.position.y,
                    calories = worker.Calories,
                    stress = worker.Stress,
                    health = worker.Health,
                    morale = worker.Morale,
                    fatigue = worker.Fatigue,
                    bladder = worker.Bladder,
                    germExposure = worker.GermExposure,
                    sickness = worker.Sickness,
                    heatExposure = worker.HeatExposure,
                    chillExposure = worker.ChillExposure,
                    stressBreakSeconds = worker.StressBreakSeconds,
                    incapacitatedSeconds = worker.IncapacitatedSeconds,
                    experience = worker.Experience,
                    suitEquipped = worker.SuitEquipped
                };
            }

            for (int i = 0; i < hatches.Count; i++)
            {
                HatchCritter hatch = hatches[i];
                Vector2Int cell = hatch.Transform == null ? hatch.Cell : WorldToCell(hatch.Transform.position);
                Vector3 position = hatch.Transform == null ? CellCenter(cell) : hatch.Transform.position;
                data.hatches[i] = new HatchSave
                {
                    name = hatch.Name,
                    cellX = cell.x,
                    cellY = cell.y,
                    positionX = position.x,
                    positionY = position.y,
                    moveTimer = hatch.MoveTimer,
                    eatTimer = hatch.EatTimer,
                    groomedSeconds = hatch.GroomedSeconds,
                    happiness = hatch.Happiness,
                    coalProduced = hatch.CoalProduced
                };
            }

            for (int i = 0; i < jobs.Count; i++)
            {
                Job job = jobs[i];
                data.jobs[i] = new JobSave
                {
                    type = (int)job.Type,
                    cellX = job.Cell.x,
                    cellY = job.Cell.y,
                    buildKind = (int)job.BuildKind,
                    workRequired = job.WorkRequired,
                    progress = job.Progress,
                    dirtCost = job.DirtCost,
                    metalCost = job.MetalCost,
                    algaeCost = job.AlgaeCost,
                    refinedMetalCost = job.RefinedMetalCost,
                    priority = job.Priority,
                    ageSeconds = job.AgeSeconds,
                    autoGenerated = job.AutoGenerated,
                    targetWorkerName = job.TargetWorkerName,
                    buildWire = job.BuildWire,
                    buildPipe = job.BuildPipe,
                    buildGasPipe = job.BuildGasPipe,
                    buildShippingRail = job.BuildShippingRail,
                    removePowerWire = job.RemovePowerWire,
                    removeAutomationWire = job.RemoveAutomationWire,
                    removeLiquidPipe = job.RemoveLiquidPipe,
                    removeGasPipe = job.RemoveGasPipe,
                    removeShippingRail = job.RemoveShippingRail
                };
            }

            return data;
        }

        private void ApplySaveData(SaveData data)
        {
            EnsureWorkerRecords();
            int cellKindMax = Enum.GetValues(typeof(CellKind)).Length - 1;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    int index = Key(x, y);
                    int cellValue = Mathf.Clamp(data.cells[index], 0, cellKindMax);
                    cells[x, y] = (CellKind)cellValue;
                    oxygen[x, y] = ReadSaveFloat(data.oxygen, index, 0f);
                    carbonDioxide[x, y] = ReadSaveFloat(data.carbonDioxide, index, 0f);
                    pollutedOxygen[x, y] = ReadSaveFloat(data.pollutedOxygen, index, 0f);
                    hydrogen[x, y] = data.version >= 37 ? ReadSaveFloat(data.hydrogen, index, 0f) : 0f;
                    steam[x, y] = data.version >= 61 ? Mathf.Clamp(ReadSaveFloat(data.steam, index, 0f), 0f, 2.8f) : 0f;
                    chlorine[x, y] = data.version >= 67 ? Mathf.Clamp(ReadSaveFloat(data.chlorine, index, 0f), 0f, 2.8f) : 0f;
                    naturalGas[x, y] = data.version >= 68 ? Mathf.Clamp(ReadSaveFloat(data.naturalGas, index, 0f), 0f, 2.8f) : 0f;
                    germs[x, y] = ReadSaveFloat(data.germs, index, cells[x, y] == CellKind.Slime ? 0.85f : 0f);
                    plantGrowth[x, y] = ReadSaveFloat(data.plantGrowth, index, 0f);
                    cropTendedSeconds[x, y] = data.version >= 46 && cells[x, y] == CellKind.Planter ? Mathf.Max(0f, ReadSaveFloat(data.cropTendedSeconds, index, 0f)) : 0f;
                    cropStress[x, y] = data.version >= 60 && cells[x, y] == CellKind.Planter ? Mathf.Max(0f, ReadSaveFloat(data.cropStress, index, 0f)) : 0f;
                    waterMass[x, y] = ReadSaveFloat(data.waterMass, index, cells[x, y] == CellKind.Water ? 80f : 0f);
                    temperature[x, y] = ReadSaveFloat(data.temperature, index, InitialTemperature(x, y, cells[x, y]));
                    equipmentCondition[x, y] = IsRepairableEquipment(cells[x, y])
                        ? Mathf.Clamp01(data.version >= 34 ? ReadSaveFloat(data.equipmentCondition, index, DefaultEquipmentCondition(cells[x, y])) : DefaultEquipmentCondition(cells[x, y]))
                        : 0f;
                    int looseMax = Enum.GetValues(typeof(LooseResourceKind)).Length - 1;
                    looseResourceKind[x, y] = data.version >= 36 ? (LooseResourceKind)Mathf.Clamp(ReadSaveInt(data.looseResourceKind, index, 0), 0, looseMax) : LooseResourceKind.None;
                    looseResourceAmount[x, y] = looseResourceKind[x, y] == LooseResourceKind.None ? 0f : Mathf.Max(0f, ReadSaveFloat(data.looseResourceAmount, index, 0f));
                    powerWire[x, y] = ReadSaveBool(data.powerWire, index, false);
                    poweredWire[x, y] = false;
                    wireLoad[x, y] = 0f;
                    overloadedWire[x, y] = false;
                    wireOverloadStress[x, y] = 0f;
                    automationWire[x, y] = data.version >= 25 && ReadSaveBool(data.automationWire, index, false);
                    automationControlledWire[x, y] = false;
                    automationSignalWire[x, y] = false;
                    automationSwitchState[x, y] = data.version >= 50 && cells[x, y] == CellKind.SignalSwitch && ReadSaveBool(data.automationSwitchState, index, false);
                    airlockOpen[x, y] = cells[x, y] == CellKind.ManualAirlock && (data.version < 51 || ReadSaveBool(data.airlockOpen, index, true));
                    shippingRail[x, y] = data.version >= 48 && ReadSaveBool(data.shippingRail, index, false);
                    shippingRailKind[x, y] = shippingRail[x, y] ? (LooseResourceKind)Mathf.Clamp(ReadSaveInt(data.shippingRailKind, index, 0), 0, looseMax) : LooseResourceKind.None;
                    shippingRailAmount[x, y] = shippingRail[x, y] && shippingRailKind[x, y] != LooseResourceKind.None ? Mathf.Clamp(ReadSaveFloat(data.shippingRailAmount, index, 0f), 0f, ShippingRailCapacity) : 0f;
                    liquidPipe[x, y] = ReadSaveBool(data.liquidPipe, index, false);
                    pipeWater[x, y] = liquidPipe[x, y] ? Mathf.Clamp(ReadSaveFloat(data.pipeWater, index, 0f), 0f, LiquidPipeCapacity) : 0f;
                    liquidReservoirWater[x, y] = data.version >= 40 && cells[x, y] == CellKind.LiquidReservoir ? Mathf.Clamp(ReadSaveFloat(data.liquidReservoirWater, index, 0f), 0f, LiquidReservoirCapacity) : 0f;
                    gasPipe[x, y] = ReadSaveBool(data.gasPipe, index, false);
                    gasPipeOxygen[x, y] = gasPipe[x, y] ? Mathf.Clamp(ReadSaveFloat(data.gasPipeOxygen, index, 0f), 0f, GasPipeCapacity) : 0f;
                    gasPipeCarbonDioxide[x, y] = gasPipe[x, y] ? Mathf.Clamp(ReadSaveFloat(data.gasPipeCarbonDioxide, index, 0f), 0f, GasPipeCapacity) : 0f;
                    gasPipePollutedOxygen[x, y] = gasPipe[x, y] ? Mathf.Clamp(ReadSaveFloat(data.gasPipePollutedOxygen, index, 0f), 0f, GasPipeCapacity) : 0f;
                    gasPipeHydrogen[x, y] = gasPipe[x, y] && data.version >= 37 ? Mathf.Clamp(ReadSaveFloat(data.gasPipeHydrogen, index, 0f), 0f, GasPipeCapacity) : 0f;
                    gasPipeChlorine[x, y] = gasPipe[x, y] && data.version >= 67 ? Mathf.Clamp(ReadSaveFloat(data.gasPipeChlorine, index, 0f), 0f, GasPipeCapacity) : 0f;
                    gasPipeNaturalGas[x, y] = gasPipe[x, y] && data.version >= 68 ? Mathf.Clamp(ReadSaveFloat(data.gasPipeNaturalGas, index, 0f), 0f, GasPipeCapacity) : 0f;
                    gasPipeGerms[x, y] = gasPipe[x, y] ? Mathf.Clamp01(ReadSaveFloat(data.gasPipeGerms, index, 0f)) : 0f;
                    bool isGasReservoir = data.version >= 40 && cells[x, y] == CellKind.GasReservoir;
                    gasReservoirOxygen[x, y] = isGasReservoir ? Mathf.Clamp(ReadSaveFloat(data.gasReservoirOxygen, index, 0f), 0f, GasReservoirCapacity) : 0f;
                    gasReservoirCarbonDioxide[x, y] = isGasReservoir ? Mathf.Clamp(ReadSaveFloat(data.gasReservoirCarbonDioxide, index, 0f), 0f, GasReservoirCapacity) : 0f;
                    gasReservoirPollutedOxygen[x, y] = isGasReservoir ? Mathf.Clamp(ReadSaveFloat(data.gasReservoirPollutedOxygen, index, 0f), 0f, GasReservoirCapacity) : 0f;
                    gasReservoirHydrogen[x, y] = isGasReservoir ? Mathf.Clamp(ReadSaveFloat(data.gasReservoirHydrogen, index, 0f), 0f, GasReservoirCapacity) : 0f;
                    gasReservoirChlorine[x, y] = isGasReservoir && data.version >= 67 ? Mathf.Clamp(ReadSaveFloat(data.gasReservoirChlorine, index, 0f), 0f, GasReservoirCapacity) : 0f;
                    gasReservoirNaturalGas[x, y] = isGasReservoir && data.version >= 68 ? Mathf.Clamp(ReadSaveFloat(data.gasReservoirNaturalGas, index, 0f), 0f, GasReservoirCapacity) : 0f;
                    gasReservoirGerms[x, y] = isGasReservoir ? Mathf.Clamp01(ReadSaveFloat(data.gasReservoirGerms, index, 0f)) : 0f;
                }
            }

            if (data.version < 3)
            {
                SeedWaterPool(7, 4, 14, 7, 85f);
                SeedWaterPool(61, 5, 71, 9, 100f);
                SeedWaterPool(18, 11, 23, 13, 60f);
            }

            if (data.version < 5 && CountCells(CellKind.Slime) == 0)
            {
                SeedSlimePockets();
            }

            if (data.version < 7 && CountCells(CellKind.Ice) == 0)
            {
                SeedIcePockets();
                for (int y = 0; y < WorldHeight; y++)
                {
                    for (int x = 0; x < WorldWidth; x++)
                    {
                        if (cells[x, y] == CellKind.Ice)
                        {
                            temperature[x, y] = InitialTemperature(x, y, CellKind.Ice);
                        }
                    }
                }
            }

            if (data.version < 8 && CountPowerWireTiles() == 0)
            {
                SeedStarterPowerWire();
            }

            if (data.version < 11 && CountCells(CellKind.Outhouse) == 0)
            {
                SeedStarterOuthouse();
            }

            if (data.version < 26 && CountCells(CellKind.Coal) == 0)
            {
                SeedCoalPockets();
            }

            if (data.version < 32 && CountCells(CellKind.PrintingPod) == 0)
            {
                SeedStarterPrintingPod();
            }

            if (data.version < 37)
            {
                SeedHydrogenPockets();
            }

            if (data.version < 67)
            {
                SeedChlorinePockets();
            }

            if (data.version < 68)
            {
                SeedNaturalGasPockets();
            }

            if (data.version < 42)
            {
                SeedNaturalVents();
            }
            else if (data.version < 68)
            {
                SeedNaturalVents();
            }

            dirt = data.dirt;
            metal = data.metal;
            algae = data.algae;
            coal = data.version >= 26 ? Mathf.Max(0f, data.coal) : 0f;
            refinedMetal = data.version >= 27 ? Mathf.Max(0f, data.refinedMetal) : 0f;
            suitOxygen = data.version >= 28 ? Mathf.Max(0f, data.suitOxygen) : 0f;
            suitOxygenUsed = data.version >= 28 ? Mathf.Max(0f, data.suitOxygenUsed) : 0f;
            suitCheckpointUses = data.version >= 39 ? Mathf.Max(0, data.suitCheckpointUses) : 0;
            suitEntryDenials = data.version >= 52 ? Mathf.Max(0, data.suitEntryDenials) : 0;
            sandFalls = data.version >= 53 ? Mathf.Max(0, data.sandFalls) : 0;
            sandStrikeInjuries = data.version >= 53 ? Mathf.Max(0, data.sandStrikeInjuries) : 0;
            liquidFlowedMass = data.version >= 54 ? Mathf.Max(0f, data.liquidFlowedMass) : 0f;
            liquidFlowEvents = data.version >= 54 ? Mathf.Max(0, data.liquidFlowEvents) : 0;
            pipeBurstWater = data.version >= 64 ? Mathf.Max(0f, data.pipeBurstWater) : 0f;
            pipeBurstEvents = data.version >= 64 ? Mathf.Max(0, data.pipeBurstEvents) : 0;
            frozenPipeBursts = data.version >= 64 ? Mathf.Max(0, data.frozenPipeBursts) : 0;
            boiledPipeBursts = data.version >= 64 ? Mathf.Max(0, data.boiledPipeBursts) : 0;
            reservoirBurstWater = data.version >= 65 ? Mathf.Max(0f, data.reservoirBurstWater) : 0f;
            reservoirBurstEvents = data.version >= 65 ? Mathf.Max(0, data.reservoirBurstEvents) : 0;
            frozenReservoirBursts = data.version >= 65 ? Mathf.Max(0, data.frozenReservoirBursts) : 0;
            boiledReservoirBursts = data.version >= 65 ? Mathf.Max(0, data.boiledReservoirBursts) : 0;
            iceMeltedTiles = data.version >= 55 ? Mathf.Max(0, data.iceMeltedTiles) : 0;
            waterFrozenTiles = data.version >= 55 ? Mathf.Max(0, data.waterFrozenTiles) : 0;
            steamEvaporatedMass = data.version >= 61 ? Mathf.Max(0f, data.steamEvaporatedMass) : 0f;
            steamCondensedMass = data.version >= 61 ? Mathf.Max(0f, data.steamCondensedMass) : 0f;
            chlorineSterilizedGerms = data.version >= 67 ? Mathf.Max(0f, data.chlorineSterilizedGerms) : 0f;
            chlorineExposureSeconds = data.version >= 67 ? Mathf.Max(0f, data.chlorineExposureSeconds) : 0f;
            chlorineHealthDamage = data.version >= 67 ? Mathf.Max(0f, data.chlorineHealthDamage) : 0f;
            submergedEquipmentDamage = data.version >= 56 ? Mathf.Max(0f, data.submergedEquipmentDamage) : 0f;
            floodedWireFailures = data.version >= 56 ? Mathf.Max(0, data.floodedWireFailures) : 0;
            overheatedEquipmentDamage = data.version >= 63 ? Mathf.Max(0f, data.overheatedEquipmentDamage) : 0f;
            overheatedEquipmentFailures = data.version >= 63 ? Mathf.Max(0, data.overheatedEquipmentFailures) : 0;
            overpressureExposureSeconds = data.version >= 57 ? Mathf.Max(0f, data.overpressureExposureSeconds) : 0f;
            overpressureHealthDamage = data.version >= 57 ? Mathf.Max(0f, data.overpressureHealthDamage) : 0f;
            thermalExposureSeconds = data.version >= 62 ? Mathf.Max(0f, data.thermalExposureSeconds) : 0f;
            thermalHealthDamage = data.version >= 62 ? Mathf.Max(0f, data.thermalHealthDamage) : 0f;
            heatStrokeCases = data.version >= 62 ? Mathf.Max(0, data.heatStrokeCases) : 0;
            hypothermiaCases = data.version >= 62 ? Mathf.Max(0, data.hypothermiaCases) : 0;
            moralePressureSeconds = data.version >= 58 ? Mathf.Max(0f, data.moralePressureSeconds) : 0f;
            moraleStressAdded = data.version >= 58 ? Mathf.Max(0f, data.moraleStressAdded) : 0f;
            staleMealsEaten = data.version >= 59 ? Mathf.Max(0, data.staleMealsEaten) : 0;
            foodPoisoningCases = data.version >= 59 ? Mathf.Max(0, data.foodPoisoningCases) : 0;
            suitOxygen = Mathf.Min(suitOxygen, SuitOxygenCapacityTotal());
            printingPodProgress = data.version >= 32 ? Mathf.Clamp01(data.printingPodProgress) : 0f;
            water = data.version < 3 && data.water <= 0f ? 45f : data.water;
            pollutedWater = data.version >= 19 ? Mathf.Max(0f, data.pollutedWater) : 0f;
            pollutedWaterOffgassedMass = data.version >= 66 ? Mathf.Max(0f, data.pollutedWaterOffgassedMass) : 0f;
            pollutedWaterOffgasEvents = data.version >= 66 ? Mathf.Max(0, data.pollutedWaterOffgasEvents) : 0;
            pollutedDirt = data.version >= 24 ? Mathf.Max(0f, data.pollutedDirt) : 0f;
            recycledWater = data.version >= 20 ? Mathf.Max(0f, data.recycledWater) : 0f;
            researchPoints = Mathf.Max(0f, data.researchPoints);
            food = data.food;
            foodFreshness = Mathf.Clamp01(data.version >= 15 ? data.foodFreshness : 0.82f);
            power = data.power;
            maxPower = data.maxPower > 0f ? data.maxPower : 100f;
            elapsedTime = data.elapsedTime;
            cycleTimer = data.cycleTimer;
            sleepStartCycleTime = data.version >= 10 ? NormalizeCycleTime(data.sleepStartCycleTime) : DefaultSleepStartCycleTime;
            sleepEndCycleTime = data.version >= 10 ? NormalizeCycleTime(data.sleepEndCycleTime) : DefaultSleepEndCycleTime;
            NormalizeSleepWindow();
            cycle = Mathf.Max(1, data.cycle);
            autosaveTimer = 0f;
            objectiveTimer = 0f;
            thermalTimer = 0f;
            liquidTimer = 0f;
            sandTimer = 0f;
            milestoneBasicShelter = data.milestoneBasicShelter;
            milestoneStableOxygen = data.milestoneStableOxygen;
            milestoneFoodProduction = data.milestoneFoodProduction;
            milestonePowerBuffer = data.milestonePowerBuffer;
            milestoneCycleFive = data.milestoneCycleFive;
            milestoneResearchProgram = data.milestoneResearchProgram;
            milestoneWaterSupply = data.milestoneWaterSupply;
            milestoneFoodPreparation = data.milestoneFoodPreparation;
            milestoneThermalControl = data.version >= 7 && data.milestoneThermalControl;
            milestonePowerGrid = data.version >= 8 && data.milestonePowerGrid;
            milestoneSanitation = data.version >= 11 && data.milestoneSanitation;
            milestoneMoraleCare = data.version >= 12 && data.milestoneMoraleCare;
            milestonePressureControl = data.version >= 14 && data.milestonePressureControl;
            milestoneFoodStorage = data.version >= 15 && data.milestoneFoodStorage;
            milestoneMaterialStorage = data.version >= 16 && data.milestoneMaterialStorage;
            milestonePlumbing = data.version >= 17 && data.milestonePlumbing;
            milestoneVentilation = data.version >= 18 && data.milestoneVentilation;
            milestoneAdvancedAtmosphere = data.version >= 19 && data.milestoneAdvancedAtmosphere;
            milestoneWaterRecycling = data.version >= 20 && data.milestoneWaterRecycling;
            milestoneDining = data.version >= 21 && data.milestoneDining;
            milestoneSkilledLabor = data.version >= 22 && data.milestoneSkilledLabor;
            milestoneDecorComfort = data.version >= 23 && data.milestoneDecorComfort;
            milestoneWasteProcessing = data.version >= 24 && data.milestoneWasteProcessing;
            milestoneAutomation = data.version >= 25 && data.milestoneAutomation;
            milestoneFuelPower = data.version >= 26 && data.milestoneFuelPower;
            milestoneMetalRefining = data.version >= 27 && data.milestoneMetalRefining;
            milestoneAtmoSuits = data.version >= 28 && data.milestoneAtmoSuits;
            milestoneInsulation = data.version >= 29 && data.milestoneInsulation;
            milestoneRoomPlanning = data.version >= 30 && data.milestoneRoomPlanning;
            milestoneReconfiguration = data.version >= 31 && data.milestoneReconfiguration;
            milestoneColonyExpansion = data.version >= 32 && data.milestoneColonyExpansion;
            milestoneSpillCleanup = data.version >= 33 && data.milestoneSpillCleanup;
            milestoneMaintenance = data.version >= 34 && data.milestoneMaintenance;
            milestoneEmergencyResponse = data.version >= 35 && data.milestoneEmergencyResponse;
            milestoneResourceLogistics = data.version >= 36 && data.milestoneResourceLogistics;
            milestoneHydrogenPower = data.version >= 37 && data.milestoneHydrogenPower;
            milestoneHydrogenFiltering = data.version >= 38 && data.milestoneHydrogenFiltering;
            milestoneReservoirBuffering = data.version >= 40 && data.milestoneReservoirBuffering;
            milestoneConduitAutomation = data.version >= 41 && data.milestoneConduitAutomation;
            milestoneRenewableVents = data.version >= 42 && data.milestoneRenewableVents;
            milestoneRanching = data.version >= 43 && data.milestoneRanching;
            milestonePowerLoadManagement = data.version >= 44 && data.milestonePowerLoadManagement;
            milestoneHygiene = data.version >= 45 && data.milestoneHygiene;
            milestoneCropTending = data.version >= 46 && data.milestoneCropTending;
            milestoneAutoSweeping = data.version >= 47 && data.milestoneAutoSweeping;
            milestoneShippingLogistics = data.version >= 48 && data.milestoneShippingLogistics;
            milestoneBottleEmptying = data.version >= 49 && data.milestoneBottleEmptying;
            milestoneSignalSwitching = data.version >= 50 && data.milestoneSignalSwitching;
            milestoneAirlockControl = data.version >= 51 && data.milestoneAirlockControl;
            milestoneSteamPower = data.version >= 69 && data.milestoneSteamPower;
            milestoneSolarPower = data.version >= 70 && data.milestoneSolarPower;
            milestoneMeteorShielding = data.version >= 71 && data.milestoneMeteorShielding;
            milestoneSpaceScanning = data.version >= 72 && data.milestoneSpaceScanning;
            mealsEatenAtTable = data.version >= 21 ? Mathf.Max(0, data.mealsEatenAtTable) : 0;
            compostedPollutedDirt = data.version >= 24 ? Mathf.Max(0f, data.compostedPollutedDirt) : 0f;
            coalPowerGenerated = data.version >= 26 ? Mathf.Max(0f, data.coalPowerGenerated) : 0f;
            refinedMetalProduced = data.version >= 27 ? Mathf.Max(0f, data.refinedMetalProduced) : 0f;
            deconstructionsCompleted = data.version >= 31 ? Mathf.Max(0, data.deconstructionsCompleted) : 0;
            duplicantsPrinted = data.version >= 32 ? Mathf.Max(0, data.duplicantsPrinted) : 0;
            moppedLiquid = data.version >= 33 ? Mathf.Max(0f, data.moppedLiquid) : 0f;
            repairsCompleted = data.version >= 34 ? Mathf.Max(0, data.repairsCompleted) : 0;
            equipmentFailures = data.version >= 34 ? Mathf.Max(0, data.equipmentFailures) : 0;
            rescuesCompleted = data.version >= 35 ? Mathf.Max(0, data.rescuesCompleted) : 0;
            sweptResources = data.version >= 36 ? Mathf.Max(0f, data.sweptResources) : 0f;
            hydrogenPowerGenerated = data.version >= 37 ? Mathf.Max(0f, data.hydrogenPowerGenerated) : 0f;
            naturalGasPowerGenerated = data.version >= 68 ? Mathf.Max(0f, data.naturalGasPowerGenerated) : 0f;
            steamTurbinePowerGenerated = data.version >= 69 ? Mathf.Max(0f, data.steamTurbinePowerGenerated) : 0f;
            steamTurbineWaterRecovered = data.version >= 69 ? Mathf.Max(0f, data.steamTurbineWaterRecovered) : 0f;
            solarPowerGenerated = data.version >= 70 ? Mathf.Max(0f, data.solarPowerGenerated) : 0f;
            solarBlockedSeconds = data.version >= 70 ? Mathf.Max(0f, data.solarBlockedSeconds) : 0f;
            meteorShowerSeconds = data.version >= 71 ? Mathf.Max(0f, data.meteorShowerSeconds) : 0f;
            meteorCooldownSeconds = data.version >= 71 ? Mathf.Max(0f, data.meteorCooldownSeconds) : MeteorInitialDelaySeconds;
            meteorStrikeTimer = data.version >= 71 ? Mathf.Max(0f, data.meteorStrikeTimer) : 0f;
            meteorStrikes = data.version >= 71 ? Mathf.Max(0, data.meteorStrikes) : 0;
            meteorImpactsBlocked = data.version >= 71 ? Mathf.Max(0, data.meteorImpactsBlocked) : 0;
            meteorDamageEvents = data.version >= 71 ? Mathf.Max(0, data.meteorDamageEvents) : 0;
            meteorRegolithDeposited = data.version >= 71 ? Mathf.Max(0f, data.meteorRegolithDeposited) : 0f;
            spaceScannerSignalSeconds = data.version >= 72 ? Mathf.Max(0f, data.spaceScannerSignalSeconds) : 0f;
            spaceScannerBlockedSeconds = data.version >= 72 ? Mathf.Max(0f, data.spaceScannerBlockedSeconds) : 0f;
            hydrogenFilteredGas = data.version >= 38 ? Mathf.Max(0f, data.hydrogenFilteredGas) : 0f;
            reservoirBufferedMass = data.version >= 40 ? Mathf.Max(0f, data.reservoirBufferedMass) : 0f;
            automatedConduitFlow = data.version >= 41 ? Mathf.Max(0f, data.automatedConduitFlow) : 0f;
            renewableWaterGenerated = data.version >= 42 ? Mathf.Max(0f, data.renewableWaterGenerated) : 0f;
            renewableHydrogenGenerated = data.version >= 42 ? Mathf.Max(0f, data.renewableHydrogenGenerated) : 0f;
            renewableNaturalGasGenerated = data.version >= 68 ? Mathf.Max(0f, data.renewableNaturalGasGenerated) : 0f;
            hatchCoalProduced = data.version >= 43 ? Mathf.Max(0f, data.hatchCoalProduced) : 0f;
            hatchesGroomed = data.version >= 43 ? Mathf.Max(0, data.hatchesGroomed) : 0;
            transformedPowerDelivered = data.version >= 44 ? Mathf.Max(0f, data.transformedPowerDelivered) : 0f;
            overloadedWireSeconds = data.version >= 44 ? Mathf.Max(0f, data.overloadedWireSeconds) : 0f;
            handsWashed = data.version >= 45 ? Mathf.Max(0, data.handsWashed) : 0;
            cropsTended = data.version >= 46 ? Mathf.Max(0, data.cropsTended) : 0;
            cropStifledSeconds = data.version >= 60 ? Mathf.Max(0f, data.cropStifledSeconds) : 0f;
            cropsWilted = data.version >= 60 ? Mathf.Max(0, data.cropsWilted) : 0;
            autoSweptResources = data.version >= 47 ? Mathf.Max(0f, data.autoSweptResources) : 0f;
            conveyorShippedResources = data.version >= 48 ? Mathf.Max(0f, data.conveyorShippedResources) : 0f;
            bottleEmptiedLiquid = data.version >= 49 ? Mathf.Max(0f, data.bottleEmptiedLiquid) : 0f;
            signalSwitchesToggled = data.version >= 50 ? Mathf.Max(0, data.signalSwitchesToggled) : 0;
            airlockToggles = data.version >= 51 ? Mathf.Max(0, data.airlockToggles) : 0;
            techAirSystems = data.techAirSystems;
            techFoodPreparation = data.techFoodPreparation;
            techPowerRegulation = data.techPowerRegulation;
            colonyVictory = data.colonyVictory;
            colonyVictoryAcknowledged = false;
            colonyFailed = data.colonyFailed;
            paused = colonyFailed;
            InvalidateRooms();

            foreach (Worker worker in workers)
            {
                ClearAssignment(worker);
            }

            if (data.workers != null)
            {
                for (int i = workers.Count - 1; i >= data.workers.Length; i--)
                {
                    Worker extraWorker = workers[i];
                    if (extraWorker.Transform != null)
                    {
                        DestroyRuntimeObject(extraWorker.Transform.gameObject);
                    }

                    workers.RemoveAt(i);
                }

                while (workers.Count < data.workers.Length)
                {
                    WorkerSave workerSave = data.workers[workers.Count];
                    Vector2Int cell = new Vector2Int(
                        Mathf.Clamp(workerSave.cellX, 0, WorldWidth - 1),
                        Mathf.Clamp(workerSave.cellY, 0, WorldHeight - 1));
                    string workerName = string.IsNullOrEmpty(workerSave.name) ? NextDuplicantName() : workerSave.name;
                    SpawnWorker(workerName, cell, WorkerTint(workers.Count));
                }

                int workerCount = Mathf.Min(workers.Count, data.workers.Length);
                for (int i = 0; i < workerCount; i++)
                {
                    Worker worker = workers[i];
                    WorkerSave workerSave = data.workers[i];
                    worker.Name = string.IsNullOrEmpty(workerSave.name) ? worker.Name : workerSave.name;
                    if (worker.Transform != null)
                    {
                        worker.Transform.name = "Duplicant " + worker.Name;
                    }

                    worker.Cell = new Vector2Int(
                        Mathf.Clamp(workerSave.cellX, 0, WorldWidth - 1),
                        Mathf.Clamp(workerSave.cellY, 0, WorldHeight - 1));
                    Vector3 position = new Vector3(workerSave.positionX, workerSave.positionY, 0f);
                    if (position.sqrMagnitude < 0.01f)
                    {
                        position = CellCenter(worker.Cell);
                    }

                    worker.Transform.position = position;
                    worker.Calories = Mathf.Max(0f, workerSave.calories);
                    worker.Stress = Mathf.Clamp(workerSave.stress, 0f, 100f);
                    worker.Health = Mathf.Clamp(data.version < 35 && workerSave.health <= 0f && !colonyFailed ? 100f : workerSave.health, 0f, 100f);
                    worker.Morale = data.version >= 58 ? Mathf.Clamp(workerSave.morale, 0f, 10f) : WorkerMoraleTarget(worker);
                    worker.Fatigue = Mathf.Clamp(data.version < 4 && workerSave.fatigue <= 0f ? 25f : workerSave.fatigue, 0f, 100f);
                    worker.Bladder = Mathf.Clamp(data.version < 11 ? 20f : workerSave.bladder, 0f, 100f);
                    worker.GermExposure = Mathf.Clamp(data.version < 5 ? 0f : workerSave.germExposure, 0f, 100f);
                    worker.Sickness = Mathf.Clamp(data.version < 5 ? 0f : workerSave.sickness, 0f, 100f);
                    worker.HeatExposure = data.version >= 62 ? Mathf.Clamp(workerSave.heatExposure, 0f, 100f) : 0f;
                    worker.ChillExposure = data.version >= 62 ? Mathf.Clamp(workerSave.chillExposure, 0f, 100f) : 0f;
                    worker.StressBreakSeconds = Mathf.Max(0f, data.version < 13 ? 0f : workerSave.stressBreakSeconds);
                    worker.StressBreakPulseTimer = 0f;
                    worker.IncapacitatedSeconds = data.version >= 35 ? Mathf.Max(0f, workerSave.incapacitatedSeconds) : 0f;
                    worker.Experience = data.version >= 22 ? Mathf.Clamp(workerSave.experience, 0f, MaxWorkerExperience()) : 0f;
                    worker.SuitEquipped = data.version >= 39 && workerSave.suitEquipped;
                    worker.WorkSpeed = WorkerSkillSpeedMultiplier(worker);
                    worker.Activity = worker.Health <= 0f ? "Incapacitated" : worker.StressBreakSeconds > 0f ? "Stress Break" : "Idle";
                    if (!IsCharacterStandableCell(worker.Cell) && TryFindCharacterStandableCellNear(worker.Cell, 10, worker, out Vector2Int safeWorkerCell))
                    {
                        worker.Cell = safeWorkerCell;
                        if (worker.Transform != null)
                        {
                            worker.Transform.position = CellCenter(safeWorkerCell);
                        }
                    }
                }
            }

            ClearHatches();
            if (data.version >= 43 && data.hatches != null)
            {
                for (int i = 0; i < data.hatches.Length && i < MaxWildHatches; i++)
                {
                    HatchSave hatchSave = data.hatches[i];
                    Vector2Int cell = new Vector2Int(
                        Mathf.Clamp(hatchSave.cellX, 0, WorldWidth - 1),
                        Mathf.Clamp(hatchSave.cellY, 0, WorldHeight - 1));
                    if (!CanHatchMoveTo(cell))
                    {
                        Vector2Int fallbackCell;
                        if (TryFindHatchSpawnCell(cell, 6, out fallbackCell))
                        {
                            cell = fallbackCell;
                        }
                    }

                    HatchCritter hatch = SpawnHatch(
                        hatchSave.name,
                        cell,
                        hatchSave.happiness,
                        hatchSave.groomedSeconds,
                        hatchSave.moveTimer,
                        hatchSave.eatTimer,
                        hatchSave.coalProduced);
                    Vector3 position = new Vector3(hatchSave.positionX, hatchSave.positionY, 0f);
                    if (position.sqrMagnitude > 0.01f && hatch.Transform != null)
                    {
                        hatch.Transform.position = position;
                    }
                }
            }
            else
            {
                SeedHatches();
            }

            jobs.Clear();
            if (data.jobs != null)
            {
                int jobTypeMax = Enum.GetValues(typeof(JobType)).Length - 1;
                for (int i = 0; i < data.jobs.Length; i++)
                {
                    JobSave jobSave = data.jobs[i];
                    Vector2Int cell = new Vector2Int(jobSave.cellX, jobSave.cellY);
                    if (!IsInside(cell.x, cell.y))
                    {
                        continue;
                    }

                    Job job = new Job((JobType)Mathf.Clamp(jobSave.type, 0, jobTypeMax), cell, jobSave.workRequired)
                    {
                        BuildKind = (CellKind)Mathf.Clamp(jobSave.buildKind, 0, cellKindMax),
                        Progress = Mathf.Max(0f, jobSave.progress),
                        DirtCost = jobSave.dirtCost,
                        MetalCost = jobSave.metalCost,
                        AlgaeCost = jobSave.algaeCost,
                        RefinedMetalCost = data.version >= 28 ? jobSave.refinedMetalCost : 0f,
                        Priority = jobSave.priority > 0 ? jobSave.priority : DefaultPriority((JobType)Mathf.Clamp(jobSave.type, 0, jobTypeMax)),
                        AgeSeconds = data.version >= 74 ? Mathf.Clamp(jobSave.ageSeconds, 0f, JobAgingMaxSeconds) : 0f,
                        AutoGenerated = jobSave.autoGenerated,
                        TargetWorkerName = jobSave.targetWorkerName,
                        BuildWire = jobSave.buildWire,
                        BuildPipe = jobSave.buildPipe,
                        BuildGasPipe = jobSave.buildGasPipe,
                        BuildShippingRail = data.version >= 48 && jobSave.buildShippingRail,
                        RemovePowerWire = data.version >= 31 && jobSave.removePowerWire,
                        RemoveAutomationWire = data.version >= 31 && jobSave.removeAutomationWire,
                        RemoveLiquidPipe = data.version >= 31 && jobSave.removeLiquidPipe,
                        RemoveGasPipe = data.version >= 31 && jobSave.removeGasPipe,
                        RemoveShippingRail = data.version >= 48 && jobSave.removeShippingRail
                    };
                    jobs.Add(job);
                }
            }

            int modeMax = Enum.GetValues(typeof(CommandMode)).Length - 1;
            currentMode = (CommandMode)Mathf.Clamp(data.currentMode, 0, modeMax);
            if (!string.IsNullOrEmpty(data.language) && Enum.TryParse(data.language, out ProjectONLanguage savedLanguage))
            {
                currentLanguage = savedLanguage;
            }

            RefreshLocalizedStaticTexts();
            SetMode(currentMode);
            int overlayModeMax = Enum.GetValues(typeof(OverlayMode)).Length - 1;
            currentOverlayMode = (OverlayMode)Mathf.Clamp(data.currentOverlayMode, 0, overlayModeMax);
            SetOverlayMode(currentOverlayMode);
            inspectedCell = null;
            lastLog = string.IsNullOrEmpty(data.lastLog) ? "Game loaded." : data.lastLog;
            UpdatePoweredWires();
            UpdateAutomationWires();
            terrainDirty = true;
            gasDirty = true;
            overlayDirty = true;
            UpdateColonyStatus(true);
        }

        private float ReadSaveFloat(float[] values, int index, float fallback)
        {
            return values == null || index < 0 || index >= values.Length ? fallback : values[index];
        }

        private int ReadSaveInt(int[] values, int index, int fallback)
        {
            return values == null || index < 0 || index >= values.Length ? fallback : values[index];
        }

        private bool ReadSaveBool(bool[] values, int index, bool fallback)
        {
            return values == null || index < 0 || index >= values.Length ? fallback : values[index];
        }

        private void UpdateHud()
        {
            if (statsText == null)
            {
                return;
            }

            RefreshModeButtonLabels();

            float averageOxygen = AverageGas(oxygen);
            float averageCo2 = AverageGas(carbonDioxide);
            float averagePollutedOxygen = AverageGas(pollutedOxygen);
            float averageHydrogen = AverageGas(hydrogen);
            float averageSteam = AverageGas(steam);
            float averageChlorine = AverageGas(chlorine);
            float averageNaturalGas = AverageGas(naturalGas);
            float averageTemperature = AverageTemperature();
            UpdatePoweredWires();
            UpdateAutomationWires();
            UpdatePowerLoad(0f, false);
            int wireTiles = CountPowerWireTiles();
            int poweredBuildings = CountPoweredBuildings();
            int overloadedWires = CountOverloadedPowerWires();
            float maxWireLoad = MaxPowerWireLoad();
            int shippingRails = CountShippingRailTiles();
            float shippingMass = TotalShippingRailMass();
            int automationTiles = CountAutomationWireTiles();
            int controlledShutoffs = CountAutomationControlledConduitShutoffs();
            int airlocks = CountCells(CellKind.ManualAirlock);
            int closedAirlocks = CountClosedAirlocks();
            int unstableSand = CountUnstableSandTiles();
            int flowingLiquids = CountFlowingLiquidTiles();
            int meltingIce = CountMeltingIceTiles();
            int freezingWater = CountFreezingWaterTiles();
            int submergedEquipment = CountSubmergedEquipment();
            int floodedPowerWires = CountFloodedPowerWires();
            int overheatingEquipment = CountOverheatingEquipment();
            int highPressureTiles = CountHighPressureTiles();
            int dangerousPressureTiles = CountDangerousPressureTiles();
            int steamTiles = CountSteamTiles();
            int chlorineTiles = CountChlorineTiles();
            int naturalGasTiles = CountNaturalGasTiles();
            int thermalWorkers = CountThermallyExposedWorkers();
            int criticalThermalWorkers = CountCriticalThermalExposureWorkers();
            float averageMorale = AverageWorkerMorale();
            float averageMoraleNeed = AverageWorkerMoraleNeed();
            int lowMoraleWorkers = CountLowMoraleWorkers();
            int stressedCrops = CountStressedCrops();
            int wiltingCrops = CountWiltingCrops();
            int pipeTiles = CountLiquidPipeTiles();
            float pipeStoredWater = TotalPipeWater();
            int unsafeLiquidPipes = CountUnsafeLiquidPipes();
            int unsafeLiquidReservoirs = CountUnsafeLiquidReservoirs();
            int pollutedWaterOffgasSourceCount = CountPollutedWaterOffgasSources();
            float reservoirWater = TotalLiquidReservoirWater();
            int gasPipeTiles = CountGasPipeTiles();
            float pipeStoredGas = TotalGasPipeMass();
            float reservoirGas = TotalGasReservoirMass();
            int openJobs = 0;
            int assignedJobs = 0;
            foreach (Job job in jobs)
            {
                if (job.AssignedWorker == null)
                {
                    openJobs++;
                }
                else
                {
                    assignedJobs++;
                }
            }

            string hudText =
                "Cycle " + cycle + "  " + (paused ? "PAUSED" : "x" + simulationSpeed.ToString("0")) +
                "  Schedule " + ScheduleLabel() +
                "    O2 " + averageOxygen.ToString("0.00") +
                "  CO2 " + averageCo2.ToString("0.00") +
                "  PO2 " + averagePollutedOxygen.ToString("0.00") +
                "  H2 " + averageHydrogen.ToString("0.00") +
                "  Steam " + averageSteam.ToString("0.00") +
                "  Cl " + averageChlorine.ToString("0.00") +
                "  NG " + averageNaturalGas.ToString("0.00") +
                "  Temp " + averageTemperature.ToString("0") + "C" +
                "    Power " + power.ToString("0") + "/" + maxPower.ToString("0") +
                "    Dirt " + dirt.ToString("0") +
                "  Metal " + metal.ToString("0") +
                "  Ref " + refinedMetal.ToString("0") +
                "  Algae " + algae.ToString("0") +
                "  Coal " + coal.ToString("0") +
                "  SuitO2 " + suitOxygen.ToString("0.0") + "/" + SuitOxygenCapacityTotal().ToString("0") +
                "  SuitX " + suitCheckpointUses +
                "  SuitBlk " + suitEntryDenials +
                "  Sand " + sandFalls + "/" + unstableSand +
                "  LFlow " + liquidFlowedMass.ToString("0") + "/" + flowingLiquids +
                "  Phase " + iceMeltedTiles + "/" + waterFrozenTiles + "@" + meltingIce + "/" + freezingWater +
                " Vapor " + steamEvaporatedMass.ToString("0.0") + "/" + steamCondensedMass.ToString("0.0") + "@" + steamTiles +
                "  Chlor " + chlorineSterilizedGerms.ToString("0.0") + "/" + chlorineExposureSeconds.ToString("0") + "/" + chlorineHealthDamage.ToString("0.0") + "@" + chlorineTiles +
                "  Flood " + submergedEquipmentDamage.ToString("0.0") + "/" + floodedWireFailures + "@" + submergedEquipment + "/" + floodedPowerWires +
                "  OHeat " + overheatedEquipmentDamage.ToString("0.0") + "/" + overheatedEquipmentFailures + "@" + overheatingEquipment +
                "  Press " + overpressureExposureSeconds.ToString("0") + "/" + overpressureHealthDamage.ToString("0.0") + "@" + highPressureTiles + "/" + dangerousPressureTiles +
                "  Therm " + thermalExposureSeconds.ToString("0") + "/" + thermalHealthDamage.ToString("0.0") + "@" + thermalWorkers + "/" + criticalThermalWorkers +
                " Inj " + heatStrokeCases + "/" + hypothermiaCases +
                "  Store " + DryResourceAmount().ToString("0") + "/" + DryResourceCapacity().ToString("0") +
                "  Loose " + TotalLooseResources().ToString("0") +
                "  Water " + water.ToString("0") +
                "  PW " + pollutedWater.ToString("0") +
                " PWGas " + pollutedWaterOffgassedMass.ToString("0.0") + "/" + pollutedWaterOffgasEvents + "@" + pollutedWaterOffgasSourceCount +
                "  PD " + pollutedDirt.ToString("0") +
                "  Wash " + handsWashed +
                "  Pipe " + pipeStoredWater.ToString("0") + "/" + (pipeTiles * LiquidPipeCapacity).ToString("0") +
                " Burst " + pipeBurstWater.ToString("0.0") + "/" + pipeBurstEvents + "@" + unsafeLiquidPipes + " F/B " + frozenPipeBursts + "/" + boiledPipeBursts +
                "  LRes " + reservoirWater.ToString("0") +
                " RBrst " + reservoirBurstWater.ToString("0.0") + "/" + reservoirBurstEvents + "@" + unsafeLiquidReservoirs + " F/B " + frozenReservoirBursts + "/" + boiledReservoirBursts +
                "  Vent " + pipeStoredGas.ToString("0.0") + "/" + (gasPipeTiles * GasPipeCapacity).ToString("0") +
                "  GRes " + reservoirGas.ToString("0.0") +
                "  Food " + food.ToString("0") +
                " Fresh " + Mathf.RoundToInt(foodFreshness * 100f) + "%" +
                " BadMeal " + staleMealsEaten + "/" + foodPoisoningCases +
                "  Morale " + averageMorale.ToString("0.0") + "/" + averageMoraleNeed.ToString("0.0") + "@" + lowMoraleWorkers +
                " Def " + moralePressureSeconds.ToString("0") + "/" + moraleStressAdded.ToString("0.0") +
                "  Farm " + cropsTended + " Stress " + cropStifledSeconds.ToString("0") + "/" + cropsWilted + "@" + stressedCrops + "/" + wiltingCrops +
                "  Research " + researchPoints.ToString("0") +
                "  Print " + Mathf.RoundToInt(printingPodProgress * 100f) + "%" +
                "  Grid " + poweredBuildings + "/" + wireTiles +
                " Load " + maxWireLoad.ToString("0.0") + "/" + (WireSafeLoad + PowerTransformerLoadBonus).ToString("0.0") +
                " Over " + overloadedWires +
                "  Auto " + CountAutomationControlledGenerators() + "/" + automationTiles +
                " Sw " + signalSwitchesToggled +
                "  Door " + closedAirlocks + "/" + airlocks +
                "  Valve " + controlledShutoffs +
                " Flow " + automatedConduitFlow.ToString("0.0") +
                "  VentW " + renewableWaterGenerated.ToString("0.0") +
                " VentH2 " + renewableHydrogenGenerated.ToString("0.00") +
                " VentNG " + renewableNaturalGasGenerated.ToString("0.00") + "@" + naturalGasTiles +
                "  Decon " + deconstructionsCompleted +
                "  Mop " + moppedLiquid.ToString("0") +
                "  Bottle " + bottleEmptiedLiquid.ToString("0") +
                "  Repair " + repairsCompleted +
                "  Rescue " + rescuesCompleted +
                "  Sweep " + sweptResources.ToString("0") +
                " AutoSwp " + autoSweptResources.ToString("0") +
                " Ship " + conveyorShippedResources.ToString("0") + "/" + shippingMass.ToString("0") + "@" + shippingRails +
                "  H2Gen " + hydrogenPowerGenerated.ToString("0") +
                " NGGen " + naturalGasPowerGenerated.ToString("0") +
                " SteamTur " + steamTurbinePowerGenerated.ToString("0") + "/" + steamTurbineWaterRecovered.ToString("0.0") +
                " Solar " + solarPowerGenerated.ToString("0") + "/" + Mathf.RoundToInt(SolarIrradiance() * 100f) + "%@" + CountSkyExposedSolarPanels() + "/" + CountCells(CellKind.SolarPanel) +
                " Meteor " + (IsMeteorShowerActive() ? "ACTIVE" + meteorShowerSeconds.ToString("0") : "in" + meteorCooldownSeconds.ToString("0")) + "/" + meteorStrikes + "/" + meteorImpactsBlocked + "/" + meteorDamageEvents +
                " Scan " + spaceScannerSignalSeconds.ToString("0") + "/" + CountSkyExposedSpaceScanners() + "/" + CountCells(CellKind.SpaceScanner) +
                "  H2Filt " + hydrogenFilteredGas.ToString("0") +
                "  Hatch " + hatches.Count + "/" + CountGroomedHatches() +
                " Coal " + hatchCoalProduced.ToString("0.0") +
                "    Jobs " + assignedJobs + "/" + jobs.Count + " (" + openJobs + " open)" +
                "\nObjective: " + objectiveText + "    Tech: " + TechSummary() + "    Alerts: " + alertText;

            hudText = BuildTopStatusText(averageOxygen, averageCo2, averageTemperature, assignedJobs, openJobs);
            statsText.text = Localize(hudText);
            modeText.text = Localize(BuildModeStatusText());
            if (scenarioText != null)
            {
                scenarioText.text = Localize(BuildScenarioText());
            }

            if (jobQueueText != null)
            {
                float now = Time.unscaledTime;
                if (now >= nextJobQueueRefreshTime || renderedJobQueueLanguage != currentLanguage || string.IsNullOrEmpty(jobQueueTextCache))
                {
                    jobQueueTextCache = Localize(BuildJobQueueText(assignedJobs, openJobs));
                    renderedJobQueueLanguage = currentLanguage;
                    nextJobQueueRefreshTime = now + JobQueueRefreshIntervalSeconds;
                }

                jobQueueText.text = jobQueueTextCache;
            }

            UpdateOverlayLegend();
            logText.text = Localize(lastLog);
            infoText.text = Localize(BuildInspectText());
            UpdateInspectControls();
            UpdateEndStatePanel();
        }

        private void UpdateOverlayLegend()
        {
            if (overlayLegendRows == null || overlayLegendTitleText == null)
            {
                return;
            }

            if (renderedLegendOverlayMode == currentOverlayMode && renderedLegendLanguage == currentLanguage)
            {
                return;
            }

            renderedLegendOverlayMode = currentOverlayMode;
            renderedLegendLanguage = currentLanguage;
            overlayLegendRows.Clear();
            overlayLegendTitleText.text = Localize("Legend: " + OverlayName(currentOverlayMode));

            switch (currentOverlayMode)
            {
                case OverlayMode.Temperature:
                    AddOverlayLegendRow(new Color(0.18f, 0.58f, 1f, 0.90f), "Cold");
                    AddOverlayLegendRow(new Color(0.20f, 1f, 0.55f, 0.45f), "Comfort");
                    AddOverlayLegendRow(new Color(1f, 0.16f, 0.04f, 0.90f), "Hot");
                    AddOverlayLegendRow(new Color(1f, 0.72f, 0.20f, 0.90f), "Steam risk");
                    break;
                case OverlayMode.Power:
                    AddOverlayLegendRow(new Color(1f, 0.86f, 0.18f, 0.90f), "Powered wire");
                    AddOverlayLegendRow(new Color(0.52f, 0.30f, 0.08f, 0.90f), "Unpowered wire");
                    AddOverlayLegendRow(new Color(1f, 0.14f, 0.04f, 0.95f), "Overload");
                    AddOverlayLegendRow(new Color(0.35f, 1f, 0.44f, 0.80f), "Machine running");
                    break;
                case OverlayMode.Germs:
                    AddOverlayLegendRow(new Color(0.20f, 1f, 0.18f, 0.45f), "Low germs");
                    AddOverlayLegendRow(new Color(0.58f, 1f, 0.18f, 0.75f), "High germs");
                    AddOverlayLegendRow(new Color(0.28f, 1f, 0.32f, 0.75f), "Polluted oxygen");
                    AddOverlayLegendRow(new Color(0f, 0f, 0f, 0.30f), "Clean tile");
                    break;
                case OverlayMode.Plumbing:
                    AddOverlayLegendRow(new Color(0.08f, 0.78f, 1f, 0.85f), "Liquid pipe");
                    AddOverlayLegendRow(new Color(0.10f, 0.86f, 1f, 0.85f), "Reservoir fill");
                    AddOverlayLegendRow(new Color(0.26f, 1f, 0.38f, 0.85f), "Green sensor");
                    AddOverlayLegendRow(new Color(1f, 0.20f, 0.12f, 0.85f), "Closed or blocked");
                    break;
                case OverlayMode.Ventilation:
                    AddOverlayLegendRow(new Color(0.20f, 0.84f, 1f, 0.85f), "Oxygen pipe");
                    AddOverlayLegendRow(new Color(0.86f, 0.36f, 1f, 0.85f), "Hydrogen pipe");
                    AddOverlayLegendRow(new Color(1f, 0.58f, 0.14f, 0.85f), "Natural gas pipe");
                    AddOverlayLegendRow(new Color(1f, 0.20f, 0.12f, 0.85f), "Closed or blocked");
                    break;
                case OverlayMode.Logistics:
                    AddOverlayLegendRow(new Color(1f, 0.72f, 0.20f, 0.85f), "Shipping rail");
                    AddOverlayLegendRow(new Color(1f, 0.72f, 0.20f, 0.85f), "Loader powered");
                    AddOverlayLegendRow(new Color(1f, 0.22f, 0.12f, 0.85f), "Loader blocked");
                    AddOverlayLegendRow(new Color(0.95f, 0.82f, 0.20f, 0.70f), "Auto-sweeper");
                    break;
                case OverlayMode.Decor:
                    AddOverlayLegendRow(new Color(1f, 0.54f, 0.96f, 0.90f), "Decor source");
                    AddOverlayLegendRow(new Color(0.92f, 0.58f, 1f, 0.65f), "Decor aura");
                    AddOverlayLegendRow(new Color(0f, 0f, 0f, 0.30f), "No decor");
                    AddOverlayLegendRow(new Color(0.62f, 1f, 0.78f, 0.75f), "Morale help");
                    break;
                case OverlayMode.Rooms:
                    AddOverlayLegendRow(RoomKindColor(RoomKind.Barracks), "Barracks");
                    AddOverlayLegendRow(RoomKindColor(RoomKind.MessHall), "Mess Hall");
                    AddOverlayLegendRow(RoomKindColor(RoomKind.Washroom), "Washroom");
                    AddOverlayLegendRow(RoomKindColor(RoomKind.MixedRoom), "Mixed Room");
                    break;
                default:
                    AddOverlayLegendRow(new Color(0.20f, 0.72f, 1f, 0.85f), "Oxygen");
                    AddOverlayLegendRow(new Color(0.85f, 0.34f, 0.16f, 0.85f), "Carbon dioxide");
                    AddOverlayLegendRow(new Color(0.28f, 0.92f, 0.36f, 0.85f), "Polluted oxygen");
                    AddOverlayLegendRow(new Color(0.82f, 0.38f, 1f, 0.85f), "Hydrogen");
                    break;
            }
        }

        private void AddOverlayLegendRow(Color color, string label)
        {
            VisualElement row = new VisualElement();
            row.AddToClassList("overlay-legend-row");

            VisualElement swatch = new VisualElement();
            swatch.AddToClassList("overlay-legend-swatch");
            swatch.style.backgroundColor = new StyleColor(color);
            row.Add(swatch);

            Label text = new Label(Localize(label));
            text.AddToClassList("overlay-legend-label");
            ApplyRuntimeFont(text);
            row.Add(text);

            overlayLegendRows.Add(row);
        }

        private void UpdateEndStatePanel()
        {
            if (endStatePanel == null)
            {
                return;
            }

            bool showFailure = colonyFailed;
            bool showVictory = colonyVictory && !colonyVictoryAcknowledged;
            bool showPanel = showFailure || showVictory;
            SetVisible(endStatePanel, showPanel);
            if (!showPanel)
            {
                return;
            }

            if (showFailure)
            {
                endStateTitleText.text = Localize("Colony failed");
                endStateBodyText.text = Localize("All duplicants are incapacitated. Load a save or start a new run.");
                SetVisible(endStateLoadButton, true);
                SetVisible(endStateNewRunButton, true);
                SetVisible(endStateContinueButton, false);
            }
            else
            {
                endStateTitleText.text = Localize("Colony Charter complete");
                endStateBodyText.text = Localize("The colony charter is complete. Continue freeplay, load a save, or start a new run.");
                SetVisible(endStateLoadButton, true);
                SetVisible(endStateNewRunButton, true);
                SetVisible(endStateContinueButton, true);
            }
        }

        private string BuildTopStatusText(float averageOxygen, float averageCo2, float averageTemperature, int assignedJobs, int openJobs)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Cycle ");
            builder.Append(cycle);
            builder.Append("  ");
            builder.Append(paused ? "PAUSED" : "x" + simulationSpeed.ToString("0"));
            builder.Append("  Schedule ");
            builder.Append(ScheduleLabel());
            builder.Append("    O2 ");
            builder.Append(averageOxygen.ToString("0.00"));
            builder.Append("  CO2 ");
            builder.Append(averageCo2.ToString("0.00"));
            builder.Append("  Temp ");
            builder.Append(averageTemperature.ToString("0"));
            builder.Append("C  Power ");
            builder.Append(power.ToString("0"));
            builder.Append("/");
            builder.Append(maxPower.ToString("0"));
            builder.AppendLine();
            builder.Append("Food ");
            builder.Append(food.ToString("0"));
            builder.Append("  Water ");
            builder.Append(water.ToString("0"));
            builder.Append("  Algae ");
            builder.Append(algae.ToString("0"));
            builder.Append("  Metal ");
            builder.Append(metal.ToString("0"));
            builder.Append("  Stress ");
            builder.Append(HighestStress().ToString("0"));
            builder.Append("%  Jobs ");
            builder.Append(assignedJobs);
            builder.Append("/");
            builder.Append(jobs.Count);
            builder.Append(" (");
            builder.Append(openJobs);
            builder.Append(" open)  Alert: ");
            if (unreachableJobCount > 0)
            {
                builder.Append("Blocked ");
                builder.Append(unreachableJobCount);
                builder.Append("  ");
            }

            builder.Append(CompactAlertText(alertText, 3));
            return builder.ToString();
        }

        private string BuildModeStatusText()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Mode: ");
            builder.Append(ModeName(currentMode));
            builder.Append("    Overlay: ");
            builder.Append(OverlayName(currentOverlayMode));
            builder.AppendLine();
            builder.Append("Rooms: ");
            builder.Append(RoomSummary());
            builder.Append("    Tech: ");
            builder.Append(TechSummary());
            builder.AppendLine();
            builder.Append("Workers: ");
            builder.Append(CountActiveWorkers());
            builder.Append("/");
            builder.Append(workers.Count);
            int incapacitated = CountIncapacitatedWorkers();
            if (incapacitated > 0)
            {
                builder.Append("  Down ");
                builder.Append(incapacitated);
            }

            int lowMorale = CountLowMoraleWorkers();
            if (lowMorale > 0)
            {
                builder.Append("  Low morale ");
                builder.Append(lowMorale);
            }

            return builder.ToString();
        }

        private string BuildScenarioText()
        {
            int completed = CountCompletedScenarioMilestones();
            int percent = Mathf.RoundToInt(completed * 100f / ScenarioMilestoneTotal);
            StringBuilder builder = new StringBuilder();
            if (colonyFailed)
            {
                builder.AppendLine("Scenario: Colony failed");
            }
            else if (colonyVictory)
            {
                builder.AppendLine("Scenario: Colony Charter complete  " + completed + "/" + ScenarioMilestoneTotal + " (" + percent + "%)");
            }
            else
            {
                builder.AppendLine("Scenario: Colony Charter  " + completed + "/" + ScenarioMilestoneTotal + " (" + percent + "%)");
            }

            builder.AppendLine("Phase: " + ScenarioPhaseName(completed) + " - " + ScenarioPhaseGoal(completed));
            builder.AppendLine("Next: " + objectiveText);
            builder.Append("Critical: ");
            builder.Append(CompactAlertText(alertText, 4));
            return builder.ToString();
        }

        private string ScenarioPhaseName(int completedMilestones)
        {
            if (colonyVictory)
            {
                return "Freeplay";
            }

            if (completedMilestones < 12)
            {
                return "Survival";
            }

            if (completedMilestones < 24)
            {
                return "Base Systems";
            }

            if (completedMilestones < 36)
            {
                return "Industry";
            }

            if (completedMilestones < 47)
            {
                return "Automation";
            }

            return "Surface & Space";
        }

        private string ScenarioPhaseGoal(int completedMilestones)
        {
            if (colonyVictory)
            {
                return "Expand, optimize, and survive indefinitely.";
            }

            if (completedMilestones < 12)
            {
                return "Beds, toilets, oxygen, food, and water.";
            }

            if (completedMilestones < 24)
            {
                return "Research, rooms, morale, power, and storage.";
            }

            if (completedMilestones < 36)
            {
                return "Pipes, vents, suits, refining, and backup power.";
            }

            if (completedMilestones < 47)
            {
                return "Sensors, sweepers, shipping, ranching, and vents.";
            }

            return "Solar, steam, scanners, bunker doors, and final charter.";
        }

        private string CompactAlertText(string alerts, int maxAlerts)
        {
            if (string.IsNullOrEmpty(alerts))
            {
                return "Stable.";
            }

            string[] alertParts = alerts.Split(new[] { ", " }, StringSplitOptions.None);
            if (alertParts.Length <= maxAlerts)
            {
                return alerts;
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < maxAlerts; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(alertParts[i]);
            }

            builder.Append(", +");
            builder.Append(alertParts.Length - maxAlerts);
            builder.Append(" more");
            return builder.ToString();
        }

        private int CountCompletedScenarioMilestones()
        {
            int count = 0;
            count += milestoneBasicShelter ? 1 : 0;
            count += milestoneWaterSupply ? 1 : 0;
            count += milestoneResearchProgram ? 1 : 0;
            count += milestoneStableOxygen ? 1 : 0;
            count += milestoneFoodPreparation ? 1 : 0;
            count += milestoneFoodProduction ? 1 : 0;
            count += milestoneCropTending ? 1 : 0;
            count += milestonePowerBuffer ? 1 : 0;
            count += milestonePowerGrid ? 1 : 0;
            count += milestonePowerLoadManagement ? 1 : 0;
            count += milestoneFuelPower ? 1 : 0;
            count += milestoneHydrogenFiltering ? 1 : 0;
            count += milestoneHydrogenPower ? 1 : 0;
            count += milestoneMetalRefining ? 1 : 0;
            count += milestoneAtmoSuits ? 1 : 0;
            count += milestoneInsulation ? 1 : 0;
            count += milestoneRoomPlanning ? 1 : 0;
            count += milestoneReconfiguration ? 1 : 0;
            count += milestoneColonyExpansion ? 1 : 0;
            count += milestoneMaintenance ? 1 : 0;
            count += milestoneEmergencyResponse ? 1 : 0;
            count += milestoneResourceLogistics ? 1 : 0;
            count += milestoneBottleEmptying ? 1 : 0;
            count += milestoneAutomation ? 1 : 0;
            count += milestoneSignalSwitching ? 1 : 0;
            count += milestoneAutoSweeping ? 1 : 0;
            count += milestoneShippingLogistics ? 1 : 0;
            count += milestoneConduitAutomation ? 1 : 0;
            count += milestoneRenewableVents ? 1 : 0;
            count += milestoneSteamPower ? 1 : 0;
            count += milestoneSolarPower ? 1 : 0;
            count += milestoneMeteorShielding ? 1 : 0;
            count += milestoneSpaceScanning ? 1 : 0;
            count += milestoneRanching ? 1 : 0;
            count += milestoneThermalControl ? 1 : 0;
            count += milestoneSanitation ? 1 : 0;
            count += milestoneHygiene ? 1 : 0;
            count += milestoneWasteProcessing ? 1 : 0;
            count += milestoneMoraleCare ? 1 : 0;
            count += milestonePressureControl ? 1 : 0;
            count += milestoneAirlockControl ? 1 : 0;
            count += milestoneFoodStorage ? 1 : 0;
            count += milestoneMaterialStorage ? 1 : 0;
            count += milestonePlumbing ? 1 : 0;
            count += milestoneSpillCleanup ? 1 : 0;
            count += milestoneVentilation ? 1 : 0;
            count += milestoneReservoirBuffering ? 1 : 0;
            count += milestoneAdvancedAtmosphere ? 1 : 0;
            count += milestoneWaterRecycling ? 1 : 0;
            count += milestoneDining ? 1 : 0;
            count += milestoneSkilledLabor ? 1 : 0;
            count += milestoneDecorComfort ? 1 : 0;
            count += milestoneCycleFive ? 1 : 0;
            return count;
        }

        private void GrantCharterMilestoneRewards(int previousCount, int nextCount)
        {
            int completed = Mathf.Max(0, nextCount - previousCount);
            if (completed <= 0)
            {
                return;
            }

            float foodReward = workers.Count * 35f * completed;
            float waterReward = 2.5f * completed;
            float dirtReward = 5f * completed;
            float metalReward = 3.5f * completed;
            float algaeReward = 0f;
            float coalReward = 0f;
            float refinedMetalReward = 0f;
            float researchReward = 0f;
            float powerCapacityReward = 0f;

            if (CrossedScenarioMilestone(previousCount, nextCount, 12))
            {
                foodReward += workers.Count * 80f;
                waterReward += 30f;
                algaeReward += 25f;
            }

            if (CrossedScenarioMilestone(previousCount, nextCount, 24))
            {
                metalReward += 25f;
                researchReward += 2f;
                powerCapacityReward += 25f;
            }

            if (CrossedScenarioMilestone(previousCount, nextCount, 36))
            {
                coalReward += 35f;
                refinedMetalReward += 12f;
            }

            if (CrossedScenarioMilestone(previousCount, nextCount, 47))
            {
                waterReward += 40f;
                refinedMetalReward += 18f;
                powerCapacityReward += 25f;
            }

            if (CrossedScenarioMilestone(previousCount, nextCount, ScenarioMilestoneTotal))
            {
                foodReward += workers.Count * 300f;
                waterReward += 75f;
                refinedMetalReward += 30f;
            }

            food += foodReward;
            water += waterReward;
            dirt += dirtReward;
            metal += metalReward;
            algae += algaeReward;
            coal += coalReward;
            refinedMetal += refinedMetalReward;
            researchPoints += researchReward;
            maxPower += powerCapacityReward;

            if (researchReward > 0f)
            {
                ApplyResearchUnlocks(true);
            }

            overlayDirty = true;
            StringBuilder builder = new StringBuilder();
            builder.Append("Charter milestone reward delivered: ");
            bool hasReward = false;
            AppendReward(builder, ref hasReward, foodReward, "food");
            AppendReward(builder, ref hasReward, waterReward, "water");
            AppendReward(builder, ref hasReward, dirtReward, "dirt");
            AppendReward(builder, ref hasReward, metalReward, "metal");
            AppendReward(builder, ref hasReward, algaeReward, "algae");
            AppendReward(builder, ref hasReward, coalReward, "coal");
            AppendReward(builder, ref hasReward, refinedMetalReward, "refined metal");
            AppendReward(builder, ref hasReward, researchReward, "research");
            AppendReward(builder, ref hasReward, powerCapacityReward, "power capacity");
            builder.Append(" (");
            builder.Append(nextCount);
            builder.Append("/");
            builder.Append(ScenarioMilestoneTotal);
            builder.Append(").");
            Log(builder.ToString());
        }

        private bool CrossedScenarioMilestone(int previousCount, int nextCount, int threshold)
        {
            return previousCount < threshold && nextCount >= threshold;
        }

        private void AppendReward(StringBuilder builder, ref bool hasReward, float amount, string label)
        {
            if (amount <= 0.01f)
            {
                return;
            }

            if (hasReward)
            {
                builder.Append(", ");
            }

            builder.Append("+");
            builder.Append(amount >= 10f ? amount.ToString("0") : amount.ToString("0.0"));
            builder.Append(" ");
            builder.Append(label);
            hasReward = true;
        }

        private void RefreshModeButtonLabels()
        {
            foreach (KeyValuePair<CommandMode, Button> pair in modeButtons)
            {
                string nextLabel = Localize(ModeButtonLabel(pair.Key));
                if (pair.Value.text != nextLabel)
                {
                    pair.Value.text = nextLabel;
                }
            }

            RefreshLanguageButtonLabel();
        }

        private string BuildInspectText()
        {
            if (!inspectedCell.HasValue)
            {
                return "Select a tile.";
            }

            Vector2Int cell = inspectedCell.Value;
            if (!IsInside(cell.x, cell.y))
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            UpdatePoweredWires();
            UpdateAutomationWires();
            UpdatePowerLoad(0f, false);
            builder.AppendLine("Tile " + cell.x + ", " + cell.y);
            builder.AppendLine(CellLabel(cells[cell.x, cell.y]));
            builder.AppendLine("Schedule " + SleepWindowLabel());
            builder.AppendLine("Temp " + temperature[cell.x, cell.y].ToString("0.0") + " C");
            float localPressure = TileGasTotal(cell.x, cell.y);
            builder.AppendLine("Pressure " + localPressure.ToString("0.00") + " kg" +
                (localPressure > OverpressureDamageThreshold ? "  DANGEROUS" : localPressure > OverpressureStressThreshold ? "  HIGH" : string.Empty));
            if (cells[cell.x, cell.y] == CellKind.Sand)
            {
                builder.AppendLine(CanSandFallInto(cell.x, cell.y - 1) ? "Unstable sand: will fall into open space below." : "Supported sand.");
                builder.AppendLine("Sand falls " + sandFalls + "  Worker hits " + sandStrikeInjuries);
            }
            else if (cells[cell.x, cell.y] == CellKind.Regolith)
            {
                builder.AppendLine(CanSandFallInto(cell.x, cell.y - 1) ? "Loose regolith: will fall into open space below." : "Regolith deposited by meteor impacts.");
                builder.AppendLine("Meteor regolith " + meteorRegolithDeposited.ToString("0") + " kg  Strikes " + meteorStrikes);
            }

            if (powerWire[cell.x, cell.y])
            {
                builder.AppendLine(poweredWire[cell.x, cell.y] ? "Power Wire: connected" : "Power Wire: unpowered");
                builder.AppendLine("Load " + wireLoad[cell.x, cell.y].ToString("0.00") + "/" + PowerLoadLimitForWire(cell.x, cell.y).ToString("0.00") + " kW" + (overloadedWire[cell.x, cell.y] ? "  OVERLOAD" : string.Empty));
                if (IsPowerWireFlooded(cell.x, cell.y))
                {
                    builder.AppendLine("Flooded wire: shorting risk " + wireOverloadStress[cell.x, cell.y].ToString("0.0") + "/" + WireOverloadBreakStress.ToString("0") + ".");
                }

                if (HasTransformerProtection(cell.x, cell.y))
                {
                    builder.AppendLine("Transformer-protected circuit.");
                }
            }
            if (RequiresPower(cells[cell.x, cell.y]))
            {
                builder.AppendLine(HasWireAccess(cell) ? "Machine power: wired  Demand " + PowerDemandForKind(cells[cell.x, cell.y]).ToString("0.00") + " kW" : "Machine power: no wire connection");
                builder.AppendLine(MachineAutomationStateText(cell));
            }

            if (automationWire[cell.x, cell.y])
            {
                UpdateAutomationWires();
                builder.AppendLine(automationControlledWire[cell.x, cell.y]
                    ? automationSignalWire[cell.x, cell.y] ? "Automation Wire: green signal" : "Automation Wire: red signal"
                    : "Automation Wire: no signal source");
            }

            if (cells[cell.x, cell.y] == CellKind.SignalSwitch)
            {
                builder.AppendLine(techPowerRegulation ? "Manual automation source. Toggle to send green or red signal." : "Locked: research Power Regulation.");
                builder.AppendLine("Signal " + (automationSwitchState[cell.x, cell.y] ? "green" : "red") + "  Toggles " + signalSwitchesToggled);
                builder.AppendLine(HasAutomationWireAccess(cell) ? "Automation wire linked." : "Needs adjacent or under-floor Automation Wire.");
            }

            if (liquidPipe[cell.x, cell.y])
            {
                builder.AppendLine("Liquid Pipe: " + pipeWater[cell.x, cell.y].ToString("0.0") + "/" + LiquidPipeCapacity.ToString("0") + " kg water");
                if (pipeWater[cell.x, cell.y] > PipePhaseRuptureMinimumMass)
                {
                    if (temperature[cell.x, cell.y] < PipeFreezeTemperature)
                    {
                        builder.AppendLine("Pipe contents freezing: liquid pipe will rupture.");
                    }
                    else if (temperature[cell.x, cell.y] > PipeBoilTemperature)
                    {
                        builder.AppendLine("Pipe contents boiling: liquid pipe will rupture.");
                    }
                    else
                    {
                        builder.AppendLine("Pipe contents stable between " + PipeFreezeTemperature.ToString("0") + " C and " + PipeBoilTemperature.ToString("0") + " C.");
                    }
                }

                builder.AppendLine("Pipe bursts " + pipeBurstEvents + "  Water released " + pipeBurstWater.ToString("0.0") + " kg  Frozen/boiled " + frozenPipeBursts + "/" + boiledPipeBursts);
            }

            if (gasPipe[cell.x, cell.y])
            {
                builder.AppendLine("Gas Pipe: " + GasPipeTotal(cell.x, cell.y).ToString("0.00") + "/" + GasPipeCapacity.ToString("0") + " kg");
                builder.AppendLine("Pipe O2 " + gasPipeOxygen[cell.x, cell.y].ToString("0.00") + " CO2 " + gasPipeCarbonDioxide[cell.x, cell.y].ToString("0.00") + " PO2 " + gasPipePollutedOxygen[cell.x, cell.y].ToString("0.00"));
                builder.AppendLine("Pipe H2 " + gasPipeHydrogen[cell.x, cell.y].ToString("0.00") + " Cl " + gasPipeChlorine[cell.x, cell.y].ToString("0.00") + " NG " + gasPipeNaturalGas[cell.x, cell.y].ToString("0.00") + " Purity " + Mathf.RoundToInt(PipeHydrogenPurity(cell.x, cell.y) * 100f) + "%");
            }

            if (shippingRail[cell.x, cell.y])
            {
                builder.AppendLine(shippingRailKind[cell.x, cell.y] == LooseResourceKind.None
                    ? "Shipping Rail: empty"
                    : "Shipping Rail: " + shippingRailAmount[cell.x, cell.y].ToString("0.0") + "/" + ShippingRailCapacity.ToString("0") + " kg " + LooseResourceLabel(shippingRailKind[cell.x, cell.y]));
            }

            if (waterMass[cell.x, cell.y] > 0.5f && cells[cell.x, cell.y] != CellKind.Water)
            {
                builder.AppendLine("Standing water " + waterMass[cell.x, cell.y].ToString("0") + " kg");
                builder.AppendLine(LiquidFlowStateText(cell));
                if (temperature[cell.x, cell.y] > WaterEvaporationTemperature)
                {
                    builder.AppendLine("Standing water: evaporating into steam.");
                }
            }

            if (cells[cell.x, cell.y] == CellKind.LiquidPipeSensor)
            {
                builder.AppendLine("Liquid pipe sensor. Sends green when pipe water is at least " + LiquidSensorThreshold.ToString("0.#") + " kg.");
                builder.AppendLine(LiquidSensorSignalActive(cell.x, cell.y) ? "Sensor signal: green." : "Sensor signal: red.");
                builder.AppendLine(automationWire[cell.x, cell.y] || HasAutomationControl(cell) ? "Automation wire linked." : "Needs adjacent Automation Wire.");
            }

            if (cells[cell.x, cell.y] == CellKind.LiquidShutoff)
            {
                UpdateAutomationWires();
                builder.AppendLine("Automation-controlled liquid shutoff. Green allows pipe flow; red stops it.");
                builder.AppendLine(IsConduitShutoffOpen(cell) ? "Valve state: open." : "Valve state: closed.");
                builder.AppendLine(HasAutomationControl(cell)
                    ? HasAutomationSignal(cell) ? "Automation: green signal." : "Automation: red signal."
                    : "Automation: no signal, defaults open.");
                builder.AppendLine("Automated conduit flow " + automatedConduitFlow.ToString("0.0") + " kg.");
            }

            if (cells[cell.x, cell.y] == CellKind.GasPipeSensor)
            {
                builder.AppendLine("Gas pipe sensor. Sends green on hydrogen or pipe pressure over " + GasSensorPressureThreshold.ToString("0.#") + " kg.");
                builder.AppendLine(GasSensorSignalActive(cell.x, cell.y) ? "Sensor signal: green." : "Sensor signal: red.");
                builder.AppendLine(automationWire[cell.x, cell.y] || HasAutomationControl(cell) ? "Automation wire linked." : "Needs adjacent Automation Wire.");
            }

            if (cells[cell.x, cell.y] == CellKind.GasShutoff)
            {
                UpdateAutomationWires();
                builder.AppendLine("Automation-controlled gas shutoff. Green allows gas pipe flow; red stops it.");
                builder.AppendLine(IsConduitShutoffOpen(cell) ? "Valve state: open." : "Valve state: closed.");
                builder.AppendLine(HasAutomationControl(cell)
                    ? HasAutomationSignal(cell) ? "Automation: green signal." : "Automation: red signal."
                    : "Automation: no signal, defaults open.");
                builder.AppendLine("Automated conduit flow " + automatedConduitFlow.ToString("0.0") + " kg.");
            }

            if (cells[cell.x, cell.y] == CellKind.SteamVent)
            {
                builder.AppendLine("Renewable natural vent. Emits hot clean water into an adjacent pocket while active.");
                builder.AppendLine("State: " + NaturalVentStateText(cell));
                builder.AppendLine(TryFindNaturalVentLiquidOutput(cell, out _) ? "Output pocket available." : "Output blocked or full.");
                builder.AppendLine("Renewable water generated " + renewableWaterGenerated.ToString("0.0") + " kg.");
                builder.AppendLine("Use pumps, liquid pipes, insulation, and automation to tame the heat.");
            }

            if (cells[cell.x, cell.y] == CellKind.HydrogenVent)
            {
                builder.AppendLine("Renewable natural vent. Emits hot hydrogen into nearby gas when active.");
                builder.AppendLine("State: " + NaturalVentStateText(cell));
                builder.AppendLine(TryFindNaturalVentGasOutput(cell, out _) ? "Output pressure available." : "Output overpressured or blocked.");
                builder.AppendLine("Renewable hydrogen generated " + renewableHydrogenGenerated.ToString("0.00") + " kg.");
                builder.AppendLine("Pump it through gas pipes to hydrogen filtering or power systems.");
            }

            if (cells[cell.x, cell.y] == CellKind.NaturalGasVent)
            {
                builder.AppendLine("Renewable natural vent. Emits hot natural gas into nearby gas when active.");
                builder.AppendLine("State: " + NaturalVentStateText(cell));
                builder.AppendLine(TryFindNaturalVentGasOutput(cell, out _) ? "Output pressure available." : "Output overpressured or blocked.");
                builder.AppendLine("Renewable natural gas generated " + renewableNaturalGasGenerated.ToString("0.00") + " kg.");
                builder.AppendLine("Pump it through gas pipes to Natural Gas Generators for power.");
            }

            if (IsRepairableEquipment(cells[cell.x, cell.y]))
            {
                int condition = Mathf.RoundToInt(Mathf.Clamp01(equipmentCondition[cell.x, cell.y]) * 100f);
                builder.AppendLine("Condition " + condition + "%  Repairs " + repairsCompleted);
                float overheatSeverity = EquipmentOverheatSeverity(cell.x, cell.y);
                if (overheatSeverity > 0.001f)
                {
                    builder.AppendLine("Overheating: thermal damage severity " + Mathf.RoundToInt(overheatSeverity * 100f) + "%.");
                    builder.AppendLine("Overheat damage " + overheatedEquipmentDamage.ToString("0.0") + "  Failures " + overheatedEquipmentFailures);
                }

                if (IsBrokenEquipment(cell))
                {
                    builder.AppendLine("Broken: disabled until repaired with " + RepairMetalCost.ToString("0.#") + " kg metal.");
                }
                else if (IsEquipmentSubmerged(cell))
                {
                    builder.AppendLine("Submerged: disabled and taking water damage.");
                }
                else if (NeedsRepair(cell))
                {
                    builder.AppendLine("Damaged: Repair restores operation reliability.");
                }
                else
                {
                    builder.AppendLine("Equipment in good condition.");
                }
            }

            builder.AppendLine("O2 " + oxygen[cell.x, cell.y].ToString("0.00") + "   CO2 " + carbonDioxide[cell.x, cell.y].ToString("0.00"));
            builder.AppendLine("PO2 " + pollutedOxygen[cell.x, cell.y].ToString("0.00") + "   H2 " + hydrogen[cell.x, cell.y].ToString("0.00") + "   Steam " + steam[cell.x, cell.y].ToString("0.00") + "   Cl " + chlorine[cell.x, cell.y].ToString("0.00") + "   NG " + naturalGas[cell.x, cell.y].ToString("0.00"));
            builder.AppendLine("Germs " + Mathf.RoundToInt(germs[cell.x, cell.y] * 100f) + "%");
            if (chlorine[cell.x, cell.y] > ChlorineExposureThreshold)
            {
                builder.AppendLine("Chlorine: sterile but toxic without an atmo suit.");
                builder.AppendLine("Sterilized " + chlorineSterilizedGerms.ToString("0.0") + "  Exposure " + chlorineExposureSeconds.ToString("0") + "s  Damage " + chlorineHealthDamage.ToString("0.0"));
            }
            if (IsPollutedWaterOffgasSource(cells[cell.x, cell.y]))
            {
                builder.AppendLine(pollutedWater > PollutedWaterOffgasMinimum
                    ? "Stored polluted water is offgassing here."
                    : "No stored polluted water to offgas.");
                builder.AppendLine("PW offgas " + pollutedWaterOffgassedMass.ToString("0.0") + " kg PO2  Events " + pollutedWaterOffgasEvents + "  Sources " + CountPollutedWaterOffgasSources());
            }
            if (steam[cell.x, cell.y] > 0.02f)
            {
                builder.AppendLine(temperature[cell.x, cell.y] < SteamCondensationTemperature ? "Steam: condensing into water." : "Steam: condenses below " + SteamCondensationTemperature.ToString("0") + " C.");
                builder.AppendLine("Vaporized " + steamEvaporatedMass.ToString("0.0") + " kg  Condensed " + steamCondensedMass.ToString("0.0") + " kg.");
            }
            HatchCritter inspectedHatch = HatchAt(cell);
            if (inspectedHatch != null)
            {
                builder.AppendLine("Critter: Hatch " + inspectedHatch.Name);
                builder.AppendLine("Happiness " + inspectedHatch.Happiness.ToString("0") + "%  Groomed " + inspectedHatch.GroomedSeconds.ToString("0") + "s");
                builder.AppendLine("Coal produced " + inspectedHatch.CoalProduced.ToString("0.0") + " kg");
                builder.AppendLine(IsHatchEdible(looseResourceKind[cell.x, cell.y]) ? "Eating local debris." : "Feeds on loose dirt, algae, or polluted dirt.");
            }

            if (HasLooseResource(cell))
            {
                builder.AppendLine("Loose " + LooseResourceLabel(looseResourceKind[cell.x, cell.y]) + " " + looseResourceAmount[cell.x, cell.y].ToString("0.#") + " kg");
                builder.AppendLine(DryResourceFreeSpace() > 0.01f ? "Sweep target: stores loose debris." : "Sweep blocked: dry storage full.");
            }

            builder.AppendLine("Decor " + Mathf.RoundToInt(DecorScoreAt(cell.x, cell.y) * 100f) + "%");
            RoomInfo room = RoomAt(cell.x, cell.y);
            if (room != null)
            {
                builder.AppendLine("Room " + RoomKindLabel(room.Kind) + "  Size " + room.Tiles + "/" + MaxRecognizedRoomTiles);
                builder.AppendLine(room.Enclosed ? "Room boundary sealed." : "Room open to asteroid.");
                string roomBonus = RoomBonusText(room);
                if (!string.IsNullOrEmpty(roomBonus))
                {
                    builder.AppendLine(roomBonus);
                }
            }
            else if (cells[cell.x, cell.y] == CellKind.ManualAirlock || cells[cell.x, cell.y] == CellKind.InsulatedTile)
            {
                builder.AppendLine("Room boundary: blocks room flood fill.");
            }

            if (cells[cell.x, cell.y] == CellKind.Ice)
            {
                builder.AppendLine(temperature[cell.x, cell.y] > 1f ? "Frozen water: melting into liquid water." : "Frozen water: melts above 1 C.");
                builder.AppendLine("Phase changes melted " + iceMeltedTiles + "  frozen " + waterFrozenTiles);
            }

            if (cells[cell.x, cell.y] == CellKind.Water)
            {
                builder.AppendLine("Liquid water " + waterMass[cell.x, cell.y].ToString("0") + " kg");
                builder.AppendLine(LiquidFlowStateText(cell));
                builder.AppendLine("World flow " + liquidFlowedMass.ToString("0.0") + " kg  Events " + liquidFlowEvents);
                builder.AppendLine(temperature[cell.x, cell.y] > WaterEvaporationTemperature ? "Phase: evaporating into steam." : temperature[cell.x, cell.y] < -1f ? "Phase: freezing into ice." : "Phase: liquid water.");
                builder.AppendLine("Phase changes melted " + iceMeltedTiles + "  frozen " + waterFrozenTiles + "  vaporized " + steamEvaporatedMass.ToString("0.0") + "  condensed " + steamCondensedMass.ToString("0.0"));
                if (waterMass[cell.x, cell.y] <= MoppableSpillMaxMass)
                {
                    builder.AppendLine(IsMoppableSpill(cell) ? "Mop target: shallow spill can be recovered." : "Mop target needs adjacent passable access.");
                    builder.AppendLine(IsPollutedMopCell(cell) ? "Contaminated spill: stores as polluted water." : "Clean spill: stores as water.");
                }
                else
                {
                    builder.AppendLine("Deep water: use a Water Pump.");
                }
            }

            if (cells[cell.x, cell.y] == CellKind.Coal)
            {
                builder.AppendLine("Mineable coal seam. Stores as dry fuel for Coal Generators.");
            }

            if (cells[cell.x, cell.y] == CellKind.Planter)
            {
                builder.AppendLine("Growth " + Mathf.RoundToInt(plantGrowth[cell.x, cell.y] * 100f) + "%");
                builder.AppendLine("Crop " + CropStressReason(cell) + "  Stress " + cropStress[cell.x, cell.y].ToString("0.0") + "/" + CropWiltThresholdSeconds.ToString("0") +
                    (cropStress[cell.x, cell.y] >= CropWiltThresholdSeconds ? "  WILTING" : cropStress[cell.x, cell.y] >= CropStressThresholdSeconds ? "  STIFLED" : string.Empty));
                builder.AppendLine("Crop stifled " + cropStifledSeconds.ToString("0") + "s  Wilt events " + cropsWilted);
                builder.AppendLine(cropTendedSeconds[cell.x, cell.y] > 0f
                    ? "Crop tended: growth boosted for " + cropTendedSeconds[cell.x, cell.y].ToString("0") + "s."
                    : TryFindFarmStationForCrop(cell, out _) ? "Farm Station can tend this crop." : "No Farm Station in range.");
            }

            if (cells[cell.x, cell.y] == CellKind.ResearchStation)
            {
                builder.AppendLine("Research " + researchPoints.ToString("0") + "/32");
                builder.AppendLine("Tech " + TechSummary());
            }

            if (cells[cell.x, cell.y] == CellKind.ManualGenerator)
            {
                builder.AppendLine(HasAutomationControl(cell)
                    ? HasAutomationSignal(cell) ? "Automation: green, generator enabled." : "Automation: red, generator disabled."
                    : "Automation: no control signal.");
            }

            if (cells[cell.x, cell.y] == CellKind.CoalGenerator)
            {
                builder.AppendLine("Burns stored coal into power, CO2, and heat when wired.");
                builder.AppendLine("Coal " + coal.ToString("0.0") + " kg  Generated " + coalPowerGenerated.ToString("0") + " power");
                builder.AppendLine(HasWireAccess(cell) ? "Power wire connected." : "Needs a power wire connection.");
                builder.AppendLine(HasAutomationControl(cell)
                    ? HasAutomationSignal(cell) ? "Automation: green, generator enabled." : "Automation: red, generator disabled."
                    : "Automation: runs while battery charge is below 72%.");
            }

            if (cells[cell.x, cell.y] == CellKind.HydrogenGenerator)
            {
                builder.AppendLine("Burns piped or nearby hydrogen into power and heat when wired.");
                builder.AppendLine("Pipe H2 " + PipedHydrogenAround(cell).ToString("0.00") + " kg  Local H2 " + HydrogenAround(cell, 3).ToString("0.00") + " kg");
                builder.AppendLine("Generated " + hydrogenPowerGenerated.ToString("0") + " power");
                builder.AppendLine(HasWireAccess(cell) ? "Power wire connected." : "Needs a power wire connection.");
                builder.AppendLine(HasAutomationControl(cell)
                    ? HasAutomationSignal(cell) ? "Automation: green, generator enabled." : "Automation: red, generator disabled."
                    : "Automation: runs while battery charge is below 78%.");
            }

            if (cells[cell.x, cell.y] == CellKind.NaturalGasGenerator)
            {
                builder.AppendLine("Burns piped or nearby natural gas into power, CO2, polluted water, and heat.");
                builder.AppendLine("Pipe NG " + PipedNaturalGasAround(cell).ToString("0.00") + " kg  Local NG " + NaturalGasAround(cell, 3).ToString("0.00") + " kg");
                builder.AppendLine("Generated " + naturalGasPowerGenerated.ToString("0") + " power  PW " + pollutedWater.ToString("0.0") + " kg");
                builder.AppendLine(HasWireAccess(cell) ? "Power wire connected." : "Needs a power wire connection.");
                builder.AppendLine(HasAutomationControl(cell)
                    ? HasAutomationSignal(cell) ? "Automation: green, generator enabled." : "Automation: red, generator disabled."
                    : "Automation: runs while battery charge is below 78%.");
            }

            if (cells[cell.x, cell.y] == CellKind.SteamTurbine)
            {
                builder.AppendLine("Converts hot steam into power and recovered clean water.");
                builder.AppendLine("Hot steam " + HotSteamAvailable(cell, SteamTurbineRadius).ToString("0.00") + " kg  Min " + SteamTurbineMinimumTemperature.ToString("0") + " C");
                builder.AppendLine("Generated " + steamTurbinePowerGenerated.ToString("0") + " power  Recovered " + steamTurbineWaterRecovered.ToString("0.0") + " kg water");
                builder.AppendLine(HasWireAccess(cell) ? "Power wire connected." : "Needs a power wire connection.");
                builder.AppendLine(TryFindAdjacentLiquidPipeWithSpace(cell, out _) ? "Recovered water can enter adjacent liquid pipe." : "Recovered water goes to stored clean water.");
                builder.AppendLine(HasAutomationControl(cell)
                    ? HasAutomationSignal(cell) ? "Automation: green, turbine enabled." : "Automation: red, turbine disabled."
                    : "Automation: runs while battery charge is below 78%.");
            }

            if (cells[cell.x, cell.y] == CellKind.SolarPanel)
            {
                builder.AppendLine("Generates renewable power during daylight when exposed to open sky.");
                builder.AppendLine("Sun " + Mathf.RoundToInt(SolarIrradiance() * 100f) + "%  Sky " + (IsSolarPanelSkyExposed(cell) ? "clear" : "blocked"));
                builder.AppendLine("Generated " + solarPowerGenerated.ToString("0") + " power  Blocked " + solarBlockedSeconds.ToString("0") + "s");
                builder.AppendLine(HasWireAccess(cell) ? "Power wire connected." : "Needs a power wire connection.");
                builder.AppendLine(HasAutomationControl(cell)
                    ? HasAutomationSignal(cell) ? "Automation: green, panel enabled." : "Automation: red, panel disabled."
                    : "Automation: runs while battery charge is below 92%.");
            }

            if (cells[cell.x, cell.y] == CellKind.BunkerDoor)
            {
                builder.AppendLine(IsBunkerDoorClosed(cell) ? "Bunker Door closed for meteor shower protection." : "Bunker Door open; sunlight, gas, and duplicants can pass.");
                builder.AppendLine("Meteor timer " + (IsMeteorShowerActive() ? meteorShowerSeconds.ToString("0") + "s active" : meteorCooldownSeconds.ToString("0") + "s to next shower"));
                builder.AppendLine("Blocked " + meteorImpactsBlocked + " impacts  Damage events " + meteorDamageEvents + "  Regolith " + meteorRegolithDeposited.ToString("0") + " kg");
                builder.AppendLine("Gas permeability " + TileGasPermeability(cell.x, cell.y).ToString("0.00") + "  Conductivity " + TileThermalConductivity(cell.x, cell.y).ToString("0.000"));
            }

            if (cells[cell.x, cell.y] == CellKind.SpaceScanner)
            {
                builder.AppendLine("Scans open sky and sends green automation before meteor showers.");
                builder.AppendLine("Sky " + (IsSpaceScannerSkyExposed(cell) ? "clear" : "blocked") + "  Signal " + (SpaceScannerSignalActive(cell.x, cell.y) ? "green" : "red"));
                builder.AppendLine("Warning window " + SpaceScannerWarningSeconds.ToString("0") + "s  Signals " + spaceScannerSignalSeconds.ToString("0") + "s  Blocked " + spaceScannerBlockedSeconds.ToString("0") + "s");
                builder.AppendLine(HasWireAccess(cell) ? "Power wire connected." : "Needs a power wire connection.");
                builder.AppendLine(HasAutomationWireAccess(cell) ? "Automation wire linked." : "Needs adjacent or under-floor Automation Wire.");
            }

            if (cells[cell.x, cell.y] == CellKind.HydrogenFilter)
            {
                builder.AppendLine("Scrubs non-H2 gases out of adjacent gas pipes, leaving H2 as generator fuel.");
                if (TryFindAdjacentMixedHydrogenPipe(cell, out Vector2Int filterPipe))
                {
                    builder.AppendLine("Target pipe H2 " + gasPipeHydrogen[filterPipe.x, filterPipe.y].ToString("0.00") + " kg  Non-H2 " + PipeNonHydrogenTotal(filterPipe.x, filterPipe.y).ToString("0.00") + " kg");
                    builder.AppendLine("Purity " + Mathf.RoundToInt(PipeHydrogenPurity(filterPipe.x, filterPipe.y) * 100f) + "%  Filtered " + hydrogenFilteredGas.ToString("0.0") + " kg");
                }
                else
                {
                    builder.AppendLine("Needs adjacent gas pipe containing mixed hydrogen and other gases.");
                    builder.AppendLine("Filtered " + hydrogenFilteredGas.ToString("0.0") + " kg");
                }

                builder.AppendLine(CanPoweredMachineRun(cell) ? "Powered filter ready." : "Needs power and green automation for filtering.");
            }

            if (cells[cell.x, cell.y] == CellKind.RockCrusher)
            {
                builder.AppendLine("Refines mined metal ore into Refined Metal for advanced machinery.");
                builder.AppendLine("Metal " + metal.ToString("0.0") + " kg  Refined " + refinedMetal.ToString("0.0") + " kg");
                builder.AppendLine("Produced " + refinedMetalProduced.ToString("0.0") + " kg refined metal.");
                builder.AppendLine(CanPoweredMachineRun(cell) ? "Powered and ready to refine." : "Needs power and green automation for refining.");
            }

            if (cells[cell.x, cell.y] == CellKind.AtmoSuitDock)
            {
                builder.AppendLine("Charges atmo suits from local oxygen and power.");
                builder.AppendLine("Suit O2 " + suitOxygen.ToString("0.0") + "/" + SuitOxygenCapacityTotal().ToString("0.0") + " kg  Used " + suitOxygenUsed.ToString("0.0"));
                builder.AppendLine("Checkpoint crossings " + suitCheckpointUses + "  Blocked entries " + suitEntryDenials);
                builder.AppendLine("Adjacent checkpoint " + (HasAdjacentSuitCheckpoint(cell) ? "ready" : "missing"));
                builder.AppendLine(CanPoweredMachineRun(cell) ? "Powered charging dock." : "Needs power and green automation to charge suits.");
                builder.AppendLine(TryFindSuitDockOxygenSource(cell, out _) ? "Oxygen source available." : "Needs breathable oxygen nearby.");
            }

            if (cells[cell.x, cell.y] == CellKind.AtmoSuitCheckpoint)
            {
                builder.AppendLine("Equips suits when duplicants cross into unsafe gas or temperature, then returns them on safe exit.");
                builder.AppendLine("Adjacent dock " + (HasAdjacentSuitDock(cell) ? "linked" : "missing") + "  Suit O2 " + suitOxygen.ToString("0.0") + "/" + SuitOxygenCapacityTotal().ToString("0.0") + " kg");
                builder.AppendLine("Checkpoint uses " + suitCheckpointUses + "  Blocked entries " + suitEntryDenials);
                builder.AppendLine("Suited workers " + CountSuitedWorkers());
                builder.AppendLine(suitOxygen > SuitCheckpointMinimumCharge ? "Charged suits available." : "Needs charged suit oxygen from a powered dock.");
            }

            if (cells[cell.x, cell.y] == CellKind.PrintingPod)
            {
                builder.AppendLine("Prints new duplicants as the colony stabilizes.");
                builder.AppendLine("Print charge " + Mathf.RoundToInt(printingPodProgress * 100f) + "%  Crew " + workers.Count + "/" + MaxWorkers);
                builder.AppendLine(workers.Count >= MaxWorkers ? "Crew limit reached." : TryFindOpenSpawnNear(cell, out _) ? "Adjacent spawn space available." : "Needs open adjacent space.");
            }

            if (cells[cell.x, cell.y] == CellKind.InsulatedTile)
            {
                builder.AppendLine("Solid insulated wall. Blocks movement and gas, and sharply slows heat transfer.");
                builder.AppendLine("Conductivity " + ThermalConductivity(CellKind.InsulatedTile).ToString("0.000") + "  Capacity " + ThermalCapacity(CellKind.InsulatedTile).ToString("0.0"));
            }

            if (cells[cell.x, cell.y] == CellKind.WaterPump)
            {
                builder.AppendLine(HasAdjacentWater(cell) ? "Pump has a water source." : "Pump needs adjacent water.");
                if (HasLiquidPipeAccess(cell))
                {
                    builder.AppendLine(CanPoweredMachineRun(cell) ? "Powered plumbing sends water into adjacent pipes." : "Pipe connected; needs power and green automation for automatic pumping.");
                }
            }

            if (cells[cell.x, cell.y] == CellKind.BottleEmptier)
            {
                builder.AppendLine("Empties stored bottled liquid into an adjacent pit.");
                builder.AppendLine("Stored clean " + water.ToString("0.0") + " kg  Polluted " + pollutedWater.ToString("0.0") + " kg");
                builder.AppendLine(TryFindBottleEmptierOutput(cell, out _) ? "Output cell available." : "Output blocked or full.");
                builder.AppendLine("Emptied " + bottleEmptiedLiquid.ToString("0.0") + " kg.");
            }

            if (cells[cell.x, cell.y] == CellKind.MicrobeMusher)
            {
                builder.AppendLine(techFoodPreparation ? "Consumes water, dirt, and power to cook food." : "Locked: research Food Preparation.");
            }

            if (cells[cell.x, cell.y] == CellKind.FarmStation)
            {
                builder.AppendLine(techFoodPreparation ? "Tends nearby planters to accelerate crop growth." : "Locked: research Food Preparation.");
                builder.AppendLine("Range " + FarmStationRange + " tiles  Targets " + CountCropTendingTargets());
                builder.AppendLine("Fertilizer PD " + pollutedDirt.ToString("0.0") + " kg or dirt " + dirt.ToString("0.0") + " kg.");
                builder.AppendLine("Crops tended " + cropsTended + "  Boost " + CropTendedGrowthMultiplier.ToString("0.0") + "x for " + CropTendedSeconds.ToString("0") + "s.");
            }

            if (cells[cell.x, cell.y] == CellKind.Refrigerator)
            {
                builder.AppendLine(IsPoweredRefrigeratorAt(cell.x, cell.y) ? "Refrigerated food storage active." : "Needs power and green automation to slow spoilage.");
                builder.AppendLine("Stored food freshness " + Mathf.RoundToInt(foodFreshness * 100f) + "%");
                builder.AppendLine("Stale meals " + staleMealsEaten + "  Food poisonings " + foodPoisoningCases);
            }

            if (cells[cell.x, cell.y] == CellKind.StorageBin)
            {
                builder.AppendLine("Adds " + StorageBinCapacity.ToString("0") + " dry storage.");
                builder.AppendLine("Dry resources " + DryResourceAmount().ToString("0") + "/" + DryResourceCapacity().ToString("0"));
                builder.AppendLine("Polluted dirt " + pollutedDirt.ToString("0.0") + " kg");
            }

            if (cells[cell.x, cell.y] == CellKind.AutoSweeper)
            {
                builder.AppendLine(techPowerRegulation ? "Automatically stores nearby loose debris when powered." : "Locked: research Power Regulation.");
                builder.AppendLine("Range " + AutoSweeperRange + " tiles  Targets " + CountAutoSweeperTargets(cell));
                builder.AppendLine("Auto-swept " + autoSweptResources.ToString("0.0") + " kg  Dry space " + DryResourceFreeSpace().ToString("0") + " kg");
                builder.AppendLine(CanPoweredMachineRun(cell) ? "Powered sweeper ready." : HasWireAccess(cell) ? "Wired, but lacks power or green automation." : "Needs adjacent or under-floor power wire.");
            }

            if (cells[cell.x, cell.y] == CellKind.ConveyorLoader)
            {
                builder.AppendLine(techPowerRegulation ? "Loads stored dry resources into adjacent Shipping Rail." : "Locked: research Power Regulation.");
                builder.AppendLine("Rail " + (HasShippingRailAccess(cell) ? "linked" : "missing") + "  Shipped " + conveyorShippedResources.ToString("0.0") + " kg");
                builder.AppendLine(CanPoweredMachineRun(cell) ? "Powered loader ready." : HasWireAccess(cell) ? "Wired, but lacks power or green automation." : "Needs adjacent or under-floor power wire.");
            }

            if (cells[cell.x, cell.y] == CellKind.ConveyorChute)
            {
                builder.AppendLine("Drops Shipping Rail packets back into the world for ranching, generators, or remote supply.");
                builder.AppendLine("Adjacent packet " + (TryFindAdjacentShippingRailWithPacket(cell, out _) ? "available" : "missing") + "  Shipped " + conveyorShippedResources.ToString("0.0") + " kg");
            }

            if (cells[cell.x, cell.y] == CellKind.LiquidReservoir)
            {
                builder.AppendLine("Stores piped clean water and buffers plumbing before vents or machines.");
                builder.AppendLine("Stored water " + liquidReservoirWater[cell.x, cell.y].ToString("0.0") + "/" + LiquidReservoirCapacity.ToString("0") + " kg");
                if (liquidReservoirWater[cell.x, cell.y] > PipePhaseRuptureMinimumMass)
                {
                    if (temperature[cell.x, cell.y] < PipeFreezeTemperature)
                    {
                        builder.AppendLine("Reservoir contents freezing: tank will rupture.");
                    }
                    else if (temperature[cell.x, cell.y] > PipeBoilTemperature)
                    {
                        builder.AppendLine("Reservoir contents boiling: tank will rupture.");
                    }
                    else
                    {
                        builder.AppendLine("Reservoir contents stable between " + PipeFreezeTemperature.ToString("0") + " C and " + PipeBoilTemperature.ToString("0") + " C.");
                    }
                }

                builder.AppendLine("Reservoir bursts " + reservoirBurstEvents + "  Water released " + reservoirBurstWater.ToString("0.0") + " kg  Frozen/boiled " + frozenReservoirBursts + "/" + boiledReservoirBursts);
                builder.AppendLine(TryFindAdjacentLiquidPipeWithWater(cell, out _) ? "Input pipe has water." : "Needs adjacent liquid pipe with water.");
                builder.AppendLine(TryFindAdjacentLiquidPipeWithSpace(cell, out _) ? "Output pipe space available." : "Needs adjacent pipe with space for output.");
            }

            if (cells[cell.x, cell.y] == CellKind.GasReservoir)
            {
                float storedGas = GasReservoirTotal(cell.x, cell.y);
                builder.AppendLine("Stores mixed piped gas and buffers ventilation networks.");
                builder.AppendLine("Stored gas " + storedGas.ToString("0.00") + "/" + GasReservoirCapacity.ToString("0") + " kg");
                builder.AppendLine("O2 " + gasReservoirOxygen[cell.x, cell.y].ToString("0.00") + " CO2 " + gasReservoirCarbonDioxide[cell.x, cell.y].ToString("0.00") + " PO2 " + gasReservoirPollutedOxygen[cell.x, cell.y].ToString("0.00") + " H2 " + gasReservoirHydrogen[cell.x, cell.y].ToString("0.00") + " Cl " + gasReservoirChlorine[cell.x, cell.y].ToString("0.00") + " NG " + gasReservoirNaturalGas[cell.x, cell.y].ToString("0.00"));
                builder.AppendLine(TryFindAdjacentGasPipeWithGas(cell, out _) ? "Input gas pipe has packets." : "Needs adjacent gas pipe with gas.");
                builder.AppendLine(TryFindAdjacentGasPipeWithSpace(cell, out _) ? "Output pipe space available." : "Needs adjacent pipe with space for output.");
            }

            if (cells[cell.x, cell.y] == CellKind.SmartBattery)
            {
                UpdateAutomationWires();
                builder.AppendLine("Automation power storage. Sends green below " + Mathf.RoundToInt(SmartBatteryLowThreshold * 100f) + "% power.");
                builder.AppendLine("Signal " + (SmartBatterySignalActive() ? "green" : "red") + "  Power " + power.ToString("0") + "/" + maxPower.ToString("0"));
                builder.AppendLine("Controlled generators " + CountAutomationControlledGenerators());
            }

            if (cells[cell.x, cell.y] == CellKind.PowerTransformer)
            {
                builder.AppendLine("Raises adjacent power wire safe load from " + WireSafeLoad.ToString("0.0") + " to " + (WireSafeLoad + PowerTransformerLoadBonus).ToString("0.0") + " kW.");
                builder.AppendLine(HasWireAccess(cell) ? "Transformer connected to wire." : "Needs adjacent or under-floor power wire.");
                builder.AppendLine("Protected delivery " + transformedPowerDelivered.ToString("0.0") + " kWs  Overload time " + overloadedWireSeconds.ToString("0") + "s.");
            }

            if (cells[cell.x, cell.y] == CellKind.MessTable)
            {
                builder.AppendLine("Dining station. Hungry duplicants eat here for stress relief.");
                builder.AppendLine("Table meals " + mealsEatenAtTable + "/" + Mathf.Max(1, workers.Count));
            }

            if (cells[cell.x, cell.y] == CellKind.DecorPlant)
            {
                builder.AppendLine("Decor plant. Improves nearby morale and slows stress gain.");
                builder.AppendLine("Radius " + DecorPlantRadius + " tiles.");
            }

            if (cells[cell.x, cell.y] == CellKind.RanchingStation)
            {
                HatchCritter targetHatch = FindGroomableHatch(cell);
                builder.AppendLine("Ranching station. Auto-queues grooming for nearby hatches.");
                builder.AppendLine("Nearby groom target " + (targetHatch == null ? "none" : targetHatch.Name) + "  Range " + HatchGroomRange + " tiles");
                builder.AppendLine("Hatches " + hatches.Count + "  Groomed " + CountGroomedHatches() + "  Sessions " + hatchesGroomed);
                builder.AppendLine("Hatch coal " + hatchCoalProduced.ToString("0.0") + " kg from loose debris.");
            }

            if (cells[cell.x, cell.y] == CellKind.LiquidVent)
            {
                builder.AppendLine(HasLiquidPipeAccess(cell) ? "Liquid vent is connected to pipe." : "Liquid vent needs an adjacent pipe.");
                builder.AppendLine(TryFindLiquidVentOutput(cell, out _) ? "Output cell available." : "Output blocked or full.");
            }

            if (cells[cell.x, cell.y] == CellKind.GasPump)
            {
                builder.AppendLine(HasGasPipeAccess(cell) ? "Gas pump is connected to pipe." : "Gas pump needs adjacent gas pipe.");
                builder.AppendLine(CanPoweredMachineRun(cell) ? "Powered pump moves nearby gas into pipes." : "Needs power and green automation for automatic ventilation.");
                builder.AppendLine(TryFindGasSource(cell, out _) ? "Nearby gas source available." : "No gas source nearby.");
            }

            if (cells[cell.x, cell.y] == CellKind.GasVent)
            {
                builder.AppendLine(HasGasPipeAccess(cell) ? "Gas vent is connected to pipe." : "Gas vent needs adjacent gas pipe.");
                builder.AppendLine(TryFindGasVentOutput(cell, out _) ? "Output pressure available." : "Output overpressured or blocked.");
            }

            if (cells[cell.x, cell.y] == CellKind.Electrolyzer)
            {
                builder.AppendLine(techAirSystems ? "Consumes water and power to produce oxygen and hydrogen." : "Locked: research Air Systems.");
                builder.AppendLine(HasWireAccess(cell) ? "Power: wired" : "Power: no wire connection");
                builder.AppendLine(HasMachineWaterFeed(cell) ? "Water feed available." : "Needs stored water or adjacent liquid pipe.");
            }

            if (cells[cell.x, cell.y] == CellKind.CarbonSkimmer)
            {
                builder.AppendLine(techAirSystems ? "Consumes water and power to scrub CO2 into polluted water." : "Locked: research Air Systems.");
                builder.AppendLine("Polluted water " + pollutedWater.ToString("0.0") + " kg");
                builder.AppendLine(HasMachineWaterFeed(cell) ? "Water feed available." : "Needs stored water or adjacent liquid pipe.");
            }

            if (cells[cell.x, cell.y] == CellKind.WaterSieve)
            {
                builder.AppendLine(techAirSystems ? "Filters polluted water into clean water." : "Locked: research Air Systems.");
                builder.AppendLine("Polluted water " + pollutedWater.ToString("0.0") + " kg  Recycled " + recycledWater.ToString("0.0") + " kg");
                builder.AppendLine(dirt > 0.05f ? "Filtration dirt available." : "Needs dirt for filtration.");
                builder.AppendLine(TryFindAdjacentLiquidPipeWithSpace(cell, out _) ? "Clean output can enter adjacent liquid pipe." : "Clean output goes to stored water.");
            }

            if (cells[cell.x, cell.y] == CellKind.AirDeodorizer)
            {
                builder.AppendLine("Cleans polluted oxygen in a small radius.");
            }

            if (cells[cell.x, cell.y] == CellKind.MedicalCot)
            {
                builder.AppendLine("Treats sickness, germ exposure, and injuries.");
                builder.AppendLine("Emergency rescues " + rescuesCompleted + ". Incapacitated " + CountIncapacitatedWorkers() + ".");
            }

            if (cells[cell.x, cell.y] == CellKind.Outhouse)
            {
                builder.AppendLine("Sanitation station. Use adds local germs, polluted oxygen, and polluted dirt.");
                builder.AppendLine("Polluted dirt " + pollutedDirt.ToString("0.0") + " kg");
            }

            if (cells[cell.x, cell.y] == CellKind.WashBasin)
            {
                builder.AppendLine("Hygiene station. Duplicants wash germy hands here before sickness builds.");
                builder.AppendLine("Clean water " + water.ToString("0.0") + " kg  Polluted water " + pollutedWater.ToString("0.0") + " kg");
                builder.AppendLine("Hands washed " + handsWashed + "  Uses " + WashBasinWaterUse.ToString("0.0") + " kg water.");
                builder.AppendLine(water > 0.05f ? "Ready to wash hands." : "Needs clean water.");
                builder.AppendLine(RoomKindAt(cell.x, cell.y) == RoomKind.Washroom ? "Washroom hygiene bonus active." : "Place with an outhouse to form a Washroom.");
            }

            if (cells[cell.x, cell.y] == CellKind.Compost)
            {
                builder.AppendLine("Processes polluted dirt into reusable dirt.");
                builder.AppendLine("Polluted dirt " + pollutedDirt.ToString("0.0") + " kg");
                builder.AppendLine("Composted " + compostedPollutedDirt.ToString("0.0") + "/6 kg");
            }

            if (cells[cell.x, cell.y] == CellKind.MassageTable)
            {
                builder.AppendLine("Stress relief station. One duplicant relaxes here at a time.");
            }

            if (cells[cell.x, cell.y] == CellKind.ManualAirlock)
            {
                builder.AppendLine(airlockOpen[cell.x, cell.y] ? "Door state: open. Duplicants and gas can pass." : "Door state: closed. Pathing and pressure are sealed.");
                builder.AppendLine("Gas permeability " + TileGasPermeability(cell.x, cell.y).ToString("0.00") + "  Conductivity " + TileThermalConductivity(cell.x, cell.y).ToString("0.000"));
                builder.AppendLine("Door toggles " + airlockToggles + ".");
            }

            if (cells[cell.x, cell.y] == CellKind.SpaceHeater)
            {
                builder.AppendLine("Consumes power to warm cold rooms.");
            }

            if (cells[cell.x, cell.y] == CellKind.ThermoRegulator)
            {
                builder.AppendLine(techPowerRegulation ? "Consumes power to cool overheated rooms." : "Locked: research Power Regulation.");
            }

            if (TryGetDeconstructTarget(cell, out CellKind deconstructKind, out bool removePower, out bool removeAutomation, out bool removeLiquid, out bool removeGas, out bool removeShipping))
            {
                Job preview = new Job(JobType.Deconstruct, cell, 1f)
                {
                    BuildKind = deconstructKind,
                    RemovePowerWire = removePower,
                    RemoveAutomationWire = removeAutomation,
                    RemoveLiquidPipe = removeLiquid,
                    RemoveGasPipe = removeGas,
                    RemoveShippingRail = removeShipping
                };
                builder.AppendLine("Deconstruct target: " + DeconstructTargetLabel(preview) + " returns about half its materials.");
            }

            Job job = FindAnyJobAt(cell);
            if (job != null)
            {
                builder.AppendLine("Job: " + JobLabel(job));
                if (!string.IsNullOrEmpty(job.TargetWorkerName))
                {
                    builder.AppendLine("Target " + job.TargetWorkerName);
                }

                builder.AppendLine("Priority " + JobPriority(job));
                builder.AppendLine(JobWaitText(job));
                builder.AppendLine("Progress " + Mathf.RoundToInt(job.Progress / job.WorkRequired * 100f) + "%");
                builder.AppendLine(JobReachabilityText(job));
            }

            Worker worker = WorkerAt(cell);
            if (worker != null)
            {
                builder.AppendLine(worker.Name + " - " + worker.Activity);
                builder.AppendLine("Health " + worker.Health.ToString("0") + "%  Calories " + worker.Calories.ToString("0"));
                builder.AppendLine("Stress " + worker.Stress.ToString("0") + "%  Fatigue " + worker.Fatigue.ToString("0") + "%");
                builder.AppendLine("Bladder " + worker.Bladder.ToString("0") + "%  Exposure " + worker.GermExposure.ToString("0") + "%");
                builder.AppendLine("Sickness " + worker.Sickness.ToString("0") + "%");
                if (worker.HeatExposure > 0.5f || worker.ChillExposure > 0.5f)
                {
                    builder.AppendLine("Thermal exposure heat " + worker.HeatExposure.ToString("0") + "%  chill " + worker.ChillExposure.ToString("0") + "%");
                    builder.AppendLine("Thermal injuries heat/chill " + heatStrokeCases + "/" + hypothermiaCases + "  Damage " + thermalHealthDamage.ToString("0.0"));
                }

                builder.AppendLine(WorkerSkillText(worker));
                builder.AppendLine(WorkerMoraleText(worker));
                if (foodFreshness < StaleFoodFreshnessThreshold)
                {
                    builder.AppendLine("Food risk " + Mathf.RoundToInt(foodFreshness * 100f) + "% fresh  Stale meals " + staleMealsEaten + "  Poisonings " + foodPoisoningCases);
                }

                builder.AppendLine("Local decor " + Mathf.RoundToInt(DecorScoreAt(worker.Cell.x, worker.Cell.y) * 100f) + "%");
                if (worker.Health <= 0f)
                {
                    builder.AppendLine("Incapacitated " + worker.IncapacitatedSeconds.ToString("0.0") + "s. Needs rescue to a Medical Cot.");
                }

                if (worker.StressBreakSeconds > 0f)
                {
                    builder.AppendLine("Stress break " + worker.StressBreakSeconds.ToString("0.0") + "s remaining");
                }
            }

            return builder.ToString();
        }

        private string TechSummary()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(techAirSystems ? "Air" : "Air " + Mathf.Clamp(8f - researchPoints, 0f, 8f).ToString("0") + "RP");
            builder.Append(" | ");
            builder.Append(techFoodPreparation ? "Food" : "Food " + Mathf.Clamp(16f - researchPoints, 0f, 16f).ToString("0") + "RP");
            builder.Append(" | ");
            builder.Append(techPowerRegulation ? "Power" : "Power " + Mathf.Clamp(28f - researchPoints, 0f, 28f).ToString("0") + "RP");
            return builder.ToString();
        }

        private string WorkerSummary()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < workers.Count; i++)
            {
                if (i > 0)
                {
                    builder.Append(" | ");
                }

                builder.Append(workers[i].Name);
                builder.Append(": ");
                builder.Append(workers[i].Activity);
                builder.Append(" H");
                builder.Append(workers[i].Health.ToString("0"));
                if (workers[i].Health <= 0f)
                {
                    builder.Append(" Down");
                    builder.Append(workers[i].IncapacitatedSeconds.ToString("0"));
                    continue;
                }

                builder.Append(" Lv");
                builder.Append(WorkerSkillLevel(workers[i]));
                float moraleDeficit = WorkerMoraleDeficit(workers[i]);
                if (moraleDeficit > 0.5f)
                {
                    builder.Append(" Mor-");
                    builder.Append(moraleDeficit.ToString("0.0"));
                }
                if (workers[i].SuitEquipped)
                {
                    builder.Append(" Suit");
                }

                builder.Append(" F");
                builder.Append(workers[i].Fatigue.ToString("0"));
                if (workers[i].StressBreakSeconds > 0f)
                {
                    builder.Append(" Break");
                    builder.Append(workers[i].StressBreakSeconds.ToString("0"));
                    continue;
                }

                if (workers[i].Stress > 50f)
                {
                    builder.Append(" S");
                    builder.Append(workers[i].Stress.ToString("0"));
                }

                if (workers[i].Bladder > 55f)
                {
                    builder.Append(" B");
                    builder.Append(workers[i].Bladder.ToString("0"));
                }

                if (workers[i].GermExposure > 18f)
                {
                    builder.Append(" Germ");
                    builder.Append(workers[i].GermExposure.ToString("0"));
                }

                if (workers[i].Sickness > 1f)
                {
                    builder.Append(" Sick");
                    builder.Append(workers[i].Sickness.ToString("0"));
                }

                if (workers[i].HeatExposure > 5f)
                {
                    builder.Append(" Heat");
                    builder.Append(workers[i].HeatExposure.ToString("0"));
                }
                else if (workers[i].ChillExposure > 5f)
                {
                    builder.Append(" Chill");
                    builder.Append(workers[i].ChillExposure.ToString("0"));
                }
            }

            return builder.ToString();
        }

        private float MaxWorkerGermExposure()
        {
            float maxExposure = 0f;
            foreach (Worker worker in workers)
            {
                if (worker.Health > 0f && worker.GermExposure > maxExposure)
                {
                    maxExposure = worker.GermExposure;
                }
            }

            return maxExposure;
        }

        private int CountThermallyExposedWorkers()
        {
            int count = 0;
            foreach (Worker worker in workers)
            {
                if (worker.Health > 0f && Mathf.Max(worker.HeatExposure, worker.ChillExposure) > 5f)
                {
                    count++;
                }
            }

            return count;
        }

        private int CountCriticalThermalExposureWorkers()
        {
            int count = 0;
            foreach (Worker worker in workers)
            {
                if (worker.Health > 0f && Mathf.Max(worker.HeatExposure, worker.ChillExposure) >= ThermalInjuryExposureThreshold)
                {
                    count++;
                }
            }

            return count;
        }

        private Worker WorkerAt(Vector2Int cell)
        {
            foreach (Worker worker in workers)
            {
                if (worker.Cell == cell)
                {
                    return worker;
                }
            }

            return null;
        }

        private float AverageGas(float[,] gas)
        {
            float total = 0f;
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (IsPassable(x, y))
                    {
                        total += gas[x, y];
                        count++;
                    }
                }
            }

            return count == 0 ? 0f : total / count;
        }

        private float AverageTemperature()
        {
            float total = 0f;
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (IsPassable(x, y))
                    {
                        total += temperature[x, y];
                        count++;
                    }
                }
            }

            return count == 0 ? 22f : total / count;
        }

        private int CountUnsafeTemperatureTiles()
        {
            int count = 0;
            for (int y = 0; y < WorldHeight; y++)
            {
                for (int x = 0; x < WorldWidth; x++)
                {
                    if (IsPassable(x, y) && (temperature[x, y] < 0f || temperature[x, y] > 52f))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private bool SpendCost(BuildSpec spec)
        {
            if (dirt < spec.Dirt || metal < spec.Metal || algae < spec.Algae || refinedMetal < spec.RefinedMetal)
            {
                return false;
            }

            dirt -= spec.Dirt;
            metal -= spec.Metal;
            algae -= spec.Algae;
            refinedMetal -= spec.RefinedMetal;
            return true;
        }

        private void RefundJobCost(Job job)
        {
            dirt += job.DirtCost;
            metal += job.MetalCost;
            algae += job.AlgaeCost;
            refinedMetal += job.RefinedMetalCost;
        }

        private bool IsBuildUnlocked(CellKind kind, bool showLog)
        {
            if (kind == CellKind.MicrobeMusher && !techFoodPreparation)
            {
                if (showLog)
                {
                    Log("Research Food Preparation before building a Microbe Musher.");
                }

                return false;
            }

            if (kind == CellKind.FarmStation && !techFoodPreparation)
            {
                if (showLog)
                {
                    Log("Research Food Preparation before building a Farm Station.");
                }

                return false;
            }

            if (kind == CellKind.Refrigerator && !techFoodPreparation)
            {
                if (showLog)
                {
                    Log("Research Food Preparation before building a Refrigerator.");
                }

                return false;
            }

            if (kind == CellKind.SmartBattery && !techPowerRegulation)
            {
                if (showLog)
                {
                    Log("Research Power Regulation before building a Smart Battery.");
                }

                return false;
            }

            if (kind == CellKind.PowerTransformer && !techPowerRegulation)
            {
                if (showLog)
                {
                    Log("Research Power Regulation before building a Power Transformer.");
                }

                return false;
            }

            if (kind == CellKind.AutoSweeper && !techPowerRegulation)
            {
                if (showLog)
                {
                    Log("Research Power Regulation before building an Auto-Sweeper.");
                }

                return false;
            }

            if ((kind == CellKind.ConveyorLoader || kind == CellKind.ConveyorChute) && !techPowerRegulation)
            {
                if (showLog)
                {
                    Log("Research Power Regulation before building conveyor shipping.");
                }

                return false;
            }

            if (kind == CellKind.SignalSwitch && !techPowerRegulation)
            {
                if (showLog)
                {
                    Log("Research Power Regulation before building a Signal Switch.");
                }

                return false;
            }

            if ((kind == CellKind.LiquidPipeSensor || kind == CellKind.LiquidShutoff) && !techPowerRegulation)
            {
                if (showLog)
                {
                    Log("Research Power Regulation before building conduit automation.");
                }

                return false;
            }

            if ((kind == CellKind.GasPipeSensor || kind == CellKind.GasShutoff) && (!techPowerRegulation || !techAirSystems))
            {
                if (showLog)
                {
                    Log("Research Air Systems and Power Regulation before building gas conduit automation.");
                }

                return false;
            }

            if (kind == CellKind.CoalGenerator && !techPowerRegulation)
            {
                if (showLog)
                {
                    Log("Research Power Regulation before building a Coal Generator.");
                }

                return false;
            }

            if (kind == CellKind.HydrogenGenerator && !techPowerRegulation)
            {
                if (showLog)
                {
                    Log("Research Power Regulation before building a Hydrogen Generator.");
                }

                return false;
            }

            if (kind == CellKind.NaturalGasGenerator && !techPowerRegulation)
            {
                if (showLog)
                {
                    Log("Research Power Regulation before building a Natural Gas Generator.");
                }

                return false;
            }

            if (kind == CellKind.SteamTurbine && !techPowerRegulation)
            {
                if (showLog)
                {
                    Log("Research Power Regulation before building a Steam Turbine.");
                }

                return false;
            }

            if (kind == CellKind.SolarPanel && !techPowerRegulation)
            {
                if (showLog)
                {
                    Log("Research Power Regulation before building a Solar Panel.");
                }

                return false;
            }

            if (kind == CellKind.BunkerDoor && !techPowerRegulation)
            {
                if (showLog)
                {
                    Log("Research Power Regulation before building a Bunker Door.");
                }

                return false;
            }

            if (kind == CellKind.SpaceScanner && !techPowerRegulation)
            {
                if (showLog)
                {
                    Log("Research Power Regulation before building a Space Scanner.");
                }

                return false;
            }

            if (kind == CellKind.HydrogenFilter && !techAirSystems)
            {
                if (showLog)
                {
                    Log("Research Air Systems before building a Hydrogen Filter.");
                }

                return false;
            }

            if (kind == CellKind.RockCrusher && !techPowerRegulation)
            {
                if (showLog)
                {
                    Log("Research Power Regulation before building a Rock Crusher.");
                }

                return false;
            }

            if (kind == CellKind.AtmoSuitDock && !techPowerRegulation)
            {
                if (showLog)
                {
                    Log("Research Power Regulation before building an Atmo Suit Dock.");
                }

                return false;
            }

            if (kind == CellKind.AtmoSuitCheckpoint && !techPowerRegulation)
            {
                if (showLog)
                {
                    Log("Research Power Regulation before building an Atmo Suit Checkpoint.");
                }

                return false;
            }

            if (kind == CellKind.InsulatedTile && !techPowerRegulation)
            {
                if (showLog)
                {
                    Log("Research Power Regulation before building Insulated Tiles.");
                }

                return false;
            }

            if (kind == CellKind.PrintingPod && !techPowerRegulation)
            {
                if (showLog)
                {
                    Log("Research Power Regulation before building another Printing Pod.");
                }

                return false;
            }

            if (kind == CellKind.AirDeodorizer && !techAirSystems)
            {
                if (showLog)
                {
                    Log("Research Air Systems before building an Air Deodorizer.");
                }

                return false;
            }

            if ((kind == CellKind.GasPump || kind == CellKind.GasVent || kind == CellKind.GasReservoir) && !techAirSystems)
            {
                if (showLog)
                {
                    Log("Research Air Systems before building ventilation.");
                }

                return false;
            }

            if ((kind == CellKind.Electrolyzer || kind == CellKind.CarbonSkimmer || kind == CellKind.WaterSieve) && !techAirSystems)
            {
                if (showLog)
                {
                    Log("Research Air Systems before building advanced atmosphere machines.");
                }

                return false;
            }

            if (kind == CellKind.ThermoRegulator && !techPowerRegulation)
            {
                if (showLog)
                {
                    Log("Research Power Regulation before building a Thermo Regulator.");
                }

                return false;
            }

            if (kind == CellKind.RanchingStation && !techFoodPreparation)
            {
                if (showLog)
                {
                    Log("Research Food Preparation before building a Ranching Station.");
                }

                return false;
            }

            return true;
        }

        private bool HasAdjacentWater(Vector2Int cell)
        {
            return TryFindAdjacentWater(cell, out _);
        }

        private bool HasLiquidPipeAccess(Vector2Int cell)
        {
            return IsLiquidPipeCell(cell.x, cell.y) ||
                IsLiquidPipeCell(cell.x + 1, cell.y) ||
                IsLiquidPipeCell(cell.x - 1, cell.y) ||
                IsLiquidPipeCell(cell.x, cell.y + 1) ||
                IsLiquidPipeCell(cell.x, cell.y - 1);
        }

        private bool HasMachineWaterFeed(Vector2Int cell)
        {
            return water > 0.05f || TryFindAdjacentLiquidPipeWithWater(cell, out _);
        }

        private bool IsLiquidPipeCell(int x, int y)
        {
            return IsInside(x, y) && liquidPipe[x, y];
        }

        private bool TryFindAdjacentLiquidPipeWithSpace(Vector2Int cell, out Vector2Int pipe)
        {
            return TryFindAdjacentLiquidPipe(cell, false, true, out pipe);
        }

        private bool TryFindAdjacentLiquidPipeWithWater(Vector2Int cell, out Vector2Int pipe)
        {
            return TryFindAdjacentLiquidPipe(cell, true, false, out pipe);
        }

        private bool TryFindAdjacentLiquidPipe(Vector2Int cell, bool requireWater, bool requireSpace, out Vector2Int pipe)
        {
            pipe = new Vector2Int(-1, -1);
            Vector2Int[] candidates =
            {
                cell,
                new Vector2Int(cell.x + 1, cell.y),
                new Vector2Int(cell.x - 1, cell.y),
                new Vector2Int(cell.x, cell.y + 1),
                new Vector2Int(cell.x, cell.y - 1)
            };

            float bestScore = requireWater ? -1f : float.MaxValue;
            for (int i = 0; i < candidates.Length; i++)
            {
                Vector2Int candidate = candidates[i];
                if (!IsInside(candidate.x, candidate.y) ||
                    !liquidPipe[candidate.x, candidate.y] ||
                    IsLiquidConduitBlocked(candidate.x, candidate.y))
                {
                    continue;
                }

                float amount = pipeWater[candidate.x, candidate.y];
                if (requireWater && amount <= 0.001f)
                {
                    continue;
                }

                if (requireSpace && amount >= LiquidPipeCapacity - 0.001f)
                {
                    continue;
                }

                if (requireWater)
                {
                    if (amount <= bestScore)
                    {
                        continue;
                    }

                    bestScore = amount;
                }
                else
                {
                    if (amount >= bestScore)
                    {
                        continue;
                    }

                    bestScore = amount;
                }

                pipe = candidate;
            }

            return pipe.x >= 0;
        }

        private bool TryFindAdjacentLiquidPipeWithSpaceExcluding(Vector2Int cell, Vector2Int excludedPipe, out Vector2Int pipe)
        {
            pipe = new Vector2Int(-1, -1);
            Vector2Int[] candidates =
            {
                cell,
                new Vector2Int(cell.x + 1, cell.y),
                new Vector2Int(cell.x - 1, cell.y),
                new Vector2Int(cell.x, cell.y + 1),
                new Vector2Int(cell.x, cell.y - 1)
            };

            float bestAmount = float.MaxValue;
            foreach (Vector2Int candidate in candidates)
            {
                if (candidate == excludedPipe ||
                    !IsInside(candidate.x, candidate.y) ||
                    !liquidPipe[candidate.x, candidate.y] ||
                    IsLiquidConduitBlocked(candidate.x, candidate.y) ||
                    pipeWater[candidate.x, candidate.y] >= LiquidPipeCapacity - 0.001f ||
                    pipeWater[candidate.x, candidate.y] >= bestAmount)
                {
                    continue;
                }

                bestAmount = pipeWater[candidate.x, candidate.y];
                pipe = candidate;
            }

            return pipe.x >= 0;
        }

        private bool TryFindLiquidVentOutput(Vector2Int vent, out Vector2Int output)
        {
            output = new Vector2Int(-1, -1);
            Vector2Int[] candidates =
            {
                new Vector2Int(vent.x, vent.y - 1),
                new Vector2Int(vent.x + 1, vent.y),
                new Vector2Int(vent.x - 1, vent.y),
                new Vector2Int(vent.x, vent.y + 1)
            };

            for (int i = 0; i < candidates.Length; i++)
            {
                Vector2Int candidate = candidates[i];
                if (!IsInside(candidate.x, candidate.y))
                {
                    continue;
                }

                if (CanLiquidOccupy(candidate.x, candidate.y) &&
                    LiquidFreeCapacity(candidate.x, candidate.y) > LiquidTileCapacity - 120f)
                {
                    output = candidate;
                    return true;
                }
            }

            return false;
        }

        private bool TryFindNaturalVentLiquidOutput(Vector2Int vent, out Vector2Int output)
        {
            output = new Vector2Int(-1, -1);
            Vector2Int[] candidates =
            {
                new Vector2Int(vent.x, vent.y - 1),
                new Vector2Int(vent.x + 1, vent.y),
                new Vector2Int(vent.x - 1, vent.y),
                new Vector2Int(vent.x, vent.y + 1)
            };

            for (int i = 0; i < candidates.Length; i++)
            {
                Vector2Int candidate = candidates[i];
                if (!IsInside(candidate.x, candidate.y))
                {
                    continue;
                }

                if (CanLiquidOccupy(candidate.x, candidate.y) &&
                    LiquidFreeCapacity(candidate.x, candidate.y) > LiquidTileCapacity - 120f)
                {
                    output = candidate;
                    return true;
                }
            }

            return false;
        }

        private bool HasGasPipeAccess(Vector2Int cell)
        {
            return IsGasPipeCell(cell.x, cell.y) ||
                IsGasPipeCell(cell.x + 1, cell.y) ||
                IsGasPipeCell(cell.x - 1, cell.y) ||
                IsGasPipeCell(cell.x, cell.y + 1) ||
                IsGasPipeCell(cell.x, cell.y - 1);
        }

        private bool IsGasPipeCell(int x, int y)
        {
            return IsInside(x, y) && gasPipe[x, y];
        }

        private bool TryFindAdjacentGasPipeWithSpace(Vector2Int cell, out Vector2Int pipe)
        {
            return TryFindAdjacentGasPipe(cell, false, true, out pipe);
        }

        private bool TryFindAdjacentGasPipeWithGas(Vector2Int cell, out Vector2Int pipe)
        {
            return TryFindAdjacentGasPipe(cell, true, false, out pipe);
        }

        private bool TryFindAdjacentGasPipeWithHydrogen(Vector2Int cell, out Vector2Int pipe)
        {
            pipe = new Vector2Int(-1, -1);
            Vector2Int[] candidates =
            {
                cell,
                new Vector2Int(cell.x + 1, cell.y),
                new Vector2Int(cell.x - 1, cell.y),
                new Vector2Int(cell.x, cell.y + 1),
                new Vector2Int(cell.x, cell.y - 1)
            };

            float best = 0.001f;
            foreach (Vector2Int candidate in candidates)
            {
                if (!IsInside(candidate.x, candidate.y) ||
                    !gasPipe[candidate.x, candidate.y] ||
                    IsGasConduitBlocked(candidate.x, candidate.y))
                {
                    continue;
                }

                float amount = gasPipeHydrogen[candidate.x, candidate.y];
                if (amount <= best)
                {
                    continue;
                }

                best = amount;
                pipe = candidate;
            }

            return pipe.x >= 0;
        }

        private bool TryFindAdjacentGasPipeWithNaturalGas(Vector2Int cell, out Vector2Int pipe)
        {
            pipe = new Vector2Int(-1, -1);
            Vector2Int[] candidates =
            {
                cell,
                new Vector2Int(cell.x + 1, cell.y),
                new Vector2Int(cell.x - 1, cell.y),
                new Vector2Int(cell.x, cell.y + 1),
                new Vector2Int(cell.x, cell.y - 1)
            };

            float best = 0.001f;
            foreach (Vector2Int candidate in candidates)
            {
                if (!IsInside(candidate.x, candidate.y) ||
                    !gasPipe[candidate.x, candidate.y] ||
                    IsGasConduitBlocked(candidate.x, candidate.y))
                {
                    continue;
                }

                float amount = gasPipeNaturalGas[candidate.x, candidate.y];
                if (amount <= best)
                {
                    continue;
                }

                best = amount;
                pipe = candidate;
            }

            return pipe.x >= 0;
        }

        private bool TryFindAdjacentMixedHydrogenPipe(Vector2Int cell, out Vector2Int pipe)
        {
            pipe = new Vector2Int(-1, -1);
            Vector2Int[] candidates =
            {
                cell,
                new Vector2Int(cell.x + 1, cell.y),
                new Vector2Int(cell.x - 1, cell.y),
                new Vector2Int(cell.x, cell.y + 1),
                new Vector2Int(cell.x, cell.y - 1)
            };

            float bestScore = 0.001f;
            foreach (Vector2Int candidate in candidates)
            {
                if (!IsInside(candidate.x, candidate.y) ||
                    !gasPipe[candidate.x, candidate.y] ||
                    IsGasConduitBlocked(candidate.x, candidate.y))
                {
                    continue;
                }

                float hydrogenMass = gasPipeHydrogen[candidate.x, candidate.y];
                float nonHydrogen = PipeNonHydrogenTotal(candidate.x, candidate.y);
                if (hydrogenMass <= 0.001f || nonHydrogen <= 0.001f)
                {
                    continue;
                }

                float score = nonHydrogen + hydrogenMass * 0.25f;
                if (score <= bestScore)
                {
                    continue;
                }

                bestScore = score;
                pipe = candidate;
            }

            return pipe.x >= 0;
        }

        private bool TryFindHydrogenFilterOutput(Vector2Int center, out Vector2Int output)
        {
            output = new Vector2Int(-1, -1);
            Vector2Int[] candidates =
            {
                center,
                new Vector2Int(center.x + 1, center.y),
                new Vector2Int(center.x - 1, center.y),
                new Vector2Int(center.x, center.y + 1),
                new Vector2Int(center.x, center.y - 1)
            };

            float bestPressure = float.MaxValue;
            foreach (Vector2Int candidate in candidates)
            {
                if (!IsInside(candidate.x, candidate.y) || !IsPassable(candidate.x, candidate.y))
                {
                    continue;
                }

                float pressure = TileGasTotal(candidate.x, candidate.y);
                if (pressure >= 2.78f || pressure >= bestPressure)
                {
                    continue;
                }

                bestPressure = pressure;
                output = candidate;
            }

            return output.x >= 0;
        }

        private bool TryFindAdjacentGasPipe(Vector2Int cell, bool requireGas, bool requireSpace, out Vector2Int pipe)
        {
            pipe = new Vector2Int(-1, -1);
            Vector2Int[] candidates =
            {
                cell,
                new Vector2Int(cell.x + 1, cell.y),
                new Vector2Int(cell.x - 1, cell.y),
                new Vector2Int(cell.x, cell.y + 1),
                new Vector2Int(cell.x, cell.y - 1)
            };

            float bestScore = requireGas ? -1f : float.MaxValue;
            for (int i = 0; i < candidates.Length; i++)
            {
                Vector2Int candidate = candidates[i];
                if (!IsInside(candidate.x, candidate.y) ||
                    !gasPipe[candidate.x, candidate.y] ||
                    IsGasConduitBlocked(candidate.x, candidate.y))
                {
                    continue;
                }

                float amount = GasPipeTotal(candidate.x, candidate.y);
                if (requireGas && amount <= 0.001f)
                {
                    continue;
                }

                if (requireSpace && amount >= GasPipeCapacity - 0.001f)
                {
                    continue;
                }

                if (requireGas)
                {
                    if (amount <= bestScore)
                    {
                        continue;
                    }

                    bestScore = amount;
                }
                else
                {
                    if (amount >= bestScore)
                    {
                        continue;
                    }

                    bestScore = amount;
                }

                pipe = candidate;
            }

            return pipe.x >= 0;
        }

        private bool TryFindAdjacentGasPipeWithSpaceExcluding(Vector2Int cell, Vector2Int excludedPipe, out Vector2Int pipe)
        {
            pipe = new Vector2Int(-1, -1);
            Vector2Int[] candidates =
            {
                cell,
                new Vector2Int(cell.x + 1, cell.y),
                new Vector2Int(cell.x - 1, cell.y),
                new Vector2Int(cell.x, cell.y + 1),
                new Vector2Int(cell.x, cell.y - 1)
            };

            float bestAmount = float.MaxValue;
            foreach (Vector2Int candidate in candidates)
            {
                if (candidate == excludedPipe ||
                    !IsInside(candidate.x, candidate.y) ||
                    !gasPipe[candidate.x, candidate.y] ||
                    IsGasConduitBlocked(candidate.x, candidate.y))
                {
                    continue;
                }

                float amount = GasPipeTotal(candidate.x, candidate.y);
                if (amount >= GasPipeCapacity - 0.001f || amount >= bestAmount)
                {
                    continue;
                }

                bestAmount = amount;
                pipe = candidate;
            }

            return pipe.x >= 0;
        }

        private bool TryFindGasSource(Vector2Int pump, out Vector2Int source)
        {
            source = new Vector2Int(-1, -1);
            Vector2Int[] candidates =
            {
                pump,
                new Vector2Int(pump.x + 1, pump.y),
                new Vector2Int(pump.x - 1, pump.y),
                new Vector2Int(pump.x, pump.y + 1),
                new Vector2Int(pump.x, pump.y - 1)
            };

            float bestGas = 0.05f;
            for (int i = 0; i < candidates.Length; i++)
            {
                Vector2Int candidate = candidates[i];
                if (!IsInside(candidate.x, candidate.y) || !IsPassable(candidate.x, candidate.y))
                {
                    continue;
                }

                float gas = TilePumpableGasTotal(candidate.x, candidate.y);
                if (gas <= bestGas)
                {
                    continue;
                }

                bestGas = gas;
                source = candidate;
            }

            return source.x >= 0;
        }

        private bool TryFindGasVentOutput(Vector2Int vent, out Vector2Int output)
        {
            output = new Vector2Int(-1, -1);
            Vector2Int[] candidates =
            {
                vent,
                new Vector2Int(vent.x + 1, vent.y),
                new Vector2Int(vent.x - 1, vent.y),
                new Vector2Int(vent.x, vent.y + 1),
                new Vector2Int(vent.x, vent.y - 1)
            };

            for (int i = 0; i < candidates.Length; i++)
            {
                Vector2Int candidate = candidates[i];
                if (!IsInside(candidate.x, candidate.y) || !IsPassable(candidate.x, candidate.y))
                {
                    continue;
                }

                if (TileGasTotal(candidate.x, candidate.y) < 2.4f)
                {
                    output = candidate;
                    return true;
                }
            }

            return false;
        }

        private bool TryFindNaturalVentGasOutput(Vector2Int vent, out Vector2Int output)
        {
            output = new Vector2Int(-1, -1);
            Vector2Int[] candidates =
            {
                new Vector2Int(vent.x, vent.y + 1),
                new Vector2Int(vent.x + 1, vent.y),
                new Vector2Int(vent.x - 1, vent.y),
                new Vector2Int(vent.x, vent.y - 1)
            };

            for (int i = 0; i < candidates.Length; i++)
            {
                Vector2Int candidate = candidates[i];
                if (!IsInside(candidate.x, candidate.y) || !IsPassable(candidate.x, candidate.y))
                {
                    continue;
                }

                if (TileGasTotal(candidate.x, candidate.y) < NaturalVentOutputPressure)
                {
                    output = candidate;
                    return true;
                }
            }

            return false;
        }

        private bool TryFindAdjacentWater(Vector2Int cell, out Vector2Int waterCell)
        {
            waterCell = new Vector2Int(-1, -1);
            Vector2Int[] offsets =
            {
                new Vector2Int(1, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, -1)
            };

            foreach (Vector2Int offset in offsets)
            {
                int x = cell.x + offset.x;
                int y = cell.y + offset.y;
                if (IsInside(x, y) && cells[x, y] == CellKind.Water && waterMass[x, y] > 0.5f)
                {
                    waterCell = new Vector2Int(x, y);
                    return true;
                }
            }

            return false;
        }

        private BuildSpec BuildSpecForMode(CommandMode mode)
        {
            switch (mode)
            {
                case CommandMode.Ladder:
                    return new BuildSpec(CellKind.Ladder, "Ladder", 0f, 2f, 0f, 1.1f);
                case CommandMode.Floor:
                    return new BuildSpec(CellKind.Floor, "Floor", 2f, 0f, 0f, 1.2f);
                case CommandMode.OxygenDiffuser:
                    return new BuildSpec(CellKind.OxygenDiffuser, "Oxygen Diffuser", 0f, 15f, 5f, 3.5f);
                case CommandMode.ManualGenerator:
                    return new BuildSpec(CellKind.ManualGenerator, "Manual Generator", 0f, 20f, 0f, 4f);
                case CommandMode.Battery:
                    return new BuildSpec(CellKind.Battery, "Battery", 0f, 20f, 0f, 3.5f);
                case CommandMode.SmartBattery:
                    return new BuildSpec(CellKind.SmartBattery, "Smart Battery", 0f, 35f, 0f, 4.4f);
                case CommandMode.PowerTransformer:
                    return new BuildSpec(CellKind.PowerTransformer, "Power Transformer", 4f, 28f, 0f, 3.8f, 4f);
                case CommandMode.CoalGenerator:
                    return new BuildSpec(CellKind.CoalGenerator, "Coal Generator", 0f, 45f, 0f, 5.2f);
                case CommandMode.HydrogenGenerator:
                    return new BuildSpec(CellKind.HydrogenGenerator, "Hydrogen Generator", 4f, 38f, 0f, 5f, 6f);
                case CommandMode.NaturalGasGenerator:
                    return new BuildSpec(CellKind.NaturalGasGenerator, "Natural Gas Generator", 4f, 42f, 0f, 5.2f, 6f);
                case CommandMode.SteamTurbine:
                    return new BuildSpec(CellKind.SteamTurbine, "Steam Turbine", 8f, 46f, 0f, 5.6f, 10f);
                case CommandMode.SolarPanel:
                    return new BuildSpec(CellKind.SolarPanel, "Solar Panel", 6f, 34f, 0f, 4.6f, 8f);
                case CommandMode.BunkerDoor:
                    return new BuildSpec(CellKind.BunkerDoor, "Bunker Door", 10f, 48f, 0f, 5.8f, 12f);
                case CommandMode.SpaceScanner:
                    return new BuildSpec(CellKind.SpaceScanner, "Space Scanner", 4f, 30f, 0f, 4.4f, 6f);
                case CommandMode.HydrogenFilter:
                    return new BuildSpec(CellKind.HydrogenFilter, "Hydrogen Filter", 3f, 26f, 0f, 3.4f);
                case CommandMode.RockCrusher:
                    return new BuildSpec(CellKind.RockCrusher, "Rock Crusher", 6f, 32f, 0f, 4.8f);
                case CommandMode.AtmoSuitDock:
                    return new BuildSpec(CellKind.AtmoSuitDock, "Atmo Suit Dock", 4f, 24f, 0f, 4.2f, 8f);
                case CommandMode.AtmoSuitCheckpoint:
                    return new BuildSpec(CellKind.AtmoSuitCheckpoint, "Atmo Suit Checkpoint", 2f, 16f, 0f, 2.8f, 4f);
                case CommandMode.InsulatedTile:
                    return new BuildSpec(CellKind.InsulatedTile, "Insulated Tile", 4f, 0f, 0f, 1.8f, 1f);
                case CommandMode.PrintingPod:
                    return new BuildSpec(CellKind.PrintingPod, "Printing Pod", 12f, 25f, 0f, 4.6f, 4f);
                case CommandMode.Bed:
                    return new BuildSpec(CellKind.Bed, "Bed", 8f, 0f, 0f, 2f);
                case CommandMode.Planter:
                    return new BuildSpec(CellKind.Planter, "Planter", 10f, 0f, 0f, 2.5f);
                case CommandMode.FarmStation:
                    return new BuildSpec(CellKind.FarmStation, "Farm Station", 12f, 14f, 0f, 2.8f);
                case CommandMode.WaterPump:
                    return new BuildSpec(CellKind.WaterPump, "Water Pump", 4f, 12f, 0f, 2.5f);
                case CommandMode.BottleEmptier:
                    return new BuildSpec(CellKind.BottleEmptier, "Bottle Emptier", 6f, 8f, 0f, 2.2f);
                case CommandMode.ResearchStation:
                    return new BuildSpec(CellKind.ResearchStation, "Research Station", 5f, 25f, 0f, 4f);
                case CommandMode.MicrobeMusher:
                    return new BuildSpec(CellKind.MicrobeMusher, "Microbe Musher", 10f, 18f, 0f, 3.5f);
                case CommandMode.AirDeodorizer:
                    return new BuildSpec(CellKind.AirDeodorizer, "Air Deodorizer", 4f, 16f, 0f, 2.8f);
                case CommandMode.MedicalCot:
                    return new BuildSpec(CellKind.MedicalCot, "Medical Cot", 8f, 18f, 0f, 3f);
                case CommandMode.SpaceHeater:
                    return new BuildSpec(CellKind.SpaceHeater, "Space Heater", 4f, 14f, 0f, 2.2f);
                case CommandMode.ThermoRegulator:
                    return new BuildSpec(CellKind.ThermoRegulator, "Thermo Regulator", 6f, 28f, 0f, 4f);
                case CommandMode.Outhouse:
                    return new BuildSpec(CellKind.Outhouse, "Outhouse", 18f, 6f, 0f, 2.6f);
                case CommandMode.WashBasin:
                    return new BuildSpec(CellKind.WashBasin, "Wash Basin", 6f, 12f, 0f, 2.2f);
                case CommandMode.Compost:
                    return new BuildSpec(CellKind.Compost, "Compost", 10f, 8f, 0f, 2.4f);
                case CommandMode.MassageTable:
                    return new BuildSpec(CellKind.MassageTable, "Massage Table", 12f, 10f, 0f, 2.8f);
                case CommandMode.ManualAirlock:
                    return new BuildSpec(CellKind.ManualAirlock, "Manual Airlock", 4f, 12f, 0f, 2.4f);
                case CommandMode.Refrigerator:
                    return new BuildSpec(CellKind.Refrigerator, "Refrigerator", 4f, 22f, 0f, 3.2f);
                case CommandMode.StorageBin:
                    return new BuildSpec(CellKind.StorageBin, "Storage Bin", 4f, 12f, 0f, 2.2f);
                case CommandMode.AutoSweeper:
                    return new BuildSpec(CellKind.AutoSweeper, "Auto-Sweeper", 2f, 24f, 0f, 3.6f, 6f);
                case CommandMode.ConveyorLoader:
                    return new BuildSpec(CellKind.ConveyorLoader, "Conveyor Loader", 2f, 26f, 0f, 3.4f, 4f);
                case CommandMode.ConveyorChute:
                    return new BuildSpec(CellKind.ConveyorChute, "Conveyor Chute", 2f, 18f, 0f, 2.5f, 2f);
                case CommandMode.SignalSwitch:
                    return new BuildSpec(CellKind.SignalSwitch, "Signal Switch", 1f, 12f, 0f, 1.8f, 1f);
                case CommandMode.LiquidPipeSensor:
                    return new BuildSpec(CellKind.LiquidPipeSensor, "Liquid Pipe Sensor", 0f, 18f, 0f, 2.4f, 2f);
                case CommandMode.LiquidShutoff:
                    return new BuildSpec(CellKind.LiquidShutoff, "Liquid Shutoff", 0f, 22f, 0f, 2.8f, 2f);
                case CommandMode.LiquidReservoir:
                    return new BuildSpec(CellKind.LiquidReservoir, "Liquid Reservoir", 6f, 20f, 0f, 3.2f);
                case CommandMode.LiquidVent:
                    return new BuildSpec(CellKind.LiquidVent, "Liquid Vent", 2f, 10f, 0f, 2f);
                case CommandMode.GasPump:
                    return new BuildSpec(CellKind.GasPump, "Gas Pump", 2f, 16f, 0f, 2.6f);
                case CommandMode.GasPipeSensor:
                    return new BuildSpec(CellKind.GasPipeSensor, "Gas Pipe Sensor", 0f, 18f, 0f, 2.4f, 2f);
                case CommandMode.GasShutoff:
                    return new BuildSpec(CellKind.GasShutoff, "Gas Shutoff", 0f, 22f, 0f, 2.8f, 2f);
                case CommandMode.GasReservoir:
                    return new BuildSpec(CellKind.GasReservoir, "Gas Reservoir", 4f, 22f, 0f, 3.4f);
                case CommandMode.GasVent:
                    return new BuildSpec(CellKind.GasVent, "Gas Vent", 1f, 8f, 0f, 1.8f);
                case CommandMode.RanchingStation:
                    return new BuildSpec(CellKind.RanchingStation, "Ranching Station", 10f, 16f, 0f, 2.8f);
                case CommandMode.Electrolyzer:
                    return new BuildSpec(CellKind.Electrolyzer, "Electrolyzer", 4f, 24f, 0f, 3.2f);
                case CommandMode.CarbonSkimmer:
                    return new BuildSpec(CellKind.CarbonSkimmer, "Carbon Skimmer", 6f, 22f, 0f, 3f);
                case CommandMode.WaterSieve:
                    return new BuildSpec(CellKind.WaterSieve, "Water Sieve", 6f, 18f, 0f, 2.8f);
                case CommandMode.MessTable:
                    return new BuildSpec(CellKind.MessTable, "Mess Table", 8f, 8f, 0f, 1.7f);
                case CommandMode.DecorPlant:
                    return new BuildSpec(CellKind.DecorPlant, "Decor Plant", 6f, 0f, 2f, 1.6f);
                default:
                    return new BuildSpec(CellKind.Empty, string.Empty, 0f, 0f, 0f, 0f);
            }
        }

        private BuildSpec BuildSpecForKind(CellKind kind)
        {
            switch (kind)
            {
                case CellKind.Ladder:
                    return BuildSpecForMode(CommandMode.Ladder);
                case CellKind.Floor:
                    return BuildSpecForMode(CommandMode.Floor);
                case CellKind.OxygenDiffuser:
                    return BuildSpecForMode(CommandMode.OxygenDiffuser);
                case CellKind.ManualGenerator:
                    return BuildSpecForMode(CommandMode.ManualGenerator);
                case CellKind.Battery:
                    return BuildSpecForMode(CommandMode.Battery);
                case CellKind.SmartBattery:
                    return BuildSpecForMode(CommandMode.SmartBattery);
                case CellKind.PowerTransformer:
                    return BuildSpecForMode(CommandMode.PowerTransformer);
                case CellKind.CoalGenerator:
                    return BuildSpecForMode(CommandMode.CoalGenerator);
                case CellKind.HydrogenGenerator:
                    return BuildSpecForMode(CommandMode.HydrogenGenerator);
                case CellKind.NaturalGasGenerator:
                    return BuildSpecForMode(CommandMode.NaturalGasGenerator);
                case CellKind.SteamTurbine:
                    return BuildSpecForMode(CommandMode.SteamTurbine);
                case CellKind.SolarPanel:
                    return BuildSpecForMode(CommandMode.SolarPanel);
                case CellKind.BunkerDoor:
                    return BuildSpecForMode(CommandMode.BunkerDoor);
                case CellKind.SpaceScanner:
                    return BuildSpecForMode(CommandMode.SpaceScanner);
                case CellKind.HydrogenFilter:
                    return BuildSpecForMode(CommandMode.HydrogenFilter);
                case CellKind.RockCrusher:
                    return BuildSpecForMode(CommandMode.RockCrusher);
                case CellKind.AtmoSuitDock:
                    return BuildSpecForMode(CommandMode.AtmoSuitDock);
                case CellKind.AtmoSuitCheckpoint:
                    return BuildSpecForMode(CommandMode.AtmoSuitCheckpoint);
                case CellKind.InsulatedTile:
                    return BuildSpecForMode(CommandMode.InsulatedTile);
                case CellKind.PrintingPod:
                    return BuildSpecForMode(CommandMode.PrintingPod);
                case CellKind.Bed:
                    return BuildSpecForMode(CommandMode.Bed);
                case CellKind.Planter:
                    return BuildSpecForMode(CommandMode.Planter);
                case CellKind.FarmStation:
                    return BuildSpecForMode(CommandMode.FarmStation);
                case CellKind.WaterPump:
                    return BuildSpecForMode(CommandMode.WaterPump);
                case CellKind.BottleEmptier:
                    return BuildSpecForMode(CommandMode.BottleEmptier);
                case CellKind.ResearchStation:
                    return BuildSpecForMode(CommandMode.ResearchStation);
                case CellKind.MicrobeMusher:
                    return BuildSpecForMode(CommandMode.MicrobeMusher);
                case CellKind.AirDeodorizer:
                    return BuildSpecForMode(CommandMode.AirDeodorizer);
                case CellKind.MedicalCot:
                    return BuildSpecForMode(CommandMode.MedicalCot);
                case CellKind.SpaceHeater:
                    return BuildSpecForMode(CommandMode.SpaceHeater);
                case CellKind.ThermoRegulator:
                    return BuildSpecForMode(CommandMode.ThermoRegulator);
                case CellKind.Outhouse:
                    return BuildSpecForMode(CommandMode.Outhouse);
                case CellKind.WashBasin:
                    return BuildSpecForMode(CommandMode.WashBasin);
                case CellKind.Compost:
                    return BuildSpecForMode(CommandMode.Compost);
                case CellKind.MassageTable:
                    return BuildSpecForMode(CommandMode.MassageTable);
                case CellKind.ManualAirlock:
                    return BuildSpecForMode(CommandMode.ManualAirlock);
                case CellKind.Refrigerator:
                    return BuildSpecForMode(CommandMode.Refrigerator);
                case CellKind.StorageBin:
                    return BuildSpecForMode(CommandMode.StorageBin);
                case CellKind.AutoSweeper:
                    return BuildSpecForMode(CommandMode.AutoSweeper);
                case CellKind.ConveyorLoader:
                    return BuildSpecForMode(CommandMode.ConveyorLoader);
                case CellKind.ConveyorChute:
                    return BuildSpecForMode(CommandMode.ConveyorChute);
                case CellKind.SignalSwitch:
                    return BuildSpecForMode(CommandMode.SignalSwitch);
                case CellKind.LiquidPipeSensor:
                    return BuildSpecForMode(CommandMode.LiquidPipeSensor);
                case CellKind.LiquidShutoff:
                    return BuildSpecForMode(CommandMode.LiquidShutoff);
                case CellKind.LiquidReservoir:
                    return BuildSpecForMode(CommandMode.LiquidReservoir);
                case CellKind.LiquidVent:
                    return BuildSpecForMode(CommandMode.LiquidVent);
                case CellKind.GasPump:
                    return BuildSpecForMode(CommandMode.GasPump);
                case CellKind.GasPipeSensor:
                    return BuildSpecForMode(CommandMode.GasPipeSensor);
                case CellKind.GasShutoff:
                    return BuildSpecForMode(CommandMode.GasShutoff);
                case CellKind.GasReservoir:
                    return BuildSpecForMode(CommandMode.GasReservoir);
                case CellKind.GasVent:
                    return BuildSpecForMode(CommandMode.GasVent);
                case CellKind.RanchingStation:
                    return BuildSpecForMode(CommandMode.RanchingStation);
                case CellKind.Electrolyzer:
                    return BuildSpecForMode(CommandMode.Electrolyzer);
                case CellKind.CarbonSkimmer:
                    return BuildSpecForMode(CommandMode.CarbonSkimmer);
                case CellKind.WaterSieve:
                    return BuildSpecForMode(CommandMode.WaterSieve);
                case CellKind.MessTable:
                    return BuildSpecForMode(CommandMode.MessTable);
                case CellKind.DecorPlant:
                    return BuildSpecForMode(CommandMode.DecorPlant);
                default:
                    return new BuildSpec(CellKind.Empty, string.Empty, 0f, 0f, 0f, 0f);
            }
        }

        private string ModeButtonLabel(CommandMode mode)
        {
            switch (mode)
            {
                case CommandMode.Inspect:
                    return "Inspect\n1";
                case CommandMode.Dig:
                    return "Dig\n2";
                case CommandMode.Ladder:
                    return "Ladder\n3  M2";
                case CommandMode.Floor:
                    return "Floor\n4  D2";
                case CommandMode.OxygenDiffuser:
                    return "Diffuser\n5  M15 A5";
                case CommandMode.ManualGenerator:
                    return "Generator\n6  M20";
                case CommandMode.Battery:
                    return "Battery\n7  M20";
                case CommandMode.SmartBattery:
                    return techPowerRegulation ? "Smart\nM35" : "Smart\nLOCK";
                case CommandMode.PowerTransformer:
                    return techPowerRegulation ? "Trans\nM28 R4" : "Trans\nLOCK";
                case CommandMode.CoalGenerator:
                    return techPowerRegulation ? "CoalGen\nM45" : "CoalGen\nLOCK";
                case CommandMode.HydrogenGenerator:
                    return techPowerRegulation ? "H2 Gen\nM38 R6" : "H2 Gen\nLOCK";
                case CommandMode.NaturalGasGenerator:
                    return techPowerRegulation ? "NG Gen\nM42 R6" : "NG Gen\nLOCK";
                case CommandMode.SteamTurbine:
                    return techPowerRegulation ? "Turbine\nM46 R10" : "Turbine\nLOCK";
                case CommandMode.SolarPanel:
                    return techPowerRegulation ? "Solar\nM34 R8" : "Solar\nLOCK";
                case CommandMode.BunkerDoor:
                    return techPowerRegulation ? "Bunker\nM48 R12" : "Bunker\nLOCK";
                case CommandMode.SpaceScanner:
                    return techPowerRegulation ? "Scanner\nM30 R6" : "Scanner\nLOCK";
                case CommandMode.HydrogenFilter:
                    return techAirSystems ? "H2 Filt\nM26" : "H2 Filt\nLOCK";
                case CommandMode.RockCrusher:
                    return techPowerRegulation ? "Crusher\nM32" : "Crusher\nLOCK";
                case CommandMode.AtmoSuitDock:
                    return techPowerRegulation ? "SuitDock\nM24 R8" : "SuitDock\nLOCK";
                case CommandMode.AtmoSuitCheckpoint:
                    return techPowerRegulation ? "SuitChk\nM16 R4" : "SuitChk\nLOCK";
                case CommandMode.InsulatedTile:
                    return techPowerRegulation ? "InsTile\nD4 R1" : "InsTile\nLOCK";
                case CommandMode.Deconstruct:
                    return "Decon\nD";
                case CommandMode.PrintingPod:
                    return techPowerRegulation ? "Pod\nD12 M25 R4" : "Pod\nLOCK";
                case CommandMode.Mop:
                    return "Mop\nS";
                case CommandMode.Repair:
                    return "Repair\nA";
                case CommandMode.Sweep:
                    return "Sweep\n.";
                case CommandMode.Bed:
                    return "Bed\n8  D8";
                case CommandMode.Planter:
                    return "Planter\n9  D10";
                case CommandMode.FarmStation:
                    return techFoodPreparation ? "Farm\nD12 M14" : "Farm\nLOCK";
                case CommandMode.WaterPump:
                    return "Pump\nP  M12";
                case CommandMode.BottleEmptier:
                    return "Bottle\nD6 M8";
                case CommandMode.ResearchStation:
                    return "Research\nR  M25";
                case CommandMode.MicrobeMusher:
                    return techFoodPreparation ? "Musher\nM  M18" : "Musher\nM LOCK";
                case CommandMode.AirDeodorizer:
                    return techAirSystems ? "Deodor\nO  M16" : "Deodor\nO LOCK";
                case CommandMode.MedicalCot:
                    return "Med Cot\nH  M18";
                case CommandMode.SpaceHeater:
                    return "Heater\nT  M14";
                case CommandMode.ThermoRegulator:
                    return techPowerRegulation ? "Cooler\nY  M28" : "Cooler\nY LOCK";
                case CommandMode.PowerWire:
                    return "Wire\nW  M1";
                case CommandMode.AutomationWire:
                    return techPowerRegulation ? "Auto\nM1" : "Auto\nLOCK";
                case CommandMode.Outhouse:
                    return "Toilet\nU  D18";
                case CommandMode.WashBasin:
                    return "Basin\nD6 M12";
                case CommandMode.Compost:
                    return "Compost\nD10 M8";
                case CommandMode.MassageTable:
                    return "Massage\nG  M10";
                case CommandMode.ManualAirlock:
                    return "Airlock\nQ  M12";
                case CommandMode.Refrigerator:
                    return techFoodPreparation ? "Fridge\nI  M22" : "Fridge\nI LOCK";
                case CommandMode.StorageBin:
                    return "Bin\nB  M12";
                case CommandMode.AutoSweeper:
                    return techPowerRegulation ? "SweepR\nM24 R6" : "SweepR\nLOCK";
                case CommandMode.ShippingRail:
                    return techPowerRegulation ? "Rail\nM1" : "Rail\nLOCK";
                case CommandMode.ConveyorLoader:
                    return techPowerRegulation ? "Loader\nM26 R4" : "Loader\nLOCK";
                case CommandMode.ConveyorChute:
                    return techPowerRegulation ? "Chute\nM18 R2" : "Chute\nLOCK";
                case CommandMode.SignalSwitch:
                    return techPowerRegulation ? "Signal\nM12 R1" : "Signal\nLOCK";
                case CommandMode.LiquidPipe:
                    return "Pipe\nL  M1";
                case CommandMode.LiquidPipeSensor:
                    return techPowerRegulation ? "L Sens\nM18 R2" : "L Sens\nLOCK";
                case CommandMode.LiquidShutoff:
                    return techPowerRegulation ? "L Shut\nM22 R2" : "L Shut\nLOCK";
                case CommandMode.LiquidReservoir:
                    return "L Res\nM20";
                case CommandMode.LiquidVent:
                    return "Vent\nV  M10";
                case CommandMode.GasPump:
                    return techAirSystems ? "GasP\nZ  M16" : "GasP\nZ LOCK";
                case CommandMode.GasPipe:
                    return techAirSystems ? "GasLn\nX  M1" : "GasLn\nX LOCK";
                case CommandMode.GasPipeSensor:
                    return techAirSystems && techPowerRegulation ? "G Sens\nM18 R2" : "G Sens\nLOCK";
                case CommandMode.GasShutoff:
                    return techAirSystems && techPowerRegulation ? "G Shut\nM22 R2" : "G Shut\nLOCK";
                case CommandMode.GasReservoir:
                    return techAirSystems ? "G Res\nM22" : "G Res\nLOCK";
                case CommandMode.GasVent:
                    return techAirSystems ? "GasV\nC  M8" : "GasV\nC LOCK";
                case CommandMode.RanchingStation:
                    return techFoodPreparation ? "Ranch\nM16" : "Ranch\nLOCK";
                case CommandMode.Electrolyzer:
                    return techAirSystems ? "Elect\nE  M24" : "Elect\nE LOCK";
                case CommandMode.CarbonSkimmer:
                    return techAirSystems ? "Skim\nK  M22" : "Skim\nK LOCK";
                case CommandMode.WaterSieve:
                    return techAirSystems ? "Sieve\nJ  M18" : "Sieve\nJ LOCK";
                case CommandMode.MessTable:
                    return "Mess\nN  D8";
                case CommandMode.DecorPlant:
                    return "Decor\nF  D6 A2";
                case CommandMode.Cancel:
                    return "Cancel\n0";
                default:
                    return mode.ToString();
            }
        }

        private string OverlayButtonLabel(OverlayMode mode)
        {
            switch (mode)
            {
                case OverlayMode.Gas:
                    return "Gas\nF1";
                case OverlayMode.Temperature:
                    return "Temp\nF2";
                case OverlayMode.Power:
                    return "Power\nF3";
                case OverlayMode.Germs:
                    return "Germs\nF4";
                case OverlayMode.Decor:
                    return "Decor\nView";
                case OverlayMode.Rooms:
                    return "Rooms\nView";
                case OverlayMode.Plumbing:
                    return "Pipe\nF11";
                case OverlayMode.Ventilation:
                    return "Vent\nF12";
                case OverlayMode.Logistics:
                    return "Ship\nView";
                default:
                    return mode.ToString();
            }
        }

        private string ModeName(CommandMode mode)
        {
            return mode == CommandMode.OxygenDiffuser ? "Oxygen Diffuser" :
                mode == CommandMode.ManualGenerator ? "Manual Generator" :
                mode == CommandMode.SmartBattery ? "Smart Battery" :
                mode == CommandMode.PowerTransformer ? "Power Transformer" :
                mode == CommandMode.CoalGenerator ? "Coal Generator" :
                mode == CommandMode.HydrogenGenerator ? "Hydrogen Generator" :
                mode == CommandMode.NaturalGasGenerator ? "Natural Gas Generator" :
                mode == CommandMode.SteamTurbine ? "Steam Turbine" :
                mode == CommandMode.SolarPanel ? "Solar Panel" :
                mode == CommandMode.BunkerDoor ? "Bunker Door" :
                mode == CommandMode.SpaceScanner ? "Space Scanner" :
                mode == CommandMode.HydrogenFilter ? "Hydrogen Filter" :
                mode == CommandMode.RockCrusher ? "Rock Crusher" :
                mode == CommandMode.AtmoSuitDock ? "Atmo Suit Dock" :
                mode == CommandMode.AtmoSuitCheckpoint ? "Atmo Suit Checkpoint" :
                mode == CommandMode.InsulatedTile ? "Insulated Tile" :
                mode == CommandMode.Deconstruct ? "Deconstruct" :
                mode == CommandMode.PrintingPod ? "Printing Pod" :
                mode == CommandMode.Mop ? "Mop" :
                mode == CommandMode.Repair ? "Repair" :
                mode == CommandMode.Sweep ? "Sweep" :
                mode == CommandMode.WaterPump ? "Water Pump" :
                mode == CommandMode.BottleEmptier ? "Bottle Emptier" :
                mode == CommandMode.FarmStation ? "Farm Station" :
                mode == CommandMode.ResearchStation ? "Research Station" :
                mode == CommandMode.MicrobeMusher ? "Microbe Musher" :
                mode == CommandMode.AirDeodorizer ? "Air Deodorizer" :
                mode == CommandMode.MedicalCot ? "Medical Cot" :
                mode == CommandMode.SpaceHeater ? "Space Heater" :
                mode == CommandMode.ThermoRegulator ? "Thermo Regulator" :
                mode == CommandMode.PowerWire ? "Power Wire" :
                mode == CommandMode.AutomationWire ? "Automation Wire" :
                mode == CommandMode.Outhouse ? "Outhouse" :
                mode == CommandMode.WashBasin ? "Wash Basin" :
                mode == CommandMode.Compost ? "Compost" :
                mode == CommandMode.MassageTable ? "Massage Table" :
                mode == CommandMode.ManualAirlock ? "Manual Airlock" :
                mode == CommandMode.Refrigerator ? "Refrigerator" :
                mode == CommandMode.StorageBin ? "Storage Bin" :
                mode == CommandMode.AutoSweeper ? "Auto-Sweeper" :
                mode == CommandMode.ShippingRail ? "Shipping Rail" :
                mode == CommandMode.ConveyorLoader ? "Conveyor Loader" :
                mode == CommandMode.ConveyorChute ? "Conveyor Chute" :
                mode == CommandMode.SignalSwitch ? "Signal Switch" :
                mode == CommandMode.LiquidPipe ? "Liquid Pipe" :
                mode == CommandMode.LiquidPipeSensor ? "Liquid Pipe Sensor" :
                mode == CommandMode.LiquidShutoff ? "Liquid Shutoff" :
                mode == CommandMode.LiquidReservoir ? "Liquid Reservoir" :
                mode == CommandMode.LiquidVent ? "Liquid Vent" :
                mode == CommandMode.GasPump ? "Gas Pump" :
                mode == CommandMode.GasPipe ? "Gas Pipe" :
                mode == CommandMode.GasPipeSensor ? "Gas Pipe Sensor" :
                mode == CommandMode.GasShutoff ? "Gas Shutoff" :
                mode == CommandMode.GasReservoir ? "Gas Reservoir" :
                mode == CommandMode.GasVent ? "Gas Vent" :
                mode == CommandMode.RanchingStation ? "Ranching Station" :
                mode == CommandMode.Electrolyzer ? "Electrolyzer" :
                mode == CommandMode.CarbonSkimmer ? "Carbon Skimmer" :
                mode == CommandMode.WaterSieve ? "Water Sieve" :
                mode == CommandMode.MessTable ? "Mess Table" :
                mode == CommandMode.DecorPlant ? "Decor Plant" :
                mode.ToString();
        }

        private string OverlayName(OverlayMode mode)
        {
            switch (mode)
            {
                case OverlayMode.Gas:
                    return "Gas";
                case OverlayMode.Temperature:
                    return "Temperature";
                case OverlayMode.Power:
                    return "Power";
                case OverlayMode.Germs:
                    return "Germs";
                case OverlayMode.Plumbing:
                    return "Plumbing";
                case OverlayMode.Ventilation:
                    return "Ventilation";
                case OverlayMode.Logistics:
                    return "Logistics";
                case OverlayMode.Decor:
                    return "Decor";
                case OverlayMode.Rooms:
                    return "Rooms";
                default:
                    return mode.ToString();
            }
        }

        private string JobLabel(Job job)
        {
            switch (job.Type)
            {
                case JobType.Dig:
                    return "Dig";
                case JobType.Build:
                    return "Build " + CellLabel(job.BuildKind);
                case JobType.BuildWire:
                    return "Build Power Wire";
                case JobType.BuildAutomationWire:
                    return "Build Automation Wire";
                case JobType.BuildPipe:
                    return "Build Liquid Pipe";
                case JobType.BuildGasPipe:
                    return "Build Gas Pipe";
                case JobType.BuildShippingRail:
                    return "Build Shipping Rail";
                case JobType.Deconstruct:
                    return "Deconstruct " + DeconstructTargetLabel(job);
                case JobType.Mop:
                    return "Mop Spill";
                case JobType.Repair:
                    return "Repair " + CellLabel(job.BuildKind);
                case JobType.Rescue:
                    return string.IsNullOrEmpty(job.TargetWorkerName) ? "Rescue" : "Rescue " + job.TargetWorkerName;
                case JobType.Sweep:
                    return HasLooseResource(job.Cell) ? "Sweep " + LooseResourceLabel(looseResourceKind[job.Cell.x, job.Cell.y]) : "Sweep";
                case JobType.OperateGenerator:
                    return "Operate Generator";
                case JobType.Harvest:
                    return "Harvest";
                case JobType.PumpWater:
                    return "Pump Water";
                case JobType.EmptyBottle:
                    return "Empty Bottle";
                case JobType.Research:
                    return "Research";
                case JobType.Cook:
                    return "Cook";
                case JobType.RefineMetal:
                    return "Refine Metal";
                case JobType.Sleep:
                    return "Sleep";
                case JobType.Treat:
                    return string.IsNullOrEmpty(job.TargetWorkerName) ? "Treatment" : "Treat " + job.TargetWorkerName;
                case JobType.UseToilet:
                    return string.IsNullOrEmpty(job.TargetWorkerName) ? "Use Outhouse" : job.TargetWorkerName + " to Outhouse";
                case JobType.WashHands:
                    return string.IsNullOrEmpty(job.TargetWorkerName) ? "Wash Hands" : job.TargetWorkerName + " to Wash Basin";
                case JobType.TendCrop:
                    return "Tend Crop";
                case JobType.Eat:
                    return string.IsNullOrEmpty(job.TargetWorkerName) ? "Eat" : "Feed " + job.TargetWorkerName;
                case JobType.Relax:
                    return string.IsNullOrEmpty(job.TargetWorkerName) ? "Relax" : "Relax " + job.TargetWorkerName;
                case JobType.Compost:
                    return "Compost Polluted Dirt";
                case JobType.GroomHatch:
                    return "Groom Hatch";
                default:
                    return job.Type.ToString();
            }
        }

        private string CellLabel(CellKind kind)
        {
            switch (kind)
            {
                case CellKind.Empty:
                    return "Empty";
                case CellKind.Dirt:
                    return "Dirt";
                case CellKind.Rock:
                    return "Rock";
                case CellKind.Sand:
                    return "Sand";
                case CellKind.MetalOre:
                    return "Metal Ore";
                case CellKind.Algae:
                    return "Algae";
                case CellKind.Coal:
                    return "Coal";
                case CellKind.Ice:
                    return "Ice";
                case CellKind.Ladder:
                    return "Ladder";
                case CellKind.Floor:
                    return "Floor";
                case CellKind.OxygenDiffuser:
                    return "Oxygen Diffuser";
                case CellKind.ManualGenerator:
                    return "Manual Generator";
                case CellKind.Battery:
                    return "Battery";
                case CellKind.SmartBattery:
                    return "Smart Battery";
                case CellKind.PowerTransformer:
                    return "Power Transformer";
                case CellKind.CoalGenerator:
                    return "Coal Generator";
                case CellKind.HydrogenGenerator:
                    return "Hydrogen Generator";
                case CellKind.NaturalGasGenerator:
                    return "Natural Gas Generator";
                case CellKind.SteamTurbine:
                    return "Steam Turbine";
                case CellKind.SolarPanel:
                    return "Solar Panel";
                case CellKind.Regolith:
                    return "Regolith";
                case CellKind.BunkerDoor:
                    return "Bunker Door";
                case CellKind.SpaceScanner:
                    return "Space Scanner";
                case CellKind.HydrogenFilter:
                    return "Hydrogen Filter";
                case CellKind.RockCrusher:
                    return "Rock Crusher";
                case CellKind.AtmoSuitDock:
                    return "Atmo Suit Dock";
                case CellKind.AtmoSuitCheckpoint:
                    return "Atmo Suit Checkpoint";
                case CellKind.InsulatedTile:
                    return "Insulated Tile";
                case CellKind.PrintingPod:
                    return "Printing Pod";
                case CellKind.Bed:
                    return "Bed";
                case CellKind.Planter:
                    return "Planter";
                case CellKind.FarmStation:
                    return "Farm Station";
                case CellKind.Water:
                    return "Water";
                case CellKind.WaterPump:
                    return "Water Pump";
                case CellKind.BottleEmptier:
                    return "Bottle Emptier";
                case CellKind.ResearchStation:
                    return "Research Station";
                case CellKind.MicrobeMusher:
                    return "Microbe Musher";
                case CellKind.AirDeodorizer:
                    return "Air Deodorizer";
                case CellKind.MedicalCot:
                    return "Medical Cot";
                case CellKind.SpaceHeater:
                    return "Space Heater";
                case CellKind.ThermoRegulator:
                    return "Thermo Regulator";
                case CellKind.Outhouse:
                    return "Outhouse";
                case CellKind.WashBasin:
                    return "Wash Basin";
                case CellKind.Compost:
                    return "Compost";
                case CellKind.MassageTable:
                    return "Massage Table";
                case CellKind.ManualAirlock:
                    return "Manual Airlock";
                case CellKind.Refrigerator:
                    return "Refrigerator";
                case CellKind.StorageBin:
                    return "Storage Bin";
                case CellKind.AutoSweeper:
                    return "Auto-Sweeper";
                case CellKind.ConveyorLoader:
                    return "Conveyor Loader";
                case CellKind.ConveyorChute:
                    return "Conveyor Chute";
                case CellKind.SignalSwitch:
                    return "Signal Switch";
                case CellKind.LiquidPipeSensor:
                    return "Liquid Pipe Sensor";
                case CellKind.LiquidShutoff:
                    return "Liquid Shutoff";
                case CellKind.LiquidReservoir:
                    return "Liquid Reservoir";
                case CellKind.LiquidVent:
                    return "Liquid Vent";
                case CellKind.GasPump:
                    return "Gas Pump";
                case CellKind.GasPipeSensor:
                    return "Gas Pipe Sensor";
                case CellKind.GasShutoff:
                    return "Gas Shutoff";
                case CellKind.SteamVent:
                    return "Steam Vent";
                case CellKind.HydrogenVent:
                    return "Hydrogen Vent";
                case CellKind.NaturalGasVent:
                    return "Natural Gas Vent";
                case CellKind.GasReservoir:
                    return "Gas Reservoir";
                case CellKind.GasVent:
                    return "Gas Vent";
                case CellKind.RanchingStation:
                    return "Ranching Station";
                case CellKind.Electrolyzer:
                    return "Electrolyzer";
                case CellKind.CarbonSkimmer:
                    return "Carbon Skimmer";
                case CellKind.WaterSieve:
                    return "Water Sieve";
                case CellKind.MessTable:
                    return "Mess Table";
                case CellKind.DecorPlant:
                    return "Decor Plant";
                default:
                    return kind.ToString();
            }
        }

        private string LooseResourceLabel(LooseResourceKind kind)
        {
            switch (kind)
            {
                case LooseResourceKind.Dirt:
                    return "dirt";
                case LooseResourceKind.Metal:
                    return "metal ore";
                case LooseResourceKind.Algae:
                    return "algae";
                case LooseResourceKind.Coal:
                    return "coal";
                case LooseResourceKind.RefinedMetal:
                    return "refined metal";
                case LooseResourceKind.PollutedDirt:
                    return "polluted dirt";
                default:
                    return "resource";
            }
        }

        private float DigWorkRequired(CellKind kind)
        {
            switch (kind)
            {
                case CellKind.Rock:
                    return 2.4f;
                case CellKind.MetalOre:
                    return 2f;
                case CellKind.Coal:
                    return 1.9f;
                case CellKind.Ice:
                    return 1.8f;
                case CellKind.Algae:
                    return 1.5f;
                case CellKind.Sand:
                case CellKind.Regolith:
                    return 0.9f;
                default:
                    return 1.2f;
            }
        }

        private Job FindAnyJobAt(Vector2Int cell)
        {
            foreach (Job job in jobs)
            {
                if (job.Cell == cell)
                {
                    return job;
                }
            }

            return null;
        }

        private Job FindJobAt(Vector2Int cell, JobType type)
        {
            foreach (Job job in jobs)
            {
                if (job.Cell == cell && job.Type == type)
                {
                    return job;
                }
            }

            return null;
        }

        private bool IsInside(int x, int y)
        {
            return x >= 0 && x < WorldWidth && y >= 0 && y < WorldHeight;
        }

        private bool IsPassable(int x, int y)
        {
            if (!IsInside(x, y))
            {
                return false;
            }

            CellKind kind = cells[x, y];
            if (kind == CellKind.ManualAirlock && !airlockOpen[x, y])
            {
                return false;
            }

            if (kind == CellKind.BunkerDoor && IsBunkerDoorClosed(new Vector2Int(x, y)))
            {
                return false;
            }

            return !IsSolidTile(kind) &&
                kind != CellKind.Water &&
                kind != CellKind.SteamVent &&
                kind != CellKind.HydrogenVent &&
                kind != CellKind.NaturalGasVent;
        }

        private float DefaultEquipmentCondition(CellKind kind)
        {
            return IsRepairableEquipment(kind) ? 1f : 0f;
        }

        private bool IsRepairableEquipment(CellKind kind)
        {
            return kind == CellKind.OxygenDiffuser ||
                kind == CellKind.ManualGenerator ||
                kind == CellKind.PowerTransformer ||
                kind == CellKind.WaterPump ||
                kind == CellKind.BottleEmptier ||
                kind == CellKind.ResearchStation ||
                kind == CellKind.MicrobeMusher ||
                kind == CellKind.AirDeodorizer ||
                kind == CellKind.SpaceHeater ||
                kind == CellKind.ThermoRegulator ||
                kind == CellKind.Refrigerator ||
                kind == CellKind.LiquidVent ||
                kind == CellKind.GasPump ||
                kind == CellKind.GasVent ||
                kind == CellKind.LiquidReservoir ||
                kind == CellKind.GasReservoir ||
                kind == CellKind.LiquidShutoff ||
                kind == CellKind.GasShutoff ||
                kind == CellKind.Electrolyzer ||
                kind == CellKind.CarbonSkimmer ||
                kind == CellKind.WaterSieve ||
                kind == CellKind.Compost ||
                kind == CellKind.WashBasin ||
                kind == CellKind.FarmStation ||
                kind == CellKind.AutoSweeper ||
                kind == CellKind.ConveyorLoader ||
                kind == CellKind.CoalGenerator ||
                kind == CellKind.HydrogenGenerator ||
                kind == CellKind.NaturalGasGenerator ||
                kind == CellKind.SteamTurbine ||
                kind == CellKind.SolarPanel ||
                kind == CellKind.BunkerDoor ||
                kind == CellKind.SpaceScanner ||
                kind == CellKind.HydrogenFilter ||
                kind == CellKind.RockCrusher ||
                kind == CellKind.AtmoSuitCheckpoint ||
                kind == CellKind.AtmoSuitDock;
        }

        private bool IsBrokenEquipment(Vector2Int cell)
        {
            return IsInside(cell.x, cell.y) &&
                IsRepairableEquipment(cells[cell.x, cell.y]) &&
                equipmentCondition[cell.x, cell.y] <= EquipmentBrokenThreshold;
        }

        private bool CanUseEquipment(Vector2Int cell)
        {
            return IsInside(cell.x, cell.y) && !IsBrokenEquipment(cell) && !IsEquipmentSubmerged(cell);
        }

        private void WearEquipment(Vector2Int cell, float amount)
        {
            if (amount <= 0f)
            {
                return;
            }

            DamageEquipment(cell, amount);
        }

        private void DamageEquipment(Vector2Int cell, float amount)
        {
            if (!IsInside(cell.x, cell.y) || !IsRepairableEquipment(cells[cell.x, cell.y]) || amount <= 0f)
            {
                return;
            }

            float before = Mathf.Clamp01(equipmentCondition[cell.x, cell.y]);
            if (before <= 0f)
            {
                return;
            }

            float temperaturePenalty = temperature[cell.x, cell.y] > 52f
                ? 1f + Mathf.Clamp01((temperature[cell.x, cell.y] - 52f) / 36f) * 2.5f
                : 1f;
            float after = Mathf.Clamp01(before - amount * temperaturePenalty);
            equipmentCondition[cell.x, cell.y] = after;
            if (before > EquipmentBrokenThreshold && after <= EquipmentBrokenThreshold)
            {
                equipmentFailures++;
                terrainDirty = true;
                overlayDirty = true;
                Log(CellLabel(cells[cell.x, cell.y]) + " broke down and needs repair.");
            }
            else if ((before > EquipmentAutoRepairThreshold && after <= EquipmentAutoRepairThreshold) || Mathf.Abs(before - after) > 0.02f)
            {
                terrainDirty = true;
                overlayDirty = true;
            }
        }

        private bool IsSolidTile(CellKind kind)
        {
            return IsNaturalSolid(kind) || kind == CellKind.InsulatedTile;
        }

        private bool IsNaturalSolid(CellKind kind)
        {
            return kind == CellKind.Dirt || kind == CellKind.Rock || kind == CellKind.Sand || kind == CellKind.Regolith || kind == CellKind.MetalOre || kind == CellKind.Algae || kind == CellKind.Coal || kind == CellKind.Slime || kind == CellKind.Ice;
        }

        private Vector3 CellCenter(Vector2Int cell)
        {
            return new Vector3(cell.x + 0.5f, cell.y + 0.5f, 0f);
        }

        private Vector2Int WorldToCell(Vector3 worldPosition)
        {
            return new Vector2Int(
                Mathf.Clamp(Mathf.FloorToInt(worldPosition.x), 0, WorldWidth - 1),
                Mathf.Clamp(Mathf.FloorToInt(worldPosition.y), 0, WorldHeight - 1));
        }

        private int Key(int x, int y)
        {
            return y * WorldWidth + x;
        }

        private void Log(string message)
        {
            lastLog = message;
        }

        private void DestroyRuntimeObject(UnityEngine.Object target)
        {
            if (target == null)
            {
                return;
            }

            if (Application.isPlaying)
            {
                Destroy(target);
            }
            else
            {
                DestroyImmediate(target);
            }
        }
    }
}
