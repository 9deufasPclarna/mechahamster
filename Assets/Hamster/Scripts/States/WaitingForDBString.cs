﻿// Copyright 2017 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Firebase.Unity.Editor;
namespace Hamster.States {
  // Utility state, for fetching strings.  Is basically
  // just a specialization of WaitingForDBLoad, but it needs
  // some minor changes to how the results are parsed, since
  // they're not technically valid json.
  class WaitingForDBString : WaitingForDBLoad<string> {
    public WaitingForDBString(string path) : base(path) { }
    protected override void HandleResult(object sender,
      Firebase.Database.ValueChangedEventArgs args) {
      // Remove the listener as soon as we get a response.
      database.GetReference(path).ValueChanged -= HandleResult;
      if (args.DatabaseError != null) {
        Debug.LogError("Database error :" + args.DatabaseError.Code + ":\n" +
          args.DatabaseError.Message + "\n" + args.DatabaseError.Details);
      } else {
        wasSuccessful = true;
        if (args.Snapshot != null) {
          string json = args.Snapshot.GetRawJsonValue();
          if (json.Length > 2) {
            result = json.Substring(1, json.Length - 2);
            wasSuccessful = true;
          }
        }
      }
      isComplete = true;
    }
  }
}