using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Common;
using ShadowVerse.Model;
using ShadowVerse.Utils;
using Wrapper;
using Wrapper.Constant;
using Wrapper.Utils;
using DelegateCommand = ShadowVerse.Command.DelegateCommand;

namespace ShadowVerse.ViewModel
{
    public class CardDetailViewModle : BaseViewModle
    {
        public DelegateCommand CmdImageChange { get; set; }
        public CardDetailModel CardDetailModel { get; set; }
        public CardDetailViewModle()
        {
            CmdImageChange = new DelegateCommand {ExecuteCommand = Image_Changed};
        }

        public void UpdateCardDetailModel(int id)
        {
            var cardModel = CardUtils.GetCardModel(id);
            var isFollower = CardUtils.IsFollower(id);
            var type = Dic.TypeCodeDic.FirstOrDefault(dic => cardModel.TypeCode == dic.Key).Value;
            var camp = Dic.CampCodeDic.FirstOrDefault(dic => cardModel.CampCode == dic.Key).Value;
            var rarity = Dic.RarityCodeDic.FirstOrDefault(dic => cardModel.RarityCode == dic.Key).Value;
            var pack = Dic.PackCodeDic.FirstOrDefault(dic => cardModel.PackCode == dic.Key).Value;
            var imageCostPath = Dic.ImageCostDic.FirstOrDefault(dic => cardModel.Cost == dic.Key).Value;
            var imagePathList = CardUtils.GetPicturePathList(cardModel.Id);
            var imageCurrentPath = imagePathList[0];
            var atk = isFollower ? cardModel.Atk.ToString() : "";
            var evoAtk = isFollower ? cardModel.EvoAtk.ToString() : "";
            var life = isFollower ? cardModel.Life.ToString() : "";
            var evoLife = isFollower ? cardModel.EvoLife.ToString() : "";
            var imageAtkPath = isFollower ? PathManager.AtkPath : "";
            var imageLifePath = isFollower ? PathManager.LifePath : "";
            var skillList = isFollower
                ? JsonUtils.Deserialize<List<string>>(cardModel.SkillJson)
                : new List<string> {cardModel.SkillJson};
            var flavourList = isFollower
                ? JsonUtils.Deserialize<List<string>>(cardModel.FlavourJosn)
                : new List<string>() {cardModel.FlavourJosn};
            var evoDescriptionList = isFollower 
                ? new List<string> {"进化前", "进化后"} 
                : new List<string> {"", ""};
            CardDetailModel = new CardDetailModel
            {
                Id = id,
                Camp = camp,
                Type = type,
                Rarity = rarity,
                Pack = pack,
                Name = cardModel.Name,
                Cv = cardModel.Cv,
                Atk = atk,
                EvoAtk = evoAtk,
                Life = life,
                EvoLife = evoLife,
                SkillList = skillList,
                FlavourList = flavourList,
                ImagePathList = imagePathList,
                ImageCurrentPath = imageCurrentPath,
                EvoDescriptionList = evoDescriptionList,
                BgRarity = GetBgRarity(cardModel.RarityCode),
                ImageCostPath = imageCostPath,
                ImageAtkPath = imageAtkPath,
                ImageLifePath = imageLifePath,
            };
            OnPropertyChanged(nameof(CardDetailModel));
        }

        private static LinearGradientBrush GetBgRarity(int rarityCode)
        {
            var brush =
                new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(1, 1)
                };
            switch (rarityCode)
            {
                case StringConst.LegendaryCode:
                    brush.GradientStops.Add(new GradientStop(Colors.DarkViolet, 0.0));
                    brush.GradientStops.Add(new GradientStop(Colors.Red, 0.25));
                    brush.GradientStops.Add(new GradientStop(Colors.Green, 0.75));
                    brush.GradientStops.Add(new GradientStop(Colors.Yellow, 1.0));
                    break;
                case StringConst.GoldCode:
                    brush.GradientStops.Add(new GradientStop(Colors.Gold, 0.0));
                    break;
                case StringConst.SilverCode:
                    brush.GradientStops.Add(new GradientStop(Colors.Silver, 0.0));
                    break;
                case StringConst.BronzeCode:
                    brush.GradientStops.Add(new GradientStop(Colors.SaddleBrown, 0.0));
                    break;
            }
            return brush;
        }

        public void Image_Changed(object obj)
        {
            if (CardUtils.IsFollower(CardDetailModel.Id)) // 随从具有进化卡图
                CardDetailModel.ImageCurrentPath =
                    CardDetailModel.ImageCurrentPath.Equals(CardDetailModel.ImagePathList[0])
                        ? CardDetailModel.ImagePathList[1]
                        : CardDetailModel.ImagePathList[0];
            else // 非随从只具有
                CardDetailModel.ImageCurrentPath = CardDetailModel.ImagePathList[0];
            OnPropertyChanged(nameof(CardDetailModel));
        }
    }
}