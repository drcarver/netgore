using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore
{
    /// <summary>
    /// Provides an optimized tracking grid that allows for faster look-up of Entities on a map based off of
    /// a variety of parameters.
    /// </summary>
    public class MapEntityGrid : IMapEntityCollection
    {
        /// <summary>
        /// Size of each segment of the wall grid in pixels (smallest requires more
        /// memory but often less checks (to an extent))
        /// </summary>
        protected const int WallGridSize = 128;

        /// <summary>
        /// Two-dimensional grid of references to entities in that sector.
        /// </summary>
        List<Entity>[,] _entityGrid;

        protected IEnumerable<Entity> this[int x, int y]
        {
            get
            {
                if (!IsLegalGridIndex(x, y))
                    return Enumerable.Empty<Entity>();

                return _entityGrid[x, y];
            }
        }

        protected IEnumerable<Entity> this[Point gridIndex]
        {
            get { return this[gridIndex.X, gridIndex.Y]; }
        }

        public Point GridSize
        {
            get { return new Point(_entityGrid.GetLength(0), _entityGrid.GetLength(1)); }
        }

        public void Add(IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                Add(entity);
            }
        }

        /// <summary>
        /// Adds an <see cref="Entity"/> to the grid.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> to add to the grid.</param>
        public void Add(Entity entity)
        {
            var minX = (int)entity.CB.Min.X / WallGridSize;
            var minY = (int)entity.CB.Min.Y / WallGridSize;
            var maxX = (int)entity.CB.Max.X / WallGridSize;
            var maxY = (int)entity.CB.Max.Y / WallGridSize;

            // Keep in range of the grid
            if (minX < 0)
                minX = 0;

            if (maxX >= GridSize.X)
                maxX = GridSize.X - 1;

            if (minY < 0)
                minY = 0;

            if (maxY >= GridSize.Y)
                maxY = GridSize.Y - 1;

            // Add to all the segments of the grid
            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y <= maxY; y++)
                {
                    if (!_entityGrid[x, y].Contains(entity))
                        _entityGrid[x, y].Add(entity);
                }
            }
        }

        /// <summary>
        /// Builds a two-dimensional array of Lists to use as the grid of entities.
        /// </summary>
        /// <returns>A two-dimensional array of Lists to use as the grid of entities.</returns>
        static List<Entity>[,] BuildEntityGrid(float width, float height)
        {
            var gridWidth = (int)Math.Ceiling(width / WallGridSize);
            var gridHeight = (int)Math.Ceiling(height / WallGridSize);

            // Create the array
            var retGrid = new List<Entity>[gridWidth,gridHeight];

            // Create the lists
            for (var x = 0; x < gridWidth; x++)
            {
                for (var y = 0; y < gridHeight; y++)
                {
                    retGrid[x, y] = new List<Entity>();
                }
            }

            return retGrid;
        }

        static bool EmptyPred<T>(T e)
        {
            return true;
        }

        /// <summary>
        /// Gets the grid segment for each intersection on the entity grid.
        /// </summary>
        /// <param name="rect">Map area to get the grid segments for.</param>
        /// <returns>An IEnumerable of all grid segments intersected by the specified <paramref name="rect"/>.</returns>
        protected IEnumerable<IEnumerable<Entity>> GetEntityGrids(Rectangle rect)
        {
            var minX = rect.X / WallGridSize;
            var minY = rect.Y / WallGridSize;
            var maxX = rect.Right / WallGridSize;
            var maxY = rect.Bottom / WallGridSize;

            // Keep in range of the grid
            if (minX < 0)
                minX = 0;

            if (maxX >= GridSize.X)
                maxX = GridSize.X - 1;

            if (minY < 0)
                minY = 0;

            // NOTE: For some reason this last check here likes to fail a lot. I think it is because gravity pushes an Entity out of the map temporarily when they are down low. Ideally, this condition is NEVER reached.
            if (maxY >= GridSize.Y)
                maxY = GridSize.Y - 1;

            // Return the grid segments
            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y <= maxY; y++)
                {
                    yield return this[x, y];
                }
            }
        }

        /// <summary>
        /// Gets if the given point is a legal grid index.
        /// </summary>
        /// <param name="point">The grid x and y co-ordinate.</param>
        /// <returns>True if the given point is a legal grid index; otherwise false.</returns>
        protected bool IsLegalGridIndex(Point point)
        {
            return IsLegalGridIndex(point.X, point.Y);
        }

        /// <summary>
        /// Gets if the given point is a legal grid index.
        /// </summary>
        /// <param name="x">The grid x co-ordinate.</param>
        /// <param name="y">The grid y co-ordinate.</param>
        /// <returns>True if the given point is a legal grid index; otherwise false.</returns>
        protected bool IsLegalGridIndex(int x, int y)
        {
            return x >= 0 && x < _entityGrid.GetLength(0) && y >= 0 && y < _entityGrid.GetLength(1);
        }

        static Point MapPositionToGridIndex(Vector2 position)
        {
            var x = position.X / WallGridSize;
            var y = position.Y / WallGridSize;
            return new Point((int)x, (int)y);
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        IEnumerable<Entity> MutableGetEntities(Rectangle rect, Predicate<Entity> condition)
        {
            var gridSegments = GetEntityGrids(rect);
            var matches = gridSegments.SelectMany(x => x).Where(x => x.CB.Intersect(rect) && condition(x)).Distinct();
            return matches;
        }

        /// <summary>
        /// Gets all entities at the given point.
        /// </summary>
        /// <param name="p">The point to find the entities at.</param>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to look for.</typeparam>
        /// <returns>All entities containing the given point that are of the given type.</returns>
        IEnumerable<T> MutableGetEntities<T>(Vector2 p, Predicate<T> condition) where T : Entity
        {
            var gridSegment = this[MapPositionToGridIndex(p)];
            var matches = gridSegment.OfType<T>().Where(x => x.CB.HitTest(p) && condition(x));
            Debug.Assert(!matches.HasDuplicates(), "Somehow we had duplicates even though we only used one grid segment!");
            return matches;
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <typeparam name="T">Type of Entity to look for.</typeparam>
        /// <returns>All Entities found intersecting the given region.</returns>
        IEnumerable<T> MutableGetEntities<T>(Rectangle rect, Predicate<T> condition) where T : Entity
        {
            var gridSegments = GetEntityGrids(rect);
            var matches = gridSegments.SelectMany(x => x).OfType<T>().Where(x => x.CB.Intersect(rect) && condition(x)).Distinct();
            return matches;
        }

        /// <summary>
        /// Gets all entities containing a given point.
        /// </summary>
        /// <param name="p">Point to find the entities at.</param>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <returns>All of the entities at the given point.</returns>
        IEnumerable<Entity> MutableGetEntities(Vector2 p, Predicate<Entity> condition)
        {
            var gridSegment = this[MapPositionToGridIndex(p)];
            var matches = gridSegment.Where(x => x.CB.HitTest(p) && condition(x));
            Debug.Assert(!matches.HasDuplicates(), "Somehow we had duplicates even though we only used one grid segment!");
            return matches;
        }

        public void Remove(Entity entity)
        {
            foreach (var gridSegment in _entityGrid)
            {
                gridSegment.Remove(entity);
            }
        }

        /// <summary>
        /// Sets the source map to a new size and clears out all existing entities in the grid.
        /// </summary>
        /// <param name="size">The new size of the source map.</param>
        public void SetMapSize(Vector2 size)
        {
            _entityGrid = BuildEntityGrid(size.X, size.Y);
        }

        public void UpdateEntity(Entity entity, Vector2 oldPos)
        {
            // FUTURE: Can optimize this method quite a lot by only adding/removing from changed grid segments

            // Check that the entity changed grid segments by comparing the lowest grid segments
            // of the old position and current position
            var minX = (int)oldPos.X / WallGridSize;
            var minY = (int)oldPos.Y / WallGridSize;
            var newMinX = (int)entity.CB.Min.X / WallGridSize;
            var newMinY = (int)entity.CB.Min.Y / WallGridSize;

            if (minX == newMinX && minY == newMinY)
                return; // No change in grid segment

            var maxX = (int)(oldPos.X + entity.CB.Width) / WallGridSize;
            var maxY = (int)(oldPos.Y + entity.CB.Height) / WallGridSize;

            // Keep in range of the grid
            if (minX < 0)
                minX = 0;

            if (maxX >= GridSize.X)
                maxX = GridSize.X - 1;

            if (minY < 0)
                minY = 0;

            if (maxY >= GridSize.Y)
                maxY = GridSize.Y - 1;

            // Remove the entity from the old grid position
            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y <= maxY; y++)
                {
                    _entityGrid[x, y].Remove(entity);
                }
            }

            // Re-add the entity to the grid
            Add(entity);
        }

        #region IMapEntityCollection Members

        /// <summary>
        /// Gets the first <see cref="Entity"/> found in the given region.
        /// </summary>
        /// <param name="rect">Region to find the <see cref="Entity"/> in.</param>
        /// <param name="condition">Additional condition an <see cref="Entity"/> must meet.</param>
        /// <param name="condition">Condition the Entities must meet.</param>
        /// <returns>The first <see cref="Entity"/> found in the given region, or null if none found.</returns>
        public T GetEntity<T>(Rectangle rect, Predicate<T> condition) where T : Entity
        {
            return MutableGetEntities(rect, condition).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first <see cref="Entity"/> found in the given region.
        /// </summary>
        /// <param name="rect">Region to find the <see cref="Entity"/> in.</param>
        /// <param name="condition">Additional condition an <see cref="Entity"/> must meet.</param>
        /// <returns>The first <see cref="Entity"/> found in the given region, or null if none found.</returns>
        public Entity GetEntity(Rectangle rect, Predicate<Entity> condition)
        {
            return MutableGetEntities(rect, condition).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first <see cref="Entity"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the entity at.</param>
        /// <param name="condition">Condition the <see cref="Entity"/> must meet.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to look for. Any other type of <see cref="Entity"/>
        /// will be ignored.</typeparam>
        /// <returns>First <see cref="Entity"/> found at the given point, or null if none found.</returns>
        public T GetEntity<T>(Vector2 p, Predicate<T> condition) where T : Entity
        {
            return MutableGetEntities(p, condition).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first <see cref="Entity"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the entity at.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to look for. Any other type of <see cref="Entity"/>
        /// will be ignored.</typeparam>
        /// <returns>First <see cref="Entity"/> found at the given point, or null if none found.</returns>
        public T GetEntity<T>(Vector2 p) where T : Entity
        {
            return MutableGetEntities<T>(p, EmptyPred).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first <see cref="Entity"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the entity at.</param>
        /// <returns>First <see cref="Entity"/> found at the given point, or null if none found.</returns>
        public Entity GetEntity(Vector2 p)
        {
            return MutableGetEntities(p, EmptyPred).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first <see cref="Entity"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the entity at.</param>
        /// <param name="condition">Condition the <see cref="Entity"/> must meet.</param>
        /// <returns>First <see cref="Entity"/> found at the given point, or null if none found.</returns>
        public Entity GetEntity(Vector2 p, Predicate<Entity> condition)
        {
            return MutableGetEntities(p, condition).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first Entity found in the given region
        /// </summary>
        /// <param name="rect">Region to check for the Entity</param>
        /// <returns>First Entity found at the given point, or null if none found</returns>
        public Entity GetEntity(Rectangle rect)
        {
            return MutableGetEntities(rect, EmptyPred).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first Entity found in the given region
        /// </summary>
        /// <param name="rect">Region to check for the Entity</param>
        /// <typeparam name="T">Type to convert to</typeparam>
        /// <returns>First Entity found at the given point, or null if none found</returns>
        public T GetEntity<T>(Rectangle rect) where T : Entity
        {
            return MutableGetEntities<T>(rect, EmptyPred).FirstOrDefault();
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public bool ContainsEntities(Rectangle rect)
        {
            return ContainsEntities(rect, EmptyPred);
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public bool ContainsEntities(Rectangle rect, Predicate<Entity> condition)
        {
            return GetEntityGrids(rect).SelectMany(x => x).Any(x => x.CB.Intersect(rect) && condition(x));
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to check against. All other types of
        /// <see cref="Entity"/> will be ignored.</typeparam>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public bool ContainsEntities<T>(Rectangle rect) where T : Entity
        {
            return ContainsEntities<T>(rect, EmptyPred);
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to check against. All other types of
        /// <see cref="Entity"/> will be ignored.</typeparam>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public bool ContainsEntities<T>(Rectangle rect, Predicate<T> condition) where T : Entity
        {
            return GetEntityGrids(rect).SelectMany(x => x).OfType<T>().Any(x => x.CB.Intersect(rect) && condition(x));
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public bool ContainsEntities(Vector2 point)
        {
            return ContainsEntities(point, EmptyPred);
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public bool ContainsEntities(Vector2 point, Predicate<Entity> condition)
        {
            var gridSegment = this[MapPositionToGridIndex(point)];
            return gridSegment.Any(x => x.CB.HitTest(point) && condition(x));
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to check against. All other types of
        /// <see cref="Entity"/> will be ignored.</typeparam>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public bool ContainsEntities<T>(Vector2 point) where T : Entity
        {
            return ContainsEntities<T>(point, EmptyPred);
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to check against. All other types of
        /// <see cref="Entity"/> will be ignored.</typeparam>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public bool ContainsEntities<T>(Vector2 point, Predicate<T> condition) where T : Entity
        {
            var gridSegment = this[MapPositionToGridIndex(point)];
            return gridSegment.OfType<T>().Any(x => x.CB.HitTest(point) && condition(x));
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<Entity> GetEntities(Rectangle rect)
        {
            return GetEntities(rect, EmptyPred);
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<Entity> GetEntities(Rectangle rect, Predicate<Entity> condition)
        {
            return MutableGetEntities(rect, condition).ToImmutable();
        }

        /// <summary>
        /// Gets all entities at the given point.
        /// </summary>
        /// <param name="p">The point to find the entities at.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to look for.</typeparam>
        /// <returns>All entities containing the given point that are of the given type.</returns>
        public IEnumerable<T> GetEntities<T>(Vector2 p) where T : Entity
        {
            return GetEntities<T>(p, EmptyPred);
        }

        /// <summary>
        /// Gets all entities at the given point.
        /// </summary>
        /// <param name="p">The point to find the entities at.</param>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to look for.</typeparam>
        /// <returns>All entities containing the given point that are of the given type.</returns>
        public IEnumerable<T> GetEntities<T>(Vector2 p, Predicate<T> condition) where T : Entity
        {
            return MutableGetEntities(p, condition).ToImmutable();
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <typeparam name="T">Type of Entity to look for.</typeparam>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<T> GetEntities<T>(Rectangle rect) where T : Entity
        {
            return GetEntities<T>(rect, EmptyPred);
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <typeparam name="T">Type of Entity to look for.</typeparam>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<T> GetEntities<T>(Rectangle rect, Predicate<T> condition) where T : Entity
        {
            return MutableGetEntities(rect, condition).ToImmutable();
        }

        /// <summary>
        /// Gets all entities containing a given point.
        /// </summary>
        /// <param name="p">Point to find the entities at.</param>
        /// <returns>All of the entities at the given point.</returns>
        public IEnumerable<Entity> GetEntities(Vector2 p)
        {
            return GetEntities(p, EmptyPred);
        }

        /// <summary>
        /// Gets all entities containing a given point.
        /// </summary>
        /// <param name="p">Point to find the entities at.</param>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <returns>All of the entities at the given point.</returns>
        public IEnumerable<Entity> GetEntities(Vector2 p, Predicate<Entity> condition)
        {
            return MutableGetEntities(p, condition).ToImmutable();
        }

        #endregion
    }
}