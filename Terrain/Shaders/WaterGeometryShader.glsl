#version 330
#extension GL_NV_tessellation_shader : enable

//layout (triangles) in;
//layout (triangle_strip, max_vertices = 3) out;

void main()
{
	gl_Position = gl_PositionIn[0];
	EmitVertex();

	gl_Position = gl_PositionIn[1];
	EmitVertex();

	gl_Position = gl_PositionIn[2];
	EmitVertex();

	EndPrimitive();
}