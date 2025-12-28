# Repository Usage Guide

## UpdateAsync - Single Entity Updates

```csharp
// RECOMMENDED: Load -> Modify -> Update (tracked entity)
var entity = await repository.GetByIdAsync(unitOfWork, id);
entity.Name = "Updated Name";
entity.Price = 99.99m;
await repository.UpdateAsync(unitOfWork, entity);
await unitOfWork.Commit();
```

**Behavior:** Only the root entity is marked as Modified. Child entity states are preserved.

```csharp
// CAUTION: Detached entity (e.g., from API DTO)
var entity = new Product
{
    Id = existingId,           // Must have valid ID
    Name = "Updated Name",     // Set ALL properties you want to keep
    Price = 99.99m,
    CategoryId = 1             // Unset properties become null/default!
};
await repository.UpdateAsync(unitOfWork, entity);
```

**Warning:** Detached entities update ALL properties. Unset properties will be overwritten with null/default values.

---

## BulkUpdateAsync - Multiple Entity Updates

```csharp
// RECOMMENDED: Load -> Modify -> Bulk Update
var entities = await repository.GetListAsync(unitOfWork, x => x.CategoryId == 5);
foreach (var entity in entities)
{
    entity.IsActive = false;
}
await repository.BulkUpdateAsync(unitOfWork, entities);
await unitOfWork.Commit();
```

**Behavior:** Marks the entire entity graph as Modified (including child entities).

---

## SoftDeleteAsync / BulkSoftDeleteAsync

```csharp
// Requires EntityBaseAudit<TKey> - throws if not
var entity = await repository.GetByIdAsync(unitOfWork, id);
await repository.SoftDeleteAsync(unitOfWork, entity);  // Sets StatusType = Deleted
await unitOfWork.Commit();
```

**Note:** Throws `InvalidOperationException` if entity doesn't inherit from `EntityBaseAudit<TKey>`.

---

## Concurrency Handling

Concurrency exceptions are thrown at `Commit()` time:

```csharp
try
{
    await unitOfWork.Commit();
}
catch (DbUpdateConcurrencyException ex)
{
    // Handle conflict - reload entity, merge changes, or notify user
}
```

---

## Quick Reference

| Method | Use Case | Child Entities |
|--------|----------|----------------|
| `UpdateAsync` | Single tracked entity | Preserved |
| `BulkUpdateAsync` | Multiple entities | Marked Modified |
| `DeleteAsync` | Hard delete | N/A |
| `SoftDeleteAsync` | Soft delete (audit) | Marked Modified |
