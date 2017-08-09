using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using ShadowVerse.Command;
using ShadowVerse.Model;
using ShadowVerse.Utils;
using Wrapper.Constant;
using Wrapper.Utils;

namespace ShadowVerse.ViewModel
{
    public class CardDetailViewModle : BaseViewModle
    {
        public CardDetailViewModle(int id)
        {
            CmdImageChange = new DelegateCommand {ExecuteCommand = Image_Changed};
            SetCardDetailModel(id);
        }

        public DelegateCommand CmdImageChange { get; set; }

        public CardDetailModel CardDetailModel { get; set; }

        private void SetCardDetailModel(int id)
        {
            var cardModel = CardUtils.GetCardModel(id);
            var isFollower = CardUtils.IsFollower(id);
            var type = Dictionary.TypeCodeDic.FirstOrDefault(dic => cardModel.TypeCode == dic.Key).Value;
            var camp = Dictionary.CampCodeDic.FirstOrDefault(dic => cardModel.CampCode == dic.Key).Value;
            var rarity = Dictionary.RarityCodeDic.FirstOrDefault(dic => cardModel.RarityCode == dic.Key).Value;
            var pack = Dictionary.PackCodeDic.FirstOrDefault(dic => cardModel.PackCode == dic.Key).Value;
            var imageCostPath = Dictionary.ImageCostDic.FirstOrDefault(dic => cardModel.Cost == dic.Key).Value;
            var imagePathList = CardUtils.GetPicturePathList(cardModel.Id);
            var imageCurrentPath = imagePathList[0];
            var atk = isFollower ? cardModel.Atk.ToString() : "";
            var evoAtk = isFollower ? cardModel.EvoAtk.ToString() : "";
            var life = isFollower ? cardModel.Life.ToString() : "";
            var evoLife = isFollower ? cardModel.EvoLife.ToString() : "";
            var skillList = isFollower
                ? JsonUtils.JsonDeserialize<List<string>>(cardModel.SkillJson)
                : new List<string> {cardModel.SkillJson};
            var flavourList = isFollower
                ? JsonUtils.JsonDeserialize<List<string>>(cardModel.FlavourJosn)
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
                ImageRarity = GetRarityColor(cardModel.RarityCode),
                ImageCostPath = imageCostPath
            };
            OnPropertyChanged(nameof(CardDetailModel));
        }

        private static SolidColorBrush GetRarityColor(int rarityCode)
        {
            var solidColorBrush = new SolidColorBrush();
            switch (rarityCode)
            {
                case StringConst.LegendaryCode:
                    solidColorBrush.Color = Colors.DarkViolet;
                    break;
                case StringConst.GoldCode:
                    solidColorBrush.Color = Colors.Gold;
                    break;
                case StringConst.SilverCode:
                    solidColorBrush.Color = Colors.Silver;
                    break;
                case StringConst.BronzeCode:
                    solidColorBrush.Color = Colors.SaddleBrown;
                    break;
            }
            return solidColorBrush;
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