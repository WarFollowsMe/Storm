﻿/*
    Copyright 2016 TownEater

    Storm is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Storm is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Storm.  If not, see <http://www.gnu.org/licenses/>.
 */

using Storm.StardewValley.Wrapper;

namespace Storm.StardewValley.Event
{
    public class SetNewDialogueEvent : StaticContextEvent
    {
        public SetNewDialogueEvent(NPC npc, string dialogueSheetName, string dialogueSheetKey, int numberToAppend, bool add, bool clearOnMovement)
        {
            NPC = npc;
            DialogueSheetName = dialogueSheetName;
            DialogueSheetKey = dialogueSheetKey;
            NumberToAppend = numberToAppend;
            Add = add;
            ClearOnMovement = clearOnMovement;
        }
        public SetNewDialogueEvent(NPC npc, string s, bool add, bool clearOnMovement)
        {
            NPC = npc;
            Dialogue = s;
            Add = add;
            ClearOnMovement = clearOnMovement;
        }

        public NPC NPC { get; }
        public string Dialogue { get; }
        public string DialogueSheetName { get; }
        public string DialogueSheetKey { get; }
        public int NumberToAppend { get; }
        public bool Add { get; }
        public bool ClearOnMovement { get; }
    }
}
