// RoundLine.fx
// Originally By Michael D. Anderson
// Based on Version 3.00, Mar 12 2009
// Modified by Andrew Russell
//
// Note that there is a (rho, theta) pair, used in the VS, that tells how to 
// scale and rotate the entire line.  There is also a different (rho, theta) 
// pair, used within the PS, that indicates what part of the line each pixel 
// is on.


// Data shared by all lines:
matrix viewProj;
float inverseScaleRadius = 1;

// Per-line instance data:
// Number of elements must match RoundLineManager.linesPerBatch in RoundLine.cs
// VS 2.0 has a minimum of 256 float4 registers
float4 instancePosition[104]; // (x0, y0, x1, y1)
float4 instanceColor[104]; // rgba
float4 instanceRadius[26]; // packed radius (4 per element)


struct VS_INPUT
{
	float4 pos : POSITION;
	float2 vertRhoTheta : NORMAL;
	float2 vertScaleTrans : TEXCOORD0;
	float instanceIndex : TEXCOORD1;
};


struct VS_OUTPUT
{
	float4 position : POSITION;
	float4 color : COLOR0;
	float3 polar : TEXCOORD0;
	float2 posModelSpace : TEXCOORD1;
};


VS_OUTPUT MyVS( VS_INPUT In )
{
	VS_OUTPUT Out = (VS_OUTPUT)0;
	float4 pos = In.pos;

	float x0 = instancePosition[In.instanceIndex].x;
	float y0 = instancePosition[In.instanceIndex].y;
	float x1 = instancePosition[In.instanceIndex].z;
	float y1 = instancePosition[In.instanceIndex].w;
	float lineRadius = instanceRadius[In.instanceIndex / 4][In.instanceIndex % 4] / inverseScaleRadius;
	float4 color = instanceColor[In.instanceIndex];
	
	float2 delta = float2(x1, y1) - float2(x0, y0);	
	float rho = length(delta);
	float theta = atan2(delta.y, delta.x);
	
	// Scale X by lineRadius, and translate X by rho, in worldspace
	// based on what part of the line we're on
	float vertScale = In.vertScaleTrans.x;
	float vertTrans = In.vertScaleTrans.y;
	pos.x *= (vertScale * lineRadius);
	pos.x += (vertTrans * rho);

	// Always scale Y by lineRadius regardless of what part of the line we're on
	pos.y *= lineRadius;
	
	// Now the vertex is adjusted for the line length and radius, and is 
	// ready for the usual world/view/projection transformation.

	// World matrix is rotate(theta) * translate(p0)
	matrix worldMatrix = 
	{
		cos(theta), sin(theta), 0, 0,
		-sin(theta), cos(theta), 0, 0,
		0, 0, 1, 0,
		x0, y0, 0, 1 
	};
	
	Out.position = mul(mul(pos, worldMatrix), viewProj);
	
	Out.color = color;
	
	Out.polar = float3(In.vertRhoTheta, 0);

	Out.posModelSpace.xy = pos.xy;

	return Out;
}


// Helper function used by several pixel shaders to blur the line edges
float BlurEdge( float rho )
{
	const float blurThreshold = 0.85;
	if( rho < blurThreshold )
	{
		return 1.0f;
	}
	else
	{
		float normrho = (rho - blurThreshold) * 1 / (1 - blurThreshold);
		return 1 - normrho;
	}
}


float4 MyPSStandard( float3 polar : TEXCOORD0, float4 lineColor : COLOR0 ) : COLOR0
{
	float4 finalColor;
	finalColor.rgb = lineColor.rgb;
	finalColor.a = lineColor.a * BlurEdge( polar.x );
	return finalColor;
}


technique Standard
{
	pass P0
	{
		CullMode = None;
		AlphaBlendEnable = true;
		SrcBlend = SrcAlpha;
		DestBlend = InvSrcAlpha;
		BlendOp = Add;
		vertexShader = compile vs_2_0 MyVS();
		pixelShader = compile ps_2_0 MyPSStandard();
	}
}

