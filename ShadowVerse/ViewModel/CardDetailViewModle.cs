using System.Collections.Generic;
using ShadowVerse.Model;
using ShadowVerse.Utils;
using Wrapper.Utils;

namespace ShadowVerse.ViewModel
{
    public class CardDetailViewModle
    {
        public CardDetailViewModle(int id)
        {
            var cardModel = CardUtils.GetCardModel(id);
            cardModel.Skill = cardModel.Skill.Replace("[u][ffcd45]", "【");
            cardModel.Skill = cardModel.Skill.Replace("[-][\\/u]", "】");
            var isMonster = CardUtils.IsMonster(id);
            CardDetailModel = new CardDetailModel
            {
                Name = cardModel.Name,
                Camp = cardModel.Camp,
                Type = cardModel.Type,
                Rarity = cardModel.Rarity,
                Pack = cardModel.Pack,
                Cv = cardModel.Cv,
                AtkAndLifeList =
                    isMonster
                        ? new List<string>
                        {
                            $"{cardModel.Atk} / {cardModel.Life}",
                            $"{cardModel.EvoAtk} / {cardModel.EvoLife}"
                        }
                        : new List<string> {"", ""},
                SkillList =
                    isMonster
                        ? JsonUtils.JsonDeserialize<List<string>>(cardModel.Skill)
                        : new List<string> {cardModel.Skill},
                ImagePathList = CardUtils.GetPicturePathList(cardModel.Id),
                EvoDescriptionList = isMonster ? new List<string> {"进化前", "进化后"} : new List<string> {"", ""}
            };
        }

        public CardDetailModel CardDetailModel { get; set; }
    }
}