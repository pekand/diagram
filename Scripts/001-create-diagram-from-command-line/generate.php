<?php

$schema = array(
  "/diagram" => "",
  "/diagram/options" => "",
  "/diagram/options/shiftx" => "0",
  "/diagram/options/shifty" => "0",
  "/diagram/options/firstLayereShift.x" => "0",
  "/diagram/options/firstLayereShift.y" => "0",
  "/diagram/options/window.position.width" => "100",
  "/diagram/options/window.position.height" => "100",
  "/diagram/options/window.state" => "0",

  "/diagram/rectangles" => "",

  "/diagram/rectangles/rectangle#1" => "",
  "/diagram/rectangles/rectangle#1/id" => "1",
  "/diagram/rectangles/rectangle#1/font" => "Open Sans; 10pt",
  "/diagram/rectangles/rectangle#1/fontcolor" => "Black",
  "/diagram/rectangles/rectangle#1/text" => "test",
  "/diagram/rectangles/rectangle#1/layer" => "0",
  "/diagram/rectangles/rectangle#1/x" => "685",
  "/diagram/rectangles/rectangle#1/y" => "319",
  "/diagram/rectangles/rectangle#1/color" => "#FFFFB8",
  "/diagram/rectangles/rectangle#1/timecreate" => "2015-5-23 20:33:28",
  "/diagram/rectangles/rectangle#1/timecreate" => "2015-5-23 20:33:28",  

  "/diagram/rectangles/rectangle#2" => "",
  "/diagram/rectangles/rectangle#2/id" => "2",
  "/diagram/rectangles/rectangle#2/font" => "Open Sans; 10pt",
  "/diagram/rectangles/rectangle#2/fontcolor" => "Black",
  "/diagram/rectangles/rectangle#2/text" => "test2",
  "/diagram/rectangles/rectangle#2/layer" => "0",
  "/diagram/rectangles/rectangle#2/x" => "860",
  "/diagram/rectangles/rectangle#2/y" => "344",
  "/diagram/rectangles/rectangle#2/color" => "#FFFFB8",
  "/diagram/rectangles/rectangle#2/timecreate" => "2015-5-23 20:33:28",
  "/diagram/rectangles/rectangle#2/timecreate" => "2015-5-23 20:33:28",  

  "/diagram/rectangles/rectangle#3" => "",
  "/diagram/rectangles/rectangle#3/id" => "3",
  "/diagram/rectangles/rectangle#3/font" => "Open Sans; 10pt",
  "/diagram/rectangles/rectangle#3/fontcolor" => "Black",
  "/diagram/rectangles/rectangle#3/text" => "test3",
  "/diagram/rectangles/rectangle#3/layer" => "0",
  "/diagram/rectangles/rectangle#3/x" => "755",
  "/diagram/rectangles/rectangle#3/y" => "430",
  "/diagram/rectangles/rectangle#3/color" => "#FFFFB8",
  "/diagram/rectangles/rectangle#3/timecreate" => "2015-5-23 20:33:28",
  "/diagram/rectangles/rectangle#3/timecreate" => "2015-5-23 20:33:28",

  "/diagram/lines" => "",

  "/diagram/lines/line#1" => "",
  "/diagram/lines/line#1/start" => "3",
  "/diagram/lines/line#1/end" => "1",
  "/diagram/lines/line#1/arrow" => "0",  

  "/diagram/lines/line#2" => "",
  "/diagram/lines/line#2/start" => "3",
  "/diagram/lines/line#2/end" => "2",
  "/diagram/lines/line#2/arrow" => "0",  

  "/diagram/lines/line#3" => "",
  "/diagram/lines/line#3/start" => "2",
  "/diagram/lines/line#3/end" => "1",
  "/diagram/lines/line#3/arrow" => "0",
);

function buildTree(&$schema)
{
    $tree = array(
        'data' => null,
        'atributes' => null,
        'tree' => null
    );

    foreach ($schema as $node => $data) {

        $parts = explode("?", $node);

        $struct = array();
        if (isset($parts[0]))
            $struct = explode("/", $parts[0]);

        $atribute = null;
        if (isset($parts[1]))
            $atribute = $parts[1];

        $treePart = &$tree;
        foreach ($struct as $part) {
            if(!empty($part)) {
                if(!isset($treePart['tree'][$part])) {
                    $treePart['tree'][$part] = array(
                        'data' => null,
                        'atributes' => null,
                        'tree' => null
                    );
                }
                $treePart = &$treePart['tree'][$part];
            }
        }

        if (!empty($atribute)) {
            $treePart['atributes'][$atribute] = $data;
        } else {
            $treePart['data'] = $data;
        }
    }

    return $tree;
}

function buildXmlFromTree(&$xml, &$xmlNode, &$tree)
{
    foreach ($tree['tree'] as $name => $node) {

        $name = explode("#", $name)[0];
        $newNode = $xml->createElement($name, $node['data']);

        if (!empty($node['atributes'])) {
            foreach ($node['atributes'] as $attribute => $value) {
                $newAttribute = $xml->createAttribute($attribute);
                $newAttribute->value = $value;
                $newNode->appendChild($newAttribute);
            }
        }

        $newXmlNode = $xmlNode->appendChild($newNode);

        if (!empty($node['tree'])) {
            buildXmlFromTree($xml, $newXmlNode, $node);
        }
    }

}

function buildXml(&$tree)
{
    $domtree = new DOMDocument('1.0', 'UTF-8');
    $domtree->preserveWhiteSpace = false;
    $domtree->formatOutput = true;
    if (!empty($tree)) {
        buildXmlFromTree($domtree, $domtree, $tree);
    }
    return $domtree->saveXML();
}



$tree = buildTree($schema);
//echo json_encode($tree, JSON_PRETTY_PRINT);
//echo buildXml($tree);
file_put_contents("out.diagram", buildXml($tree));