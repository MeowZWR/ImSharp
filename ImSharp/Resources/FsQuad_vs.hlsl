#include "include/FsQuad.hlsli"

fs_quad_vertex main(uint id : SV_VertexID)
{
    fs_quad_vertex vertex;
    vertex.uv       = float2(id & 1, (id & 2) >> 1);
    vertex.position = float4(vertex.uv * float2(2.0, -2.0) + float2(-1.0, 1.0), 0.0, 1.0);
    return vertex;
}
