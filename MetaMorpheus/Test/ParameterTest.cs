﻿using EngineLayer;
using MassSpectrometry;
using MzLibUtil;
using Nett;
using NUnit.Framework;
using Proteomics.Fragmentation;
using Proteomics.ProteolyticDigestion;
using System;
using System.Collections.Generic;
using System.IO;
using TaskLayer;

namespace Test
{
    [TestFixture]
    public static class ParameterTest
    {
        [Test]
        public static void TestFileSpecificParametersClone()
        {
            var fileSpecificToml = Toml.ReadFile(Path.Combine(TestContext.CurrentContext.TestDirectory, "testFileSpecfic.toml"), MetaMorpheusTask.tomlConfig);
            FileSpecificParameters fsp = new FileSpecificParameters(fileSpecificToml);
            FileSpecificParameters fspClone = fsp.Clone();
            Assert.AreEqual(fsp.DissociationType, fspClone.DissociationType);
            Assert.AreEqual(fsp.MaxMissedCleavages, fspClone.MaxMissedCleavages);
            Assert.AreEqual(fsp.MaxModsForPeptide, fspClone.MaxModsForPeptide);
            Assert.AreEqual(fsp.MaxPeptideLength, fspClone.MaxPeptideLength);
            Assert.AreEqual(fsp.MinPeptideLength, fspClone.MinPeptideLength);
            Assert.AreEqual(fsp.PrecursorMassTolerance, fspClone.PrecursorMassTolerance);
            Assert.AreEqual(fsp.ProductMassTolerance, fspClone.ProductMassTolerance);
            Assert.AreEqual(fsp.Protease, fspClone.Protease);
            Assert.AreEqual(fsp.SeparationType, fspClone.SeparationType);
            CollectionAssert.AreEqual(fsp.CustomIons, fspClone.CustomIons);
        }

        [Test]
        //This test exists because file specific parameters code has a tendency to overwrite common parameters and make life horrible 
        public static void TestFileSpecifcParameterOverwrite()
        {
            CommonParameters defaultParameters = new CommonParameters();

            DissociationTypeCollection.ProductsFromDissociationType[DissociationType.Custom] = 
                new List<ProductType> { ProductType.b, ProductType.y, ProductType.c };
            CommonParameters notDefaultParameters = new CommonParameters(
                dissociationType: DissociationType.ETD,
                doPrecursorDeconvolution: false,
                useProvidedPrecursorInfo: false,
                deconvolutionIntensityRatio: 69,
                deconvolutionMaxAssumedChargeState: 69,
                reportAllAmbiguity: false,
                addCompIons: true,
                totalPartitions: 69,
                scoreCutoff: 69,
                numberOfPeaksToKeepPerWindow: 69,
                minimumAllowedIntensityRatioToBasePeak: 69,
                trimMs1Peaks: true,
                trimMsMsPeaks: false,
                useDeltaScore: true,
                productMassTolerance: new AbsoluteTolerance(69),
                precursorMassTolerance: new AbsoluteTolerance(69),
                deconvolutionMassTolerance: new AbsoluteTolerance(69),
                maxThreadsToUsePerFile: 69,
                digestionParams: new DigestionParams(
                    protease: "Asp-N",
                    maxMissedCleavages: 69,
                    minPeptideLength: 1,
                    maxPeptideLength: 69,
                    maxModificationIsoforms: 69,
                    initiatorMethionineBehavior: InitiatorMethionineBehavior.Retain,
                    maxModsForPeptides: 69,
                    searchModeType: CleavageSpecificity.Semi,
                    fragmentationTerminus: FragmentationTerminus.C
                    ),
                listOfModsVariable: new List<(string, string)> { ("asdf", "asdf") },
                listOfModsFixed: new List<(string, string)> { ("asdf", "asdf") }
                );

            //check that the defaults are not the same as the not defaults.
            //IF ONE OF THESE FAILS, PLEASE UPDATE THE "notDefaultParameters"
            Assert.AreNotEqual(defaultParameters.DissociationType, notDefaultParameters.DissociationType);
            Assert.AreNotEqual(defaultParameters.DoPrecursorDeconvolution, notDefaultParameters.DoPrecursorDeconvolution);
            Assert.AreNotEqual(defaultParameters.UseProvidedPrecursorInfo, notDefaultParameters.UseProvidedPrecursorInfo);
            Assert.AreNotEqual(defaultParameters.DeconvolutionIntensityRatio, notDefaultParameters.DeconvolutionIntensityRatio);
            Assert.AreNotEqual(defaultParameters.DeconvolutionMaxAssumedChargeState, notDefaultParameters.DeconvolutionMaxAssumedChargeState);
            Assert.AreNotEqual(defaultParameters.ReportAllAmbiguity, notDefaultParameters.ReportAllAmbiguity);
            Assert.AreNotEqual(defaultParameters.AddCompIons, notDefaultParameters.AddCompIons);
            Assert.AreNotEqual(defaultParameters.TotalPartitions, notDefaultParameters.TotalPartitions);
            Assert.AreNotEqual(defaultParameters.ScoreCutoff, notDefaultParameters.ScoreCutoff);
            Assert.AreNotEqual(defaultParameters.NumberOfPeaksToKeepPerWindow, notDefaultParameters.NumberOfPeaksToKeepPerWindow);
            Assert.AreNotEqual(defaultParameters.MinimumAllowedIntensityRatioToBasePeak, notDefaultParameters.MinimumAllowedIntensityRatioToBasePeak);
            Assert.AreNotEqual(defaultParameters.TrimMs1Peaks, notDefaultParameters.TrimMs1Peaks);
            Assert.AreNotEqual(defaultParameters.TrimMsMsPeaks, notDefaultParameters.TrimMsMsPeaks);
            Assert.AreNotEqual(defaultParameters.UseDeltaScore, notDefaultParameters.UseDeltaScore);
            Assert.AreNotEqual(defaultParameters.ProductMassTolerance, notDefaultParameters.ProductMassTolerance);
            Assert.AreNotEqual(defaultParameters.PrecursorMassTolerance, notDefaultParameters.PrecursorMassTolerance);
            Assert.AreNotEqual(defaultParameters.DeconvolutionMassTolerance, notDefaultParameters.DeconvolutionMassTolerance);
            Assert.AreNotEqual(defaultParameters.MaxThreadsToUsePerFile, notDefaultParameters.MaxThreadsToUsePerFile);
            Assert.AreNotEqual(defaultParameters.DigestionParams, notDefaultParameters.DigestionParams);
            Assert.AreNotEqual(defaultParameters.ListOfModsVariable, notDefaultParameters.ListOfModsVariable);
            Assert.AreNotEqual(defaultParameters.ListOfModsFixed, notDefaultParameters.ListOfModsFixed);
            Assert.AreNotEqual(defaultParameters.CustomIons, notDefaultParameters.CustomIons);

            FileSpecificParameters emptyFileSpecificParameters = new FileSpecificParameters();
            CommonParameters updatedParameters = MetaMorpheusTask.SetAllFileSpecificCommonParams(notDefaultParameters, emptyFileSpecificParameters);

            //CHECK THAT NOTHING CHANGED
            Assert.AreEqual(updatedParameters.DissociationType, notDefaultParameters.DissociationType);
            Assert.AreEqual(updatedParameters.DoPrecursorDeconvolution, notDefaultParameters.DoPrecursorDeconvolution);
            Assert.AreEqual(updatedParameters.UseProvidedPrecursorInfo, notDefaultParameters.UseProvidedPrecursorInfo);
            Assert.AreEqual(updatedParameters.DeconvolutionIntensityRatio, notDefaultParameters.DeconvolutionIntensityRatio);
            Assert.AreEqual(updatedParameters.DeconvolutionMaxAssumedChargeState, notDefaultParameters.DeconvolutionMaxAssumedChargeState);
            Assert.AreEqual(updatedParameters.ReportAllAmbiguity, notDefaultParameters.ReportAllAmbiguity);
            Assert.AreEqual(updatedParameters.AddCompIons, notDefaultParameters.AddCompIons);
            Assert.AreEqual(updatedParameters.TotalPartitions, notDefaultParameters.TotalPartitions);
            Assert.AreEqual(updatedParameters.ScoreCutoff, notDefaultParameters.ScoreCutoff);
            Assert.AreEqual(updatedParameters.NumberOfPeaksToKeepPerWindow, notDefaultParameters.NumberOfPeaksToKeepPerWindow);
            Assert.AreEqual(updatedParameters.MinimumAllowedIntensityRatioToBasePeak, notDefaultParameters.MinimumAllowedIntensityRatioToBasePeak);
            Assert.AreEqual(updatedParameters.TrimMs1Peaks, notDefaultParameters.TrimMs1Peaks);
            Assert.AreEqual(updatedParameters.TrimMsMsPeaks, notDefaultParameters.TrimMsMsPeaks);
            Assert.AreEqual(updatedParameters.UseDeltaScore, notDefaultParameters.UseDeltaScore);
            Assert.AreEqual(updatedParameters.ProductMassTolerance, notDefaultParameters.ProductMassTolerance);
            Assert.AreEqual(updatedParameters.PrecursorMassTolerance, notDefaultParameters.PrecursorMassTolerance);
            Assert.AreEqual(updatedParameters.DeconvolutionMassTolerance, notDefaultParameters.DeconvolutionMassTolerance);
            Assert.AreEqual(updatedParameters.MaxThreadsToUsePerFile, notDefaultParameters.MaxThreadsToUsePerFile);
            Assert.AreEqual(updatedParameters.DigestionParams, notDefaultParameters.DigestionParams);
            Assert.AreEqual(updatedParameters.ListOfModsVariable, notDefaultParameters.ListOfModsVariable);
            Assert.AreEqual(updatedParameters.ListOfModsFixed, notDefaultParameters.ListOfModsFixed);
            Assert.AreEqual(updatedParameters.CustomIons, notDefaultParameters.CustomIons);

            FileSpecificParameters basicFileSpecificParameters = new FileSpecificParameters
            {
                PrecursorMassTolerance = new PpmTolerance(10),
                ProductMassTolerance = new PpmTolerance(30),
                Protease = new Protease("Arg-C", CleavageSpecificity.Full, null, null, new List<DigestionMotif> { new DigestionMotif("K", null, 1, "") }),
                MinPeptideLength = 1,
                MaxPeptideLength = 50,
                MaxMissedCleavages = 2,
                MaxModsForPeptide = 1,
                DissociationType = DissociationType.CID,
                CustomIons = new List<ProductType> { ProductType.b, ProductType.y }
            };
            updatedParameters = MetaMorpheusTask.SetAllFileSpecificCommonParams(notDefaultParameters, basicFileSpecificParameters);
            //CHECK THAT SOMETHINGS CHANGED AND OTHERS DIDN'T
            Assert.AreEqual(updatedParameters.DissociationType, basicFileSpecificParameters.DissociationType);
            Assert.AreEqual(updatedParameters.ProductMassTolerance, basicFileSpecificParameters.ProductMassTolerance);
            Assert.AreEqual(updatedParameters.PrecursorMassTolerance, basicFileSpecificParameters.PrecursorMassTolerance);
            Assert.AreEqual(updatedParameters.DigestionParams.MaxModsForPeptide, basicFileSpecificParameters.MaxModsForPeptide);
            Assert.AreEqual(updatedParameters.DigestionParams.MaxMissedCleavages, basicFileSpecificParameters.MaxMissedCleavages);
            Assert.AreEqual(updatedParameters.DigestionParams.MinPeptideLength, basicFileSpecificParameters.MinPeptideLength);
            Assert.AreEqual(updatedParameters.DigestionParams.MaxPeptideLength, basicFileSpecificParameters.MaxPeptideLength);
            Assert.AreEqual(updatedParameters.DigestionParams.Protease, basicFileSpecificParameters.Protease);
            Assert.AreEqual(updatedParameters.CustomIons, basicFileSpecificParameters.CustomIons);

            Assert.AreEqual(updatedParameters.DoPrecursorDeconvolution, notDefaultParameters.DoPrecursorDeconvolution);
            Assert.AreEqual(updatedParameters.UseProvidedPrecursorInfo, notDefaultParameters.UseProvidedPrecursorInfo);
            Assert.AreEqual(updatedParameters.DeconvolutionIntensityRatio, notDefaultParameters.DeconvolutionIntensityRatio);
            Assert.AreEqual(updatedParameters.DeconvolutionMaxAssumedChargeState, notDefaultParameters.DeconvolutionMaxAssumedChargeState);
            Assert.AreEqual(updatedParameters.ReportAllAmbiguity, notDefaultParameters.ReportAllAmbiguity);
            Assert.AreEqual(updatedParameters.AddCompIons, notDefaultParameters.AddCompIons);
            Assert.AreEqual(updatedParameters.TotalPartitions, notDefaultParameters.TotalPartitions);
            Assert.AreEqual(updatedParameters.ScoreCutoff, notDefaultParameters.ScoreCutoff);
            Assert.AreEqual(updatedParameters.NumberOfPeaksToKeepPerWindow, notDefaultParameters.NumberOfPeaksToKeepPerWindow);
            Assert.AreEqual(updatedParameters.MinimumAllowedIntensityRatioToBasePeak, notDefaultParameters.MinimumAllowedIntensityRatioToBasePeak);
            Assert.AreEqual(updatedParameters.TrimMs1Peaks, notDefaultParameters.TrimMs1Peaks);
            Assert.AreEqual(updatedParameters.TrimMsMsPeaks, notDefaultParameters.TrimMsMsPeaks);
            Assert.AreEqual(updatedParameters.UseDeltaScore, notDefaultParameters.UseDeltaScore);
            Assert.AreEqual(updatedParameters.DeconvolutionMassTolerance, notDefaultParameters.DeconvolutionMassTolerance);
            Assert.AreEqual(updatedParameters.MaxThreadsToUsePerFile, notDefaultParameters.MaxThreadsToUsePerFile);
            Assert.AreEqual(updatedParameters.DigestionParams.InitiatorMethionineBehavior, notDefaultParameters.DigestionParams.InitiatorMethionineBehavior);
            Assert.AreEqual(updatedParameters.ListOfModsVariable, notDefaultParameters.ListOfModsVariable);
            Assert.AreEqual(updatedParameters.ListOfModsFixed, notDefaultParameters.ListOfModsFixed);

        }
    }
}