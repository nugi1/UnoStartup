using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UnoAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Color = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WinnerName = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    RoundId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rounds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Played = table.Column<int>(type: "integer", nullable: false),
                    TopCardId = table.Column<Guid>(type: "uuid", nullable: true),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rounds_Cards_TopCardId",
                        column: x => x.TopCardId,
                        principalTable: "Cards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rounds_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    IsWinner = table.Column<bool>(type: "boolean", nullable: false),
                    IsTurn = table.Column<bool>(type: "boolean", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    IsOwner = table.Column<bool>(type: "boolean", nullable: false),
                    RoundId = table.Column<Guid>(type: "uuid", nullable: true),
                    ConnectionId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UnoSaid = table.Column<bool>(type: "boolean", nullable: false),
                    SayUnoUntil = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GameId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Players_Rounds_RoundId",
                        column: x => x.RoundId,
                        principalTable: "Rounds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoundCards",
                columns: table => new
                {
                    RoundId = table.Column<Guid>(type: "uuid", nullable: false),
                    CardId = table.Column<Guid>(type: "uuid", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoundCards", x => new { x.RoundId, x.CardId });
                    table.ForeignKey(
                        name: "FK_RoundCards_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoundCards_Rounds_RoundId",
                        column: x => x.RoundId,
                        principalTable: "Rounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Color", "Value" },
                values: new object[,]
                {
                    { new Guid("013e625e-b660-4b22-890b-15f059b58a50"), 2, 7 },
                    { new Guid("01fd8dfb-31f5-4f27-9f7b-6c1735f053a3"), 0, 4 },
                    { new Guid("057b4b43-6de1-4530-8243-9d0c574bf0b9"), 3, 0 },
                    { new Guid("07abf442-efb2-4937-b87a-cf1630d0c066"), 2, 11 },
                    { new Guid("1188be63-e44b-46b1-b2ff-178599a4c33c"), 3, 7 },
                    { new Guid("1d00f50e-48f2-493a-b3ac-9a17a0f53850"), 0, 7 },
                    { new Guid("1f1a8318-5db3-4935-9d1a-12c602b90851"), 0, 10 },
                    { new Guid("229cbf89-d623-49b3-b2cc-d6f8f24f1662"), 1, 7 },
                    { new Guid("24c64e6d-d2e0-4e5f-b604-b258b21f5879"), 0, 9 },
                    { new Guid("24d207f9-c7b2-4776-b5c8-54b07b0c06b3"), 2, 3 },
                    { new Guid("2a4c0c2f-6cd2-4a76-8978-ccce7e5f314f"), 3, 3 },
                    { new Guid("2c43267b-2044-487f-b459-92b6344a4d4e"), 1, 5 },
                    { new Guid("2d4b839b-d12c-474a-b2b3-e8b61ef5f500"), 1, 4 },
                    { new Guid("2e743cc5-06c2-4214-9027-cb97d52e2835"), 2, 12 },
                    { new Guid("370b6a8f-1c90-4da1-9ff1-30a8a933d2a7"), 1, 8 },
                    { new Guid("3ec6d3e8-f2b9-45be-bd16-6128b5c4bc73"), 2, 0 },
                    { new Guid("407879e2-64ad-4eae-bd83-93e8e277bfe4"), 4, 13 },
                    { new Guid("417f5f8b-7d61-47ab-bc81-5ca042e07fe4"), 2, 8 },
                    { new Guid("4302cf6b-9c81-40bc-98cf-b9670f66fd2f"), 2, 7 },
                    { new Guid("45d0bb02-e3ab-4005-8a51-274d4b116459"), 1, 6 },
                    { new Guid("46722ec9-b6a7-45a9-a03a-2d62664d9982"), 2, 10 },
                    { new Guid("4e450f78-c63e-4d6e-b625-c745eb1d18a0"), 3, 2 },
                    { new Guid("4f5e3f72-2f79-4882-bdf6-69c4f5b1c7e1"), 0, 6 },
                    { new Guid("4fa3b1f7-bdf9-4c70-b8b4-2b8b9947459e"), 2, 10 },
                    { new Guid("50178f22-2c6b-4c13-a0a2-b1bc5f8b8a13"), 2, 8 },
                    { new Guid("5073da44-f207-4a55-85b0-6397a9563db9"), 1, 12 },
                    { new Guid("520d685f-687a-4a11-bd96-083c8e524eb8"), 0, 8 },
                    { new Guid("5248b0fd-c72f-4e71-b51c-e3b20f0fa6c9"), 1, 10 },
                    { new Guid("58a9b906-5cc4-4c8a-8330-2c7b587f6c9f"), 0, 3 },
                    { new Guid("5a574cc9-cc1e-4085-a4de-fb0e9b387ff1"), 4, 14 },
                    { new Guid("5ae1f68f-2d73-4c66-9f1d-9c25c69c2996"), 0, 11 },
                    { new Guid("5b9a0548-8bb2-4d86-849b-3f18cdd60752"), 3, 5 },
                    { new Guid("60da4970-c48d-4375-9f3a-c5cb11ca46b0"), 0, 11 },
                    { new Guid("61c9ad71-59e5-4cba-9f90-3fa66a9894f2"), 0, 2 },
                    { new Guid("61eaf7fe-9680-4427-8663-4c21c0730ad7"), 2, 5 },
                    { new Guid("6a92f2d8-3d5e-4770-b94d-d430d36d9fbb"), 0, 3 },
                    { new Guid("6bb0905d-1b45-4780-9c6b-41f97b96a6f8"), 1, 2 },
                    { new Guid("6be88365-0ff7-4b69-94f2-5bc489519cc0"), 3, 5 },
                    { new Guid("6cc1c84c-e4da-45d3-960f-bb6437a2a87f"), 0, 12 },
                    { new Guid("6d153307-dc7e-45c8-b663-dfb25e37ed79"), 3, 12 },
                    { new Guid("6d7f1982-e5e9-4f3e-b710-05c85ff312f6"), 2, 6 },
                    { new Guid("6f2ed1f1-89b7-4024-83c7-d2a8eec29235"), 1, 1 },
                    { new Guid("6f68e622-bd59-4311-8e6f-c1c9a34252e9"), 4, 13 },
                    { new Guid("70c3d6d2-530d-4f16-b87d-9d46e8e3a5f0"), 0, 5 },
                    { new Guid("7240d34d-1a6c-44f1-a649-77a17bfffd6e"), 0, 2 },
                    { new Guid("742238ad-74e3-4ffb-81bc-90d0f2ebf30e"), 1, 9 },
                    { new Guid("742df238-bcf6-4330-bfa1-fd42b35d5b3a"), 3, 11 },
                    { new Guid("748e0b2a-4732-4c7d-b233-923e7bb556c0"), 1, 2 },
                    { new Guid("7b0e7785-7097-47db-b93d-84b56cf0fe38"), 2, 3 },
                    { new Guid("7bce94b2-eaa0-47e1-b498-cc0838815f53"), 4, 14 },
                    { new Guid("7d6b9ebf-d009-4bfa-b13e-5b79f4e6f8b3"), 3, 6 },
                    { new Guid("7e9f07b0-0147-4c44-9bc3-14d98f03bc60"), 1, 4 },
                    { new Guid("80b21b93-8ac9-42e6-86c9-e557bd40a7fe"), 3, 8 },
                    { new Guid("8b1d6a2a-d498-4607-9600-d5a6b5a57067"), 2, 9 },
                    { new Guid("8b990d8d-d345-44d0-8363-d3d457fc5971"), 3, 11 },
                    { new Guid("8c55d90a-3d19-47d2-8f9f-b1e1b710a1d1"), 3, 7 },
                    { new Guid("8fd4ef88-c35c-4c3a-bb6e-0c2f1ed51d1d"), 2, 2 },
                    { new Guid("91b219b9-8201-45b5-aeae-7c52d689aa9d"), 0, 9 },
                    { new Guid("94c4d748-246b-4207-a2a1-28261e6f8646"), 0, 5 },
                    { new Guid("94e385f3-8b07-456d-83b1-d938ec129e0b"), 3, 1 },
                    { new Guid("98b61cf7-e719-4743-8275-69b90fbd8031"), 2, 11 },
                    { new Guid("98c8a160-df1e-4b83-b5cc-e2a18c58533b"), 4, 14 },
                    { new Guid("9e382433-7f4a-4d27-bb9e-7774cc469801"), 0, 8 },
                    { new Guid("9e40f313-0939-4e92-9c95-e57dbb21f6fa"), 3, 1 },
                    { new Guid("a8f1c9f3-1fc3-4cfd-b3f1-bb595adf45db"), 0, 1 },
                    { new Guid("ab4a24c4-6792-4a1a-8545-290d6a02c879"), 3, 10 },
                    { new Guid("ac589428-3838-47d3-8d8d-e98a8f09c0ea"), 1, 10 },
                    { new Guid("ad4876fa-16d5-4a89-b9c9-b9535c8e28fe"), 4, 13 },
                    { new Guid("b017e439-946a-4377-bbe0-6fa396f69852"), 0, 1 },
                    { new Guid("b5d7994f-7f95-47d9-890f-1119a42e2348"), 2, 5 },
                    { new Guid("b5dcd58e-d433-468d-b32d-e4e30b799f5e"), 1, 1 },
                    { new Guid("b845af7a-b1d3-49a9-bca9-0a775edb6791"), 0, 4 },
                    { new Guid("b8e56d75-6027-4797-bdf9-7065db3f0e9d"), 0, 7 },
                    { new Guid("bb29b0c4-20f9-4b2d-a967-7239f8a99317"), 2, 4 },
                    { new Guid("bdb83f59-e5ca-46cc-9b88-03bcbd0647f5"), 3, 2 },
                    { new Guid("be2bcff3-49d9-4386-9e5e-e3e96e2d043a"), 2, 4 },
                    { new Guid("bf848f1e-df93-4ff5-98ff-2d720bf20c8b"), 1, 3 },
                    { new Guid("c2175d28-1ff5-4d61-b1a3-7a5941b02864"), 1, 7 },
                    { new Guid("c23b8108-9117-4e0b-bf1d-7587e312b0b9"), 1, 9 },
                    { new Guid("c63f7f06-2c25-48f9-bcbb-cd698dbba56b"), 3, 8 },
                    { new Guid("c8c8b813-6c63-4bb7-911b-6ef2325ab08d"), 4, 14 },
                    { new Guid("c911f070-bd02-45ff-97f7-cbc5c2708323"), 0, 10 },
                    { new Guid("cb5ef12b-cd8b-4d6f-8333-d6c8bbce2a79"), 3, 4 },
                    { new Guid("cc13e9fa-7f61-47fe-b2a5-fd740c44d0db"), 4, 13 },
                    { new Guid("d3922329-f00c-4931-b9c9-4ae3c6a9b0e6"), 3, 10 },
                    { new Guid("d4b9b57f-cd90-4a74-a0fd-c6d3c0b7fcd4"), 2, 2 },
                    { new Guid("d7c8a32a-96b1-4785-bb68-9877988e1eab"), 0, 0 },
                    { new Guid("de1bb81e-28c5-49f4-b5ba-75a1ab218c56"), 3, 4 },
                    { new Guid("df870b99-99a7-4e3a-97d0-bd1b7e3c574f"), 2, 12 },
                    { new Guid("dfd14a06-0808-4707-a365-02a6d2054531"), 3, 12 },
                    { new Guid("e01c9f68-c207-47bc-bdc7-b0a043e8b781"), 3, 9 },
                    { new Guid("e1f1d83d-0e29-4375-99e3-6b617ba3c64f"), 1, 6 },
                    { new Guid("e2a1b03e-460d-4f3e-b9c7-4bdf9e1d12b1"), 3, 9 },
                    { new Guid("e8ff8b8b-631d-44c4-8be6-9903fa7715fd"), 1, 12 },
                    { new Guid("ec44a06c-dca7-4f32-8b6d-25f50b0bbf8f"), 1, 0 },
                    { new Guid("f0d8fa3d-e4da-4f7f-bf30-b0a8e8aee76c"), 0, 6 },
                    { new Guid("f10e3c29-b212-4a84-b14d-80d4c9895760"), 1, 3 },
                    { new Guid("f2048a42-d61b-4710-8390-30967cbb43a5"), 0, 12 },
                    { new Guid("f372c4c0-fc1e-484e-b177-0d4ed99c6de5"), 3, 3 },
                    { new Guid("f3b0dbf1-4c4a-4d5f-b20c-e9e6602c2331"), 1, 11 },
                    { new Guid("f6c86964-b93d-4646-b6c1-13a417b1a259"), 3, 6 },
                    { new Guid("f8d24f56-1034-4878-bd6c-818f352fb205"), 2, 1 },
                    { new Guid("f9a45dff-4d50-4e25-b377-71e4f8d7e1e7"), 2, 1 },
                    { new Guid("fae64929-09f8-420d-bbdf-4b0a5ae19b71"), 1, 11 },
                    { new Guid("fb89002a-f1ff-417d-8ed0-bf2b8ea1b3a7"), 2, 9 },
                    { new Guid("fbd87e5e-b941-4cc5-a7b2-535a5750f800"), 1, 5 },
                    { new Guid("fc8c0c33-35ff-48c4-84c5-d8429d8db2a6"), 2, 6 },
                    { new Guid("fd695ed4-380e-4c26-9500-6d93d7b82e9c"), 1, 8 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_GameId",
                table: "Players",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_RoundId",
                table: "Players",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_RoundCards_CardId",
                table: "RoundCards",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_GameId",
                table: "Rounds",
                column: "GameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_TopCardId",
                table: "Rounds",
                column: "TopCardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "RoundCards");

            migrationBuilder.DropTable(
                name: "Rounds");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
