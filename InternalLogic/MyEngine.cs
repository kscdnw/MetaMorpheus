﻿using Proteomics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace InternalLogicEngineLayer
{
    public abstract class MyEngine
    {
        #region Public Fields

        public static UsefulProteomicsDatabases.Generated.unimod unimodDeserialized;

        public static Dictionary<int, ChemicalFormulaModification> uniprotDeseralized;

        #endregion Public Fields

        #region Internal Fields

        internal readonly int Level;

        #endregion Internal Fields

        #region Protected Constructors

        protected MyEngine(int Level)
        {
            this.Level = Level;
        }

        #endregion Protected Constructors

        #region Public Events

        public static event EventHandler<SingleEngineEventArgs> startingSingleEngineHander;

        public static event EventHandler<SingleEngineFinishedEventArgs> finishedSingleEngineHandler;

        public static event EventHandler<string> outLabelStatusHandler;

        public static event EventHandler<ProgressEventArgs> outProgressHandler;

        #endregion Public Events

        #region Public Methods

        public MyResults Run()
        {
            startingSingleEngine();
            ValidateParams();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var myResults = RunSpecific();
            stopWatch.Stop();
            myResults.Time = stopWatch.Elapsed;
            finishedSingleEngine(myResults);
            return myResults;
        }

        protected abstract void ValidateParams();

        #endregion Public Methods

        #region Protected Methods

        protected void status(string v)
        {
            outLabelStatusHandler?.Invoke(this, v);
        }

        protected void ReportProgress(ProgressEventArgs v)
        {
            outProgressHandler?.Invoke(this, v);
        }

        protected abstract MyResults RunSpecific();

        #endregion Protected Methods

        #region Private Methods

        private void startingSingleEngine()
        {
            startingSingleEngineHander?.Invoke(this, new SingleEngineEventArgs(this));
        }

        private void finishedSingleEngine(MyResults myResults)
        {
            finishedSingleEngineHandler?.Invoke(this, new SingleEngineFinishedEventArgs(myResults));
        }

        #endregion Private Methods
    }
}