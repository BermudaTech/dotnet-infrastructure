# Repository Usage Guide

Each repository method calls `SaveChangesAsync()` internally - changes are persisted immediately.

## Basic CRUD Operations

```csharp
// Insert
var entity = new Product { Name = "New Product" };
await repository.InsertAsync(unitOfWork, entity);  // Saved immediately

// Update (load -> modify -> save)
var entity = await repository.GetByIdAsync(unitOfWork, id);
entity.Name = "Updated Name";
await repository.UpdateAsync(unitOfWork, entity);  // Saved immediately

// Delete
var entity = await repository.GetByIdAsync(unitOfWork, id);
await repository.DeleteAsync(unitOfWork, entity);  // Saved immediately
```

No need to call `unitOfWork.Commit()` for simple operations.

---

## Grouping Multiple Operations (Optional)

Use `Begin()`/`Commit()` only when you need atomic transactions:

```csharp
unitOfWork.Begin();
try
{
    await repository.InsertAsync(unitOfWork, entity1);
    await repository.InsertAsync(unitOfWork, entity2);
    unitOfWork.Commit();  // Both succeed or both fail
}
catch
{
    // Transaction auto-rolls back on exception
    throw;
}
```

---

## SoftDeleteAsync / BulkSoftDeleteAsync

```csharp
// Requires EntityBaseAudit<TKey> - throws if not
var entity = await repository.GetByIdAsync(unitOfWork, id);
await repository.SoftDeleteAsync(unitOfWork, entity);  // Sets StatusType = Deleted
```

**Note:** Throws `InvalidOperationException` if entity doesn't inherit from `EntityBaseAudit<TKey>`.

---

## Concurrency Handling

If using concurrency tokens (e.g., PostgreSQL xmin), exceptions are thrown immediately:

```csharp
try
{
    await repository.UpdateAsync(unitOfWork, entity);
}
catch (DbUpdateConcurrencyException ex)
{
    // Handle conflict - reload entity, merge changes, or notify user
}
```

---

## Auto-Set Fields

- `UpdatedDate` is auto-set to `DateTime.UtcNow` if null (for `EntityBaseAudit` entities)
- Applies to: `UpdateAsync`, `BulkUpdateAsync`, `SoftDeleteAsync`, `BulkSoftDeleteAsync`

---

## Quick Reference

| Method | Saves Immediately | Notes |
|--------|-------------------|-------|
| `InsertAsync` | Yes | |
| `BulkInsertAsync` | Yes | |
| `UpdateAsync` | Yes | Auto-sets UpdatedDate |
| `BulkUpdateAsync` | Yes | Auto-sets UpdatedDate |
| `DeleteAsync` | Yes | Hard delete |
| `BulkDeleteAsync` | Yes | Hard delete |
| `SoftDeleteAsync` | Yes | Requires EntityBaseAudit |
| `BulkSoftDeleteAsync` | Yes | Requires EntityBaseAudit |
