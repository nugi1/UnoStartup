using Shared.Enums;
using UnoAccess.Entities;

namespace UnoAccess.PredefinedData;

public static class CardsData
{
    public static ICollection<Card> Get => new List<Card>()
    {
        new Card
        {
            Id = Guid.Parse("d7c8a32a-96b1-4785-bb68-9877988e1eab"), Color = CardColor.Red, Value = CardValue.Zero
        },
        new Card
        {
            Id = Guid.Parse("b017e439-946a-4377-bbe0-6fa396f69852"), Color = CardColor.Red, Value = CardValue.One
        },
        new Card
        {
            Id = Guid.Parse("a8f1c9f3-1fc3-4cfd-b3f1-bb595adf45db"), Color = CardColor.Red, Value = CardValue.One
        },
        new Card
        {
            Id = Guid.Parse("61c9ad71-59e5-4cba-9f90-3fa66a9894f2"), Color = CardColor.Red, Value = CardValue.Two
        },
        new Card
        {
            Id = Guid.Parse("7240d34d-1a6c-44f1-a649-77a17bfffd6e"), Color = CardColor.Red, Value = CardValue.Two
        },
        new Card
        {
            Id = Guid.Parse("58a9b906-5cc4-4c8a-8330-2c7b587f6c9f"), Color = CardColor.Red, Value = CardValue.Three
        },
        new Card
        {
            Id = Guid.Parse("6a92f2d8-3d5e-4770-b94d-d430d36d9fbb"), Color = CardColor.Red, Value = CardValue.Three
        },
        new Card
        {
            Id = Guid.Parse("b845af7a-b1d3-49a9-bca9-0a775edb6791"), Color = CardColor.Red, Value = CardValue.Four
        },
        new Card
        {
            Id = Guid.Parse("01fd8dfb-31f5-4f27-9f7b-6c1735f053a3"), Color = CardColor.Red, Value = CardValue.Four
        },
        new Card
        {
            Id = Guid.Parse("94c4d748-246b-4207-a2a1-28261e6f8646"), Color = CardColor.Red, Value = CardValue.Five
        },
        new Card
        {
            Id = Guid.Parse("70c3d6d2-530d-4f16-b87d-9d46e8e3a5f0"), Color = CardColor.Red, Value = CardValue.Five
        },
        new Card
        {
            Id = Guid.Parse("4f5e3f72-2f79-4882-bdf6-69c4f5b1c7e1"), Color = CardColor.Red, Value = CardValue.Six
        },
        new Card
        {
            Id = Guid.Parse("f0d8fa3d-e4da-4f7f-bf30-b0a8e8aee76c"), Color = CardColor.Red, Value = CardValue.Six
        },
        new Card
        {
            Id = Guid.Parse("b8e56d75-6027-4797-bdf9-7065db3f0e9d"), Color = CardColor.Red, Value = CardValue.Seven
        },
        new Card
        {
            Id = Guid.Parse("1d00f50e-48f2-493a-b3ac-9a17a0f53850"), Color = CardColor.Red, Value = CardValue.Seven
        },
        new Card
        {
            Id = Guid.Parse("9e382433-7f4a-4d27-bb9e-7774cc469801"), Color = CardColor.Red, Value = CardValue.Eight
        },
        new Card
        {
            Id = Guid.Parse("520d685f-687a-4a11-bd96-083c8e524eb8"), Color = CardColor.Red, Value = CardValue.Eight
        },
        new Card
        {
            Id = Guid.Parse("91b219b9-8201-45b5-aeae-7c52d689aa9d"), Color = CardColor.Red, Value = CardValue.Nine
        },
        new Card
        {
            Id = Guid.Parse("24c64e6d-d2e0-4e5f-b604-b258b21f5879"), Color = CardColor.Red, Value = CardValue.Nine
        },
        new Card
        {
            Id = Guid.Parse("1f1a8318-5db3-4935-9d1a-12c602b90851"), Color = CardColor.Red, Value = CardValue.DrawTwo
        },
        new Card
        {
            Id = Guid.Parse("c911f070-bd02-45ff-97f7-cbc5c2708323"), Color = CardColor.Red, Value = CardValue.DrawTwo
        },
        new Card
        {
            Id = Guid.Parse("60da4970-c48d-4375-9f3a-c5cb11ca46b0"), Color = CardColor.Red, Value = CardValue.Reverse
        },
        new Card
        {
            Id = Guid.Parse("5ae1f68f-2d73-4c66-9f1d-9c25c69c2996"), Color = CardColor.Red, Value = CardValue.Reverse
        },
        new Card
        {
            Id = Guid.Parse("6cc1c84c-e4da-45d3-960f-bb6437a2a87f"), Color = CardColor.Red, Value = CardValue.Skip
        },
        new Card
        {
            Id = Guid.Parse("f2048a42-d61b-4710-8390-30967cbb43a5"), Color = CardColor.Red, Value = CardValue.Skip
        },

        // Blue Cards
        new Card
        {
            Id = Guid.Parse("ec44a06c-dca7-4f32-8b6d-25f50b0bbf8f"), Color = CardColor.Blue, Value = CardValue.Zero
        },
        new Card
        {
            Id = Guid.Parse("b5dcd58e-d433-468d-b32d-e4e30b799f5e"), Color = CardColor.Blue, Value = CardValue.One
        },
        new Card
        {
            Id = Guid.Parse("6f2ed1f1-89b7-4024-83c7-d2a8eec29235"), Color = CardColor.Blue, Value = CardValue.One
        },
        new Card
        {
            Id = Guid.Parse("6bb0905d-1b45-4780-9c6b-41f97b96a6f8"), Color = CardColor.Blue, Value = CardValue.Two
        },
        new Card
        {
            Id = Guid.Parse("748e0b2a-4732-4c7d-b233-923e7bb556c0"), Color = CardColor.Blue, Value = CardValue.Two
        },
        new Card
        {
            Id = Guid.Parse("bf848f1e-df93-4ff5-98ff-2d720bf20c8b"), Color = CardColor.Blue, Value = CardValue.Three
        },
        new Card
        {
            Id = Guid.Parse("f10e3c29-b212-4a84-b14d-80d4c9895760"), Color = CardColor.Blue, Value = CardValue.Three
        },
        new Card
        {
            Id = Guid.Parse("7e9f07b0-0147-4c44-9bc3-14d98f03bc60"), Color = CardColor.Blue, Value = CardValue.Four
        },
        new Card
        {
            Id = Guid.Parse("2d4b839b-d12c-474a-b2b3-e8b61ef5f500"), Color = CardColor.Blue, Value = CardValue.Four
        },
        new Card
        {
            Id = Guid.Parse("2c43267b-2044-487f-b459-92b6344a4d4e"), Color = CardColor.Blue, Value = CardValue.Five
        },
        new Card
        {
            Id = Guid.Parse("fbd87e5e-b941-4cc5-a7b2-535a5750f800"), Color = CardColor.Blue, Value = CardValue.Five
        },
        new Card
        {
            Id = Guid.Parse("45d0bb02-e3ab-4005-8a51-274d4b116459"), Color = CardColor.Blue, Value = CardValue.Six
        },
        new Card
        {
            Id = Guid.Parse("e1f1d83d-0e29-4375-99e3-6b617ba3c64f"), Color = CardColor.Blue, Value = CardValue.Six
        },
        new Card
        {
            Id = Guid.Parse("c2175d28-1ff5-4d61-b1a3-7a5941b02864"), Color = CardColor.Blue, Value = CardValue.Seven
        },
        new Card
        {
            Id = Guid.Parse("229cbf89-d623-49b3-b2cc-d6f8f24f1662"), Color = CardColor.Blue, Value = CardValue.Seven
        },
        new Card
        {
            Id = Guid.Parse("370b6a8f-1c90-4da1-9ff1-30a8a933d2a7"), Color = CardColor.Blue, Value = CardValue.Eight
        },
        new Card
        {
            Id = Guid.Parse("fd695ed4-380e-4c26-9500-6d93d7b82e9c"), Color = CardColor.Blue, Value = CardValue.Eight
        },
        new Card
        {
            Id = Guid.Parse("c23b8108-9117-4e0b-bf1d-7587e312b0b9"), Color = CardColor.Blue, Value = CardValue.Nine
        },
        new Card
        {
            Id = Guid.Parse("742238ad-74e3-4ffb-81bc-90d0f2ebf30e"), Color = CardColor.Blue, Value = CardValue.Nine
        },
        new Card
        {
            Id = Guid.Parse("ac589428-3838-47d3-8d8d-e98a8f09c0ea"), Color = CardColor.Blue, Value = CardValue.DrawTwo
        },
        new Card
        {
            Id = Guid.Parse("5248b0fd-c72f-4e71-b51c-e3b20f0fa6c9"), Color = CardColor.Blue, Value = CardValue.DrawTwo
        },
        new Card
        {
            Id = Guid.Parse("f3b0dbf1-4c4a-4d5f-b20c-e9e6602c2331"), Color = CardColor.Blue, Value = CardValue.Reverse
        },
        new Card
        {
            Id = Guid.Parse("fae64929-09f8-420d-bbdf-4b0a5ae19b71"), Color = CardColor.Blue, Value = CardValue.Reverse
        },
        new Card
        {
            Id = Guid.Parse("5073da44-f207-4a55-85b0-6397a9563db9"), Color = CardColor.Blue, Value = CardValue.Skip
        },
        new Card
        {
            Id = Guid.Parse("e8ff8b8b-631d-44c4-8be6-9903fa7715fd"), Color = CardColor.Blue, Value = CardValue.Skip
        },

        // Green Cards
        new Card
        {
            Id = Guid.Parse("3ec6d3e8-f2b9-45be-bd16-6128b5c4bc73"), Color = CardColor.Green, Value = CardValue.Zero
        },
        new Card
        {
            Id = Guid.Parse("f9a45dff-4d50-4e25-b377-71e4f8d7e1e7"), Color = CardColor.Green, Value = CardValue.One
        },
        new Card
        {
            Id = Guid.Parse("f8d24f56-1034-4878-bd6c-818f352fb205"), Color = CardColor.Green, Value = CardValue.One
        },
        new Card
        {
            Id = Guid.Parse("8fd4ef88-c35c-4c3a-bb6e-0c2f1ed51d1d"), Color = CardColor.Green, Value = CardValue.Two
        },
        new Card
        {
            Id = Guid.Parse("d4b9b57f-cd90-4a74-a0fd-c6d3c0b7fcd4"), Color = CardColor.Green, Value = CardValue.Two
        },
        new Card
        {
            Id = Guid.Parse("7b0e7785-7097-47db-b93d-84b56cf0fe38"), Color = CardColor.Green, Value = CardValue.Three
        },
        new Card
        {
            Id = Guid.Parse("24d207f9-c7b2-4776-b5c8-54b07b0c06b3"), Color = CardColor.Green, Value = CardValue.Three
        },
        new Card
        {
            Id = Guid.Parse("bb29b0c4-20f9-4b2d-a967-7239f8a99317"), Color = CardColor.Green, Value = CardValue.Four
        },
        new Card
        {
            Id = Guid.Parse("be2bcff3-49d9-4386-9e5e-e3e96e2d043a"), Color = CardColor.Green, Value = CardValue.Four
        },
        new Card
        {
            Id = Guid.Parse("61eaf7fe-9680-4427-8663-4c21c0730ad7"), Color = CardColor.Green, Value = CardValue.Five
        },
        new Card
        {
            Id = Guid.Parse("b5d7994f-7f95-47d9-890f-1119a42e2348"), Color = CardColor.Green, Value = CardValue.Five
        },
        new Card
        {
            Id = Guid.Parse("fc8c0c33-35ff-48c4-84c5-d8429d8db2a6"), Color = CardColor.Green, Value = CardValue.Six
        },
        new Card
        {
            Id = Guid.Parse("6d7f1982-e5e9-4f3e-b710-05c85ff312f6"), Color = CardColor.Green, Value = CardValue.Six
        },
        new Card
        {
            Id = Guid.Parse("013e625e-b660-4b22-890b-15f059b58a50"), Color = CardColor.Green, Value = CardValue.Seven
        },
        new Card
        {
            Id = Guid.Parse("4302cf6b-9c81-40bc-98cf-b9670f66fd2f"), Color = CardColor.Green, Value = CardValue.Seven
        },
        new Card
        {
            Id = Guid.Parse("417f5f8b-7d61-47ab-bc81-5ca042e07fe4"), Color = CardColor.Green, Value = CardValue.Eight
        },
        new Card
        {
            Id = Guid.Parse("50178f22-2c6b-4c13-a0a2-b1bc5f8b8a13"), Color = CardColor.Green, Value = CardValue.Eight
        },
        new Card
        {
            Id = Guid.Parse("fb89002a-f1ff-417d-8ed0-bf2b8ea1b3a7"), Color = CardColor.Green, Value = CardValue.Nine
        },
        new Card
        {
            Id = Guid.Parse("8b1d6a2a-d498-4607-9600-d5a6b5a57067"), Color = CardColor.Green, Value = CardValue.Nine
        },
        new Card
        {
            Id = Guid.Parse("4fa3b1f7-bdf9-4c70-b8b4-2b8b9947459e"), Color = CardColor.Green, Value = CardValue.DrawTwo
        },
        new Card
        {
            Id = Guid.Parse("46722ec9-b6a7-45a9-a03a-2d62664d9982"), Color = CardColor.Green, Value = CardValue.DrawTwo
        },
        new Card
        {
            Id = Guid.Parse("07abf442-efb2-4937-b87a-cf1630d0c066"), Color = CardColor.Green, Value = CardValue.Reverse
        },
        new Card
        {
            Id = Guid.Parse("98b61cf7-e719-4743-8275-69b90fbd8031"), Color = CardColor.Green, Value = CardValue.Reverse
        },
        new Card
        {
            Id = Guid.Parse("2e743cc5-06c2-4214-9027-cb97d52e2835"), Color = CardColor.Green, Value = CardValue.Skip
        },
        new Card
        {
            Id = Guid.Parse("df870b99-99a7-4e3a-97d0-bd1b7e3c574f"), Color = CardColor.Green, Value = CardValue.Skip
        },

        // Yellow Cards
        new Card
        {
            Id = Guid.Parse("057b4b43-6de1-4530-8243-9d0c574bf0b9"), Color = CardColor.Yellow, Value = CardValue.Zero
        },
        new Card
        {
            Id = Guid.Parse("94e385f3-8b07-456d-83b1-d938ec129e0b"), Color = CardColor.Yellow, Value = CardValue.One
        },
        new Card
        {
            Id = Guid.Parse("9e40f313-0939-4e92-9c95-e57dbb21f6fa"), Color = CardColor.Yellow, Value = CardValue.One
        },
        new Card
        {
            Id = Guid.Parse("4e450f78-c63e-4d6e-b625-c745eb1d18a0"), Color = CardColor.Yellow, Value = CardValue.Two
        },
        new Card
        {
            Id = Guid.Parse("bdb83f59-e5ca-46cc-9b88-03bcbd0647f5"), Color = CardColor.Yellow, Value = CardValue.Two
        },
        new Card
        {
            Id = Guid.Parse("f372c4c0-fc1e-484e-b177-0d4ed99c6de5"), Color = CardColor.Yellow, Value = CardValue.Three
        },
        new Card
        {
            Id = Guid.Parse("2a4c0c2f-6cd2-4a76-8978-ccce7e5f314f"), Color = CardColor.Yellow, Value = CardValue.Three
        },
        new Card
        {
            Id = Guid.Parse("cb5ef12b-cd8b-4d6f-8333-d6c8bbce2a79"), Color = CardColor.Yellow, Value = CardValue.Four
        },
        new Card
        {
            Id = Guid.Parse("de1bb81e-28c5-49f4-b5ba-75a1ab218c56"), Color = CardColor.Yellow, Value = CardValue.Four
        },
        new Card
        {
            Id = Guid.Parse("5b9a0548-8bb2-4d86-849b-3f18cdd60752"), Color = CardColor.Yellow, Value = CardValue.Five
        },
        new Card
        {
            Id = Guid.Parse("6be88365-0ff7-4b69-94f2-5bc489519cc0"), Color = CardColor.Yellow, Value = CardValue.Five
        },
        new Card
        {
            Id = Guid.Parse("f6c86964-b93d-4646-b6c1-13a417b1a259"), Color = CardColor.Yellow, Value = CardValue.Six
        },
        new Card
        {
            Id = Guid.Parse("7d6b9ebf-d009-4bfa-b13e-5b79f4e6f8b3"), Color = CardColor.Yellow, Value = CardValue.Six
        },
        new Card
        {
            Id = Guid.Parse("8c55d90a-3d19-47d2-8f9f-b1e1b710a1d1"), Color = CardColor.Yellow, Value = CardValue.Seven
        },
        new Card
        {
            Id = Guid.Parse("1188be63-e44b-46b1-b2ff-178599a4c33c"), Color = CardColor.Yellow, Value = CardValue.Seven
        },
        new Card
        {
            Id = Guid.Parse("c63f7f06-2c25-48f9-bcbb-cd698dbba56b"), Color = CardColor.Yellow, Value = CardValue.Eight
        },
        new Card
        {
            Id = Guid.Parse("80b21b93-8ac9-42e6-86c9-e557bd40a7fe"), Color = CardColor.Yellow, Value = CardValue.Eight
        },
        new Card
        {
            Id = Guid.Parse("e01c9f68-c207-47bc-bdc7-b0a043e8b781"), Color = CardColor.Yellow, Value = CardValue.Nine
        },
        new Card
        {
            Id = Guid.Parse("e2a1b03e-460d-4f3e-b9c7-4bdf9e1d12b1"), Color = CardColor.Yellow, Value = CardValue.Nine
        },
        new Card
        {
            Id = Guid.Parse("d3922329-f00c-4931-b9c9-4ae3c6a9b0e6"), Color = CardColor.Yellow, Value = CardValue.DrawTwo
        },
        new Card
        {
            Id = Guid.Parse("ab4a24c4-6792-4a1a-8545-290d6a02c879"), Color = CardColor.Yellow, Value = CardValue.DrawTwo
        },
        new Card
        {
            Id = Guid.Parse("8b990d8d-d345-44d0-8363-d3d457fc5971"), Color = CardColor.Yellow, Value = CardValue.Reverse
        },
        new Card
        {
            Id = Guid.Parse("742df238-bcf6-4330-bfa1-fd42b35d5b3a"), Color = CardColor.Yellow, Value = CardValue.Reverse
        },
        new Card
        {
            Id = Guid.Parse("dfd14a06-0808-4707-a365-02a6d2054531"), Color = CardColor.Yellow, Value = CardValue.Skip
        },
        new Card
        {
            Id = Guid.Parse("6d153307-dc7e-45c8-b663-dfb25e37ed79"), Color = CardColor.Yellow, Value = CardValue.Skip
        },

        // Wild Cards
        new Card
        {
            Id = Guid.Parse("ad4876fa-16d5-4a89-b9c9-b9535c8e28fe"), Color = CardColor.Wild, Value = CardValue.Wild
        },
        new Card
        {
            Id = Guid.Parse("407879e2-64ad-4eae-bd83-93e8e277bfe4"), Color = CardColor.Wild, Value = CardValue.Wild
        },
        new Card
        {
            Id = Guid.Parse("cc13e9fa-7f61-47fe-b2a5-fd740c44d0db"), Color = CardColor.Wild, Value = CardValue.Wild
        },
        new Card
        {
            Id = Guid.Parse("6f68e622-bd59-4311-8e6f-c1c9a34252e9"), Color = CardColor.Wild, Value = CardValue.Wild
        },
        new Card
        {
            Id = Guid.Parse("5a574cc9-cc1e-4085-a4de-fb0e9b387ff1"), Color = CardColor.Wild,
            Value = CardValue.WildDrawFour
        },
        new Card
        {
            Id = Guid.Parse("98c8a160-df1e-4b83-b5cc-e2a18c58533b"), Color = CardColor.Wild,
            Value = CardValue.WildDrawFour
        },
        new Card
        {
            Id = Guid.Parse("7bce94b2-eaa0-47e1-b498-cc0838815f53"), Color = CardColor.Wild,
            Value = CardValue.WildDrawFour
        },
        new Card
        {
            Id = Guid.Parse("c8c8b813-6c63-4bb7-911b-6ef2325ab08d"), Color = CardColor.Wild,
            Value = CardValue.WildDrawFour
        }
    };
}