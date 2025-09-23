void rectangleSDF_float(float2 UV, float2 Size, out float Dist)
{
    float2 d = abs(UV) - Size;
    Dist = length(max(d, 0)) + min(max(d.x, d.y), 0);
}
void OutlineSDF_float(float Distance, float thickness, out float Dist)
{
    Dist = abs(Distance) - thickness;
}