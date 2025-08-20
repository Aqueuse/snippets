using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Layout/Snake Layout Group")]
public class SnakeLayoutGroup : LayoutGroup {
    public int maxPerLine = 5;
    public float spacing = 10f;
    public Vector2 cellSize = new Vector2(100, 100);
    
    public enum SnakeDirection {
        BottomToTop,
        TopToBottom,
        LeftToRight,
        RightToLeft
    }

    public SnakeDirection direction = SnakeDirection.RightToLeft;

    public override void CalculateLayoutInputVertical() { }

    public override void SetLayoutHorizontal() {
        SetCells();
    }

    public override void SetLayoutVertical() {
        SetCells();
    }

    private void SetCells() {
        int childCount = rectChildren.Count;
        if (childCount == 0) return;

        int columns, rows;

        if (direction == SnakeDirection.LeftToRight || direction == SnakeDirection.RightToLeft) {
            columns = Mathf.CeilToInt((float)childCount / maxPerLine);
            rows = Mathf.Min(maxPerLine, childCount);
        } else {
            rows = Mathf.CeilToInt((float)childCount / maxPerLine);
            columns = Mathf.Min(maxPerLine, childCount);
        }

        float totalWidth = columns * cellSize.x + (columns - 1) * spacing;
        float totalHeight = rows * cellSize.y + (rows - 1) * spacing;

        Vector2 startOffset = GetStartOffset(totalWidth, totalHeight);

        // Détermine le sens vertical en fonction de l'anchor
        int verticalDirection = 1;
        switch (childAlignment) {
            case TextAnchor.UpperLeft:
            case TextAnchor.UpperCenter:
            case TextAnchor.UpperRight:
                verticalDirection = 1; // descend
                break;
            case TextAnchor.LowerLeft:
            case TextAnchor.LowerCenter:
            case TextAnchor.LowerRight:
                verticalDirection = -1; // monte
                break;
            case TextAnchor.MiddleLeft:
            case TextAnchor.MiddleCenter:
            case TextAnchor.MiddleRight:
                verticalDirection = 1; // descend par défaut
                break;
        }

        for (int i = 0; i < childCount; i++) {
            RectTransform child = rectChildren[i];
            int col, row;

            switch (direction) {
                case SnakeDirection.BottomToTop:
                    col = i / maxPerLine;
                    row = i % maxPerLine;
                    if (col % 2 == 1) row = maxPerLine - 1 - row;
                    break;

                case SnakeDirection.TopToBottom:
                    col = i / maxPerLine;
                    row = i % maxPerLine;

                    int totalCols = Mathf.CeilToInt((float)childCount / maxPerLine);
                    bool isLastCol = col == totalCols - 1;

                    if (isLastCol) {
                        int itemsThisCol = childCount - col * maxPerLine;
                        row = itemsThisCol - 1 - row;
                    } else {
                        row = maxPerLine - 1 - row;
                    }

                    if (col % 2 == 1) row = maxPerLine - 1 - row;
                    break;

                case SnakeDirection.LeftToRight:
                    col = i % maxPerLine;
                    row = i / maxPerLine;
                    if (row % 2 == 1) col = maxPerLine - 1 - col;
                    break;

                case SnakeDirection.RightToLeft:
                    row = i / maxPerLine;
                    col = i % maxPerLine;

                    int totalRows = Mathf.CeilToInt((float)childCount / maxPerLine);
                    bool isLastRow = row == totalRows - 1;

                    if (isLastRow) {
                        int itemsThisRow = childCount - row * maxPerLine;
                        col = itemsThisRow - 1 - col;
                    } else {
                        col = maxPerLine - 1 - col;
                    }

                    if (row % 2 == 1) col = maxPerLine - 1 - col;
                    break;

                default:
                    col = 0; row = 0;
                    break;
            }

            float x = startOffset.x + col * (cellSize.x + spacing);
            float y = startOffset.y + verticalDirection * row * (cellSize.y + spacing);

            SetChildAlongAxis(child, 0, x, cellSize.x);
            SetChildAlongAxis(child, 1, y, cellSize.y);
        }
    }

    private Vector2 GetStartOffset(float totalWidth, float totalHeight) {
        float x = 0f, y = 0f;
        float rectW = rectTransform.rect.width;
        float rectH = rectTransform.rect.height;

        switch (childAlignment) {
            case TextAnchor.UpperLeft:
                x = padding.left;
                y = padding.top;
                break;
            case TextAnchor.UpperCenter:
                x = (rectW - totalWidth) / 2f;
                y = padding.top;
                break;
            case TextAnchor.UpperRight:
                x = rectW - totalWidth - padding.right;
                y = padding.top;
                break;

            case TextAnchor.MiddleLeft:
                x = padding.left;
                y = rectH / 2f;
                break;
            case TextAnchor.MiddleCenter:
                x = (rectW - totalWidth) / 2f;
                y = rectH / 2f;
                break;
            case TextAnchor.MiddleRight:
                x = rectW - totalWidth - padding.right;
                y = rectH / 2f;
                break;

            case TextAnchor.LowerLeft:
                x = padding.left;
                y = rectH - cellSize.y - padding.top;
                break;
            case TextAnchor.LowerCenter:
                x = (rectW - totalWidth) / 2f;
                y = rectH - cellSize.y - padding.top;
                break;
            case TextAnchor.LowerRight:
                x = rectW - totalWidth - padding.right;
                y = rectH - cellSize.y - padding.top;
                break;
        }

        return new Vector2(x, y);
    }
}
