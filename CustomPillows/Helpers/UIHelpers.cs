using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage.Components;
using HMUI;
using IPA.Utilities;
using UnityEngine;

namespace CustomPillows.Helpers
{
    public static class UIHelpers
    {
        private static readonly WaitForSeconds _delay = new WaitForSeconds(0.2f);

        public static void ReloadListSize(this CustomListTableData list)
        {
            SharedCoroutineStarter.instance.StartCoroutine(FixList(list));
        }

        private static IEnumerator FixList(CustomListTableData list)
        {
            yield return _delay;
            list.tableView.GetField<TableViewScroller, TableView>("scroller").RefreshScrollableSize();
        }
    }
}
