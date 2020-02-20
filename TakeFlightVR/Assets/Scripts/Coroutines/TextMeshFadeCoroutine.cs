using UnityEngine;
using System.Collections;

public class TextMeshFadeCoroutine
{
    public static IEnumerator Fade(TextMesh mesh, float fromAlpha, float toAlpha, float duration)
    {
        var elapsedTime = 0f;
        mesh.color = new Color(mesh.color.r, mesh.color.g, mesh.color.b, fromAlpha);
        var startingColor = mesh.color;
        var targetColor = new Color(mesh.color.r, mesh.color.g, mesh.color.b, toAlpha);
        while (elapsedTime < duration)
        {
            var timeRatio = elapsedTime / duration;
            mesh.color = Color.Lerp(startingColor, targetColor, timeRatio);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        mesh.color = targetColor;
    }
}
