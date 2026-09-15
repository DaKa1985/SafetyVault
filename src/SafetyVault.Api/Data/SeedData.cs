using SafetyVault.Api.Entities;

namespace SafetyVault.Api.Data
{
    public static class SeedData
    {
        public static List<Checklist> CreateChecklists()
        {
            //items erstellen, die dann auch in der datenbank gespeichert werden

            var wasser = new Item { Name = "Wasser", Description = "Wasser für 7 Tage" };
            var essen = new Item { Name = "Essen", Description = "Essen für 7 Tage" };
            var medikamente = new Item { Name = "Medikamente", Description = "Medikamente für 7 Tage" };
            var taschenlampe = new Item { Name = "Taschenlampe", Description = "Taschenlampe für den Notfall" };
            var batterien = new Item { Name = "Batterien", Description = "Batterien für die Taschenlampe" };
            var ersteHilfeSet = new Item { Name = "Erste-Hilfe-Set", Description = "Erste-Hilfe-Set für den Notfall" };
            
            return new List<Checklist>
            {
                new Checklist {
                    Name = "Notfallrucksack", 
                    Description = "Alles für den Rucksack" ,
                    ChecklistItems = new List<ChecklistItem>
                    {
                        new ChecklistItem { Item = taschenlampe, TargetQuantity = 2, ActualQuantity = 0},
                        new ChecklistItem { Item = batterien, TargetQuantity = 4, ActualQuantity = 0 },
                        new ChecklistItem { Item = ersteHilfeSet, TargetQuantity = 1, ActualQuantity = 0 }
                    }
                },
                new Checklist {
                    Name = "Verpflegungskoffer", 
                    Description = "Verpflegungskoffer für 7 Tage" ,
                    ChecklistItems = new List<ChecklistItem>
                    {
                        new ChecklistItem { Item = wasser, TargetQuantity = 14, ActualQuantity = 14 , PackedAt = new DateTime(2026, 9, 15, 0, 0, 0, DateTimeKind.Utc)},
                        new ChecklistItem { Item = essen, TargetQuantity = 21, ActualQuantity = 0 },
                        new ChecklistItem { Item = medikamente, TargetQuantity = 7, ActualQuantity = 0 }
                    }
                },
                new Checklist {
                    Name = "technisches Material",
                    Description = "Technisches Material für den Notfall",
                    ChecklistItems = new List<ChecklistItem>
                    {
                        new ChecklistItem { Item = taschenlampe, TargetQuantity = 2, ActualQuantity = 1 },
                        new ChecklistItem { Item = batterien, TargetQuantity = 4, ActualQuantity = 2 }
                    }
                }
            };
        }
    }
}
