﻿using System.Globalization;
using System.Windows.Controls;
using Catrobat.IDE.Core.CatrobatObjects.Formulas.FormulaToken;

namespace Catrobat.IDE.Phone.Controls.FormulaControls.PartControls
{
    public class FormulaPartControlDecimalSeparator : FormulaPartControl
    {
        public string GetText()
        {
            return CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        }

        #region FormulaPartControl

        protected override Grid CreateControls(double fontSize, bool isParentSelected, bool isSelected, bool isError)
        {
            var grid = new Grid {DataContext = this};
            grid.Children.Add(new TextBlock
            {
                Text = GetText(),
                FontSize = fontSize
            });

            return grid;
        }

        public override int GetCharacterWidth()
        {
            return GetText().Length;
        }

        public override FormulaPartControl Copy()
        {
            return new FormulaPartControlNumber
            {
                Style = Style
            };
        }

        public override FormulaPartControl CreateUiTokenTemplate(IFormulaToken token)
        {
            return new FormulaPartControlDecimalSeparator
            {
                Style = Style, 
                Token = token
            };
        }

        #endregion
    }
}