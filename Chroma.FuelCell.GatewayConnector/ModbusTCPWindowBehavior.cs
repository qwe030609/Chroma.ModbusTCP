using Chroma.UI.Wpf.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Chroma.FuelCell.GatewayConnector
{
    public class ModbusTCPWindowBehavior : BehaviorBase<ModbusTCPWindow>
    {
        #region Member

        private ModbusTCPWindow window;
        private DataGrid dataGrid_DO;
        //private DataGrid dataGrid_DI;
        private DataGrid dataGrid_AO;
        //private DataGrid dataGrid_AI;

        #endregion

        #region Setup and Cleanup

        protected override void OnSetup()
        {
            window = this.AssociatedObject;
            dataGrid_DO = this.AssociatedObject.CoilTagsDataGrid;
            //dataGrid_DI = this.AssociatedObject.DiscreteInputTagsDataGrid;
            dataGrid_AO = this.AssociatedObject.HoldingRegisterTagsDataGrid;
            //dataGrid_AI = this.AssociatedObject.InputRegisterTagsDataGrid;

            dataGrid_DO.PreviewMouseLeftButtonDown += DataGrid_DO_PreviewMouseLeftButtonDown;
            //dataGrid_DI.PreviewMouseLeftButtonDown += DataGrid_DI_PreviewMouseLeftButtonDown;
            dataGrid_AO.PreviewMouseLeftButtonDown += DataGrid_AO_PreviewMouseLeftButtonDown;
            //dataGrid_AI.PreviewMouseLeftButtonDown += DataGrid_AI_PreviewMouseLeftButtonDown;
        }

        protected override void OnCleanup()
        {
            dataGrid_DO.PreviewMouseLeftButtonDown -= DataGrid_DO_PreviewMouseLeftButtonDown;
            //dataGrid_DI.PreviewMouseLeftButtonDown -= DataGrid_DI_PreviewMouseLeftButtonDown;
            dataGrid_AO.PreviewMouseLeftButtonDown -= DataGrid_AO_PreviewMouseLeftButtonDown;
            //dataGrid_AI.PreviewMouseLeftButtonDown -= DataGrid_AI_PreviewMouseLeftButtonDown;
        }

        #endregion

        #region 事件

        private void DataGrid_DO_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var os = e?.OriginalSource as FrameworkElement;
            CheckBox CurrSelectedCkBx = os?.TemplatedParent as CheckBox;
            TagDataModel CurrSelectedCmd = CurrSelectedCkBx?.DataContext as TagDataModel;
            List<TagDataModel> dgSelectedItemList;

            if (CurrSelectedCkBx != null && CurrSelectedCmd != null) // ensure CheckBox was clicked
            {
                // CheckBox check All Selected Commands
                dgSelectedItemList = dataGrid_DO.SelectedItems.Cast<TagDataModel>().ToList();     // Cast Ilist to List

                if (dgSelectedItemList.Count != 0)
                {
                    if (dgSelectedItemList.Contains(CurrSelectedCmd))
                    {
                        if (!CurrSelectedCmd.IsChecked)
                        {
                            dgSelectedItemList.Select(ism => { ism.IsChecked = ism.Equals(CurrSelectedCmd) ? false : true; return ism; }).ToList();
                        }
                        else
                        {
                            dgSelectedItemList.Select(ism => { ism.IsChecked = ism.Equals(CurrSelectedCmd) ? true : false; return ism; }).ToList();
                        }
                    }
                }
            }
        }

        //private void DataGrid_DI_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    var os = e?.OriginalSource as FrameworkElement;
        //    CheckBox CurrSelectedCkBx = os?.TemplatedParent as CheckBox;
        //    TagDataModel CurrSelectedCmd = CurrSelectedCkBx?.DataContext as TagDataModel;
        //    List<TagDataModel> dgSelectedItemList;

        //    if (CurrSelectedCkBx != null && CurrSelectedCmd != null) // ensure CheckBox was clicked
        //    {
        //        // CheckBox check All Selected Commands
        //        dgSelectedItemList = dataGrid_DI.SelectedItems.Cast<TagDataModel>().ToList();     // Cast Ilist to List

        //        if (dgSelectedItemList.Count != 0)
        //        {
        //            if (dgSelectedItemList.Contains(CurrSelectedCmd))
        //            {
        //                if (!CurrSelectedCmd.IsChecked)
        //                {
        //                    dgSelectedItemList.Select(ism => { ism.IsChecked = ism.Equals(CurrSelectedCmd) ? false : true; return ism; }).ToList();
        //                }
        //                else
        //                {
        //                    dgSelectedItemList.Select(ism => { ism.IsChecked = ism.Equals(CurrSelectedCmd) ? true : false; return ism; }).ToList();
        //                }
        //            }
        //        }
        //    }
        //}

        private void DataGrid_AO_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var os = e?.OriginalSource as FrameworkElement;
            CheckBox CurrSelectedCkBx = os?.TemplatedParent as CheckBox;
            TagDataModel CurrSelectedCmd = CurrSelectedCkBx?.DataContext as TagDataModel;
            List<TagDataModel> dgSelectedItemList;

            if (CurrSelectedCkBx != null && CurrSelectedCmd != null) // ensure CheckBox was clicked
            {
                // CheckBox check All Selected Commands
                dgSelectedItemList = dataGrid_AO.SelectedItems.Cast<TagDataModel>().ToList();     // Cast Ilist to List

                if (dgSelectedItemList.Count != 0)
                {
                    if (dgSelectedItemList.Contains(CurrSelectedCmd))
                    {
                        if (!CurrSelectedCmd.IsChecked)
                        {
                            dgSelectedItemList.Select(ism => { ism.IsChecked = ism.Equals(CurrSelectedCmd) ? false : true; return ism; }).ToList();
                        }
                        else
                        {
                            dgSelectedItemList.Select(ism => { ism.IsChecked = ism.Equals(CurrSelectedCmd) ? true : false; return ism; }).ToList();
                        }
                    }
                }
            }
        }

        //private void DataGrid_AI_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    var os = e?.OriginalSource as FrameworkElement;
        //    CheckBox CurrSelectedCkBx = os?.TemplatedParent as CheckBox;
        //    TagDataModel CurrSelectedCmd = CurrSelectedCkBx?.DataContext as TagDataModel;
        //    List<TagDataModel> dgSelectedItemList;

        //    if (CurrSelectedCkBx != null && CurrSelectedCmd != null) // ensure CheckBox was clicked
        //    {
        //        // CheckBox check All Selected Commands
        //        dgSelectedItemList = dataGrid_AI.SelectedItems.Cast<TagDataModel>().ToList();     // Cast Ilist to List

        //        if (dgSelectedItemList.Count != 0)
        //        {
        //            if (dgSelectedItemList.Contains(CurrSelectedCmd))
        //            {
        //                if (!CurrSelectedCmd.IsChecked)
        //                {
        //                    dgSelectedItemList.Select(ism => { ism.IsChecked = ism.Equals(CurrSelectedCmd) ? false : true; return ism; }).ToList();
        //                }
        //                else
        //                {
        //                    dgSelectedItemList.Select(ism => { ism.IsChecked = ism.Equals(CurrSelectedCmd) ? true : false; return ism; }).ToList();
        //                }
        //            }
        //        }
        //    }
        //}


        #endregion

        #region Method

        #endregion
    }
}
